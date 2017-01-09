using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CPTBot
{
	static class SM {
		public static void Save(CommandManager m) {
			Stream stream = File.Open("data.bin", FileMode.Create);
			BinaryFormatter formatter = new BinaryFormatter();

			formatter.Serialize(stream, m);
			stream.Close();
		}
		public static CommandManager Load() {
			if (File.Exists ("data.bin") && File.ReadAllBytes("data.bin").Length > 0) {
				Stream stream = File.Open ("data.bin", FileMode.Open);
				BinaryFormatter formatter = new BinaryFormatter ();

				CommandManager obj = (CommandManager)formatter.Deserialize (stream);
				stream.Close ();
				return obj;
			} else
				return new CommandManager ();
		}
	}
}

