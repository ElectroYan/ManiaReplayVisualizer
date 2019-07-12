using osu_database_reader.Components.Player;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuReplayVisualizer
{
	class ManiaParser : Parser
	{

		public override void ReadReplay(string path)
		{
			Replay replay = Replay.Read(path);
			bool[] keyPressed = new bool[10];
			int[] holdDuration = new int[10];
			int[] startTiming = new int[10];
			ReplayFile = new List<HitObject>();
			List<ReplayFrame> frames = replay.ReplayFrames.Where(x => x.TimeAbs > 0).ToList();
			int keyCount = frames.Select(x=> Convert.ToString((int)x.X, 2).Length).Max();
			for (int j = 0; j < frames.Count(); j++)
			{
				ReplayFrame item = frames[j];
				string x = String.Join("", Convert.ToString((int)item.X, 2).ToCharArray().Reverse().ToArray());
				x = x.PadRight(keyCount, '0');
				//Console.WriteLine(x);

				char[] key = x.ToCharArray();

				for (int i = 0; i < key.Count(); i++)
				{
					if (key[i] == '1')
					{
						if (!keyPressed[i])
							startTiming[i] = item.TimeAbs;
						keyPressed[i] = true;
						if (j < frames.Count()-1)
							holdDuration[i] += frames[j+1].TimeDiff;
					}
					else
					{
						if (keyPressed[i])
						{
							ReplayFile.Add(new HitObject(startTiming[i], i, holdDuration[i]));
							holdDuration[i] = 0;
						}
						keyPressed[i] = false;
					}
				}
			}
			//Console.WriteLine(ReplayFile.Count());
			//foreach (var item in ReplayFile)
			//{
			//	Console.WriteLine(item.Timing + "\t" + item.Key);
			//}
		}

		public override void ReadBeatmap(List<string> file, int keyCount)
		{
			BeatmapFile = AddNotes(keyCount, file);
		}

		private List<HitObject> AddNotes(int keyCount, List<string> file)
		{
			List<int> columns = ManiaColumns.GetColumnsByKeycount(keyCount);
			bool hitObjects = false;
			List<HitObject> objects = new List<HitObject>();
			foreach (string entry in file)
			{
				if (hitObjects)
				{//add or subtract first timing point maybe.
					string[] parts = entry.Split(',');
					objects.Add(new HitObject(int.Parse(parts[2]), columns.IndexOf(int.Parse(parts[0])), Math.Max(0,int.Parse(parts[5].Split(':')[0])-int.Parse(parts[2]))));
				}
				if (entry.StartsWith("[HitObjects]"))
					hitObjects = true;
			}
			return objects;
		}
	}
}
