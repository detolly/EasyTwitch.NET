using System;

namespace CPTBot
{
	public struct Message
	{
		public string message {
			get; set;
		}
		public string username {
			get; set;
		}
		public Message(string user, string message) {
			this.message = message;
			this.username = user;
		}
	}
	public static class Extensions {
		public static string[] getArgs(this Message message) {
			return message.message.Split(' ');
		}
		public static string rest(this Message message, int placeInArray) {
			string returnA = message.message;
			int length = 0;
			for (int i = 0; i < message.message.Length; i++) {
				if (i <= placeInArray -1) {
					length += message.message.Split(' ')[i].Length + 1;
				}
			}
			return returnA.Remove (0, length);
		}
	}
}

