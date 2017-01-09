# EasyTwitch.NET
Simple Library for Twitch Bots. Great for learning C#.

EasyTwitch.NET was made entirely for noobs when it comes to coding twitch bots in c#. With this library you can create swift twitch bots in minutes. Custom commands, connection systems, minigames, and more to come. Made by @FlappyFlag.

# Creating your first project with EasyTwitch.NET:

In order to create your first code with EasyTwitch.NET, firstly you're going to have to download the package. You can use Git clone, or you could download it from the link at the bottom of this page.

1. Get the DLL file and copy it to somewhere you will remember it.

2. Create a project if you haven't already done so first.

3. Right-click on your project. Click the listing that says packages, or similar.

4. Locate Add Package.

5. Add Package from File, and locate the DLL file from before.

6. Import, and you're done with the installation.

# Getting to know EasyTwitch.NET
Here's a tutorial c# class that you can learn from. If any questions, you can read the documentation.
```c#
using System;
using EasyTwitchNET;

namespace CPTBot
{
	public class TestClass
	{
		ConnectionManager connectionManager;
		CommandManager commandManager;

		static void Main(string[] args)
		{
			new TestClass ().Run ();
		}

		public void Run() {
			//The CommandManager is a manager that manages our commands. Pretty simple, right?
			//This will be used later on, so no needs to touch it yet. We have to initialize it here though to make it a public variable.
			commandManager = SM.Load();
			setupCommandManager ();

			//Here we initialize our connection to twitch with our credentials, all in one line of code.
			connectionManager = new ConnectionManager ("username", "oauth:xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

			//This is where we do the connection itself.
			connectionManager.Connect(Connected);
		}
			
		public void Connected() {
			
			//Giving ourselves a response, now we can see the program working in the console!
			Console.WriteLine ("Connected to Twitch. Now we Join a channel.\r\n");

			//Whenever we're done joining a channel, we're going to be directly rerouted to the Joined method.
			connectionManager.Join ("savjz", Joined);
		}

		public void Joined() {
			//Giving ourselves a response, now we can see the program working in the console!
			Console.WriteLine ("Joined." + Environment.NewLine);


			//Here we say that an action is going to be done whenever a message is received.
			//The code behind this is a little advanced, so just rolling with it in the start
			//is a good idea.
			Action<Message> receive = (message) => {
				//Do whatever you want with your message here,
				//Or reroute to another void like this ->
				DoSomethingWithString(message);
			};

			//This has to be at the bottom, because it's an infinite loop and will never return anything.
			connectionManager.ReceiveMessages (receive);
		}

		public void DoSomethingWithString(Message message) {
			//Now what we wrote in the "receive" Action, is going to be rerouted in here.
			Console.WriteLine (message.username + ": " + message.message);

			//Now, we can check out the command manager. Check out the setupCommandManager
			//method if you haven't already. Then you can return back here.

			try {
				//This line of code checks the message that we got from the chat for commands.
				//If it finds a command that we have, then it will send the response back to the chat.
				commandManager.CheckCommands (message.message, connectionManager);
			} catch (Exception e){
				//If something went wrong, it's written in the console.
				Console.WriteLine (e.Message);
			}

			//Down here I'm going to have a custom command that makes me able to have a command
			//that I can create more commands from.
			//Remember: This is completely unnecessary in order to have a functioning bot, but I'm
			//just showing youhow to make a 

			if (message.message.StartsWith ("!commandadd ")) {
				//The message.getArgs function returns an array with the message split by a space.
				//Then I'm choosing the second spot of the array, and since it's a stringarray, it's put in
				//a string object.
				string trigger = message.getArgs () [1];
				//This is a custom function i wrote that you're welcome to use, it's basically a function that
				//gets the rest of the message after x many words.
				string response = message.rest (2);
				commandManager.AddCommand (trigger, response);
			}
		}

		public void setupCommandManager() {
			//Remember that CommandManager that we had over in the Run method? We're going to use this here.
			//Down here we can have a CommandManager that will scan the message for commands
			//that YOU can make, and then have it send a response. This can be done like this->

			commandManager.setup ("!addcom", "!delcom", "!commands");

			//if the command !test12345 doesn't exist, create it.
			if (!commandManager.commandExists ("!test12345"))
				commandManager.AddCommand ("!test12345", "This is 100% a test.");
		}
	}
}

