using System;

namespace CPTBot
{
	[Serializable()]
	public struct Command
	{
		private string trigger;

		public string getResponse {
			get;
			private set;
		}

		public string getTrigger() {
			return trigger;
		}

		public Command(string trigger, string response) {
			this.trigger = trigger;
			this.getResponse = response;
		}

		public bool checkTrigger(string message) {
			if (message.StartsWith(trigger + " ") || message == trigger)
				return true;
			return false;
		}
	}
}

