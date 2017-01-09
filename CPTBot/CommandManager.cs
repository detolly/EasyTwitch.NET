using System;

namespace CPTBot
{
	[Serializable()]
	class CommandManager {

		private string addCommandTrigger, removeCommandTrigger, commandListTrigger;
		private Command[] commands;
		private int throttle {
			get;
			set;
		}
		private DateTime lastCommandResponseSent;

		public CommandManager() {
			commands = new Command[0];
			throttle = 0;
		}

		public void AddCommand(string trigger, string response) {
			foreach (Command c in commands) {
				if (c.getTrigger () == trigger)
					throw new Exception ("This command already exists.");
			}
			if (commands.Length == 0) {
				commands = new Command[1];
				commands [0] = new Command (trigger, response);
			} else {
				Command[] temp = commands;
				commands = new Command[temp.Length + 1];
				for (int i = 0; i < temp.Length + 1; i++) {
					if (i != temp.Length)
						commands [i] = temp [i];
					else {
						commands [i] = new Command (trigger, response);
					}
				}
			}
			SM.Save (this);
		}

		public void setThrottle(int throttle) {
			this.throttle = throttle;
		}

		public void CheckCommands(string message, ConnectionManager p) {
			if (addCommandTrigger.Length < 1 || removeCommandTrigger.Length < 1 || commandListTrigger.Length < 1) throw new Exception("You have to set up the CommandManager with the CommandManager.setup() method before you can use it.");
			if (message.StartsWith (addCommandTrigger + " ")) {
				AddCommand (message.Split (' ') [1], message.Remove (0, addCommandTrigger.Length + 2 + message.Split (' ') [1].Length));
				p.SendMessage ("Added command " + message.Split (' ') [1] + ".");
			} else if (message.StartsWith (commandListTrigger) || message == commandListTrigger) {
				string response = "Current commands: ";
				foreach (Command c in this.commands) {
					response += c.getTrigger () + ", ";
				}
				response = response.Remove (response.Length - 2, 2);
				response += ".";
				p.SendMessage (response);
			} else if (message.StartsWith (removeCommandTrigger + " ")) {
				RemoveCommand (message.Split (' ') [1]);
			} else if (DateTime.Now - lastCommandResponseSent > TimeSpan.FromSeconds(throttle)){
				foreach (Command c in this.getCommands())
					if (c.checkTrigger (message))
						p.SendMessage (c.getResponse);
				lastCommandResponseSent = DateTime.Now;
			}
		}

		public void setup(string addCommandTrigger, string removeCommandTrigger, string commandListTrigger) {
			this.addCommandTrigger = addCommandTrigger;
			this.removeCommandTrigger = removeCommandTrigger;
			this.commandListTrigger = commandListTrigger;
		}

		public void RemoveCommand(string commandTrigger) {
			Command[] temp = commands;
			commands = new Command[temp.Length - 1];
			Command c = new Command("r", "r");
			for (int i = 0; i < temp.Length; i++) {
				if (temp [i].getTrigger() == commandTrigger)
					c = temp [i];
			}
			bool d = false;
			for (int i = 0; i < temp.Length -1; i++) {
				if (d) {
					commands [i] = temp [i + 1];
				}
				if (temp[i].getTrigger() != c.getTrigger())
					commands [i] = temp [i];
				else {
					i--;
					d = true;
				}
			}
		}

		public bool commandExists(string trigger) {
			foreach (var command in commands)
				if (command.getTrigger() == trigger)
					return true;
			return false;
		}

		public Command[] getCommands() {
			return commands;
		}

	}

}

