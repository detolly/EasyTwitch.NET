using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace CPTBot
{
	public class ConnectionManager
	{
		private string Username, OAuth, Channel;
		private TcpClient tcp;
		private StreamWriter writer;
		private StreamReader reader;

		public ConnectionManager (string Username, string OAuth, Action Callback)
		{
			Construct (Username, OAuth);
			Callback ();
		}

		public ConnectionManager (string Username, string OAuth)
		{
			Construct (Username, OAuth);
		}

		private void Construct(string Username, string OAuth) {
			this.Username = Username;
			this.OAuth = OAuth;
		}

		public void Connect(Action Callback) {
			insertConnection ();
			Callback ();
		}

		public void Connect() {
			insertConnection ();
		}

		private void insertConnection() {
			string ip = "irc.chat.twitch.tv";
			int port = 6667;

			tcp = new TcpClient ();
			tcp.Connect (ip, port);

			this.writer = new StreamWriter (tcp.GetStream());
			this.reader = new StreamReader (tcp.GetStream());

			string login = "PASS " + OAuth + "\r\nNICK "+ Username +"\r\n";
			writer.WriteLine (login);
			writer.Flush ();
		}

		public void Join(string channel, Action Callback) {
			iJoin (channel);
			Callback ();
		}

		public void Join(string channel) {
			iJoin (channel);
		}

		private void iJoin(string channel) {
			this.Channel = channel;
			writer.WriteLine ("JOIN #"+  channel);
			writer.Flush ();
		}

		public void ReceiveMessages(Action<Message> Callback) {
			if (tcp != null && writer != null && reader != null) {
				while (true) {
					while (reader.Peek () > 0 || tcp.Available > 0) {
						string raw = reader.ReadLine ();
						if (raw.Contains ("PRIVMSG")) {
							int iAt = raw.IndexOf ('@') + 1;
							int iDot = raw.IndexOf ('.', iAt);
							int iM = raw.IndexOf (':', iAt);
							string message = raw.Substring (iM + 1);
							string name = raw.Substring (iAt, iDot - iAt);
							Callback (new Message (name, message));
						} else if (raw.Contains ("PING")) {
							writer.Write("PONG tmi.twitch.tv\r\n");
							writer.Flush ();
						}
					}
				}
			}
		}

		public void SendMessage(string message) {
			string userName = Username;
			writer.WriteLine (":" + userName + "!" + userName + "@" + userName + ".tmi.twitch.tv PRIVMSG #" + Channel + " :" + message);
			writer.Flush ();
		}
	}
}

