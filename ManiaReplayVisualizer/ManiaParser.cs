using osu_database_reader.Components.Player;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaReplayVisualizer
{
	class ManiaParser
	{
		public static List<ManiaHitObject> ReplayFile = new List<ManiaHitObject>();
		public static List<ManiaHitObject> BeatmapFile = new List<ManiaHitObject>();
		public static void ReadReplay(string path)
		{
			Replay replay = Replay.Read(path);
			bool[] keyPressed = new bool[10];
			foreach (var item in replay.ReplayFrames.Where(x => x.TimeAbs > 0))
			{
				string x = String.Join("", Convert.ToString((int)item.X, 2).ToCharArray().Reverse().ToArray());
				x += x.Length == 1 ? "000" : (x.Length == 2 ? "00" : (x.Length == 3 ? "0" : ""));
				//Console.WriteLine(x);
				char[] key = x.ToCharArray();

				for (int i = 0; i < key.Count(); i++)
				{
					if (key[i] == '1')
					{
						if (!keyPressed[i])
							ReplayFile.Add(new ManiaHitObject(item.TimeAbs, i));
						keyPressed[i] = true;
					}
					else
						keyPressed[i] = false;
				}
			}
			//Console.WriteLine(ReplayFile.Count());
			//foreach (var item in ReplayFile)
			//{
			//	Console.WriteLine(item.Timing + "\t" + item.Key);
			//}
		}

		public static string ReadBeatmap(string path)
		{
			List<string> file = File.ReadAllLines(path).ToList();
			int keyCount = int.Parse(file.FirstOrDefault(x => x.StartsWith("CircleSize:")).Split(':')[1].Trim());
			string audioFile = path.Substring(0, path.LastIndexOf('\\')+1) + file.FirstOrDefault(x => x.StartsWith("AudioFilename:")).Split(':')[1].Trim();
			BeatmapFile = AddNotes(keyCount, file);
			//foreach (var item in BeatmapFile)
			//{
			//	Console.WriteLine(item.Timing + "\t" + item.Key);
			//}
			return audioFile;
		}

		private static List<ManiaHitObject> AddNotes(int keyCount, List<string> file)
		{
			List<int> columns = ManiaColumns.GetColumnsByKeycount(keyCount);
			bool hitObjects = false;
			List<ManiaHitObject> objects = new List<ManiaHitObject>();
			foreach (string entry in file)
			{
				if (hitObjects)
				{
					string[] parts = entry.Split(',');
					objects.Add(new ManiaHitObject(int.Parse(parts[2]), columns.IndexOf(int.Parse(parts[0])), int.Parse(parts[5].Split(':')[0])));
				}
				if (entry.StartsWith("[HitObjects]"))
					hitObjects = true;
			}
			return objects;
		}
	}
}
