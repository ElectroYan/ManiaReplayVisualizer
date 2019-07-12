using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu_database_reader;
using osu_database_reader.Components.Player;

namespace OsuReplayVisualizer
{
	class TaikoParser : Parser
	{
		public override void ReadReplay(string path)
		{
			Replay replay = Replay.Read(path);
			ReplayFile = new List<HitObject>();
			bool[] keyPressed = new bool[4];
			for (int i = 0; i < replay.ReplayFrames.Count(); i++)
			{
				ReplayFrame frame = replay.ReplayFrames[i];
				int keyValue = GetSingleNumber(frame.Keys);
				string x = string.Join("", Convert.ToString(keyValue, 2).ToCharArray().Reverse().ToArray());
				x = x.PadRight(4, '0');
				if (x.Length > 4)
					continue;
				char[] key = x.ToCharArray();

				for (int j = 0; j < key.Count(); j++)
				{
					if (key[j] == '1')
					{
						if (!keyPressed[j])
							ReplayFile.Add(new HitObject(frame.TimeAbs, j));
						keyPressed[j] = true;
					}
					else
					{
						keyPressed[j] = false;
					}

				}

			}
			//M2 is the outer left key while M1 the inner. But 2 is assigned assigned to M2 thus it's seen as the inner left and vice versa
			SwitchKey0And1BecauseOsuSucks();
		}

		private void SwitchKey0And1BecauseOsuSucks()
		{
			for (int i = 0; i < ReplayFile.Count(); i++)
			{
				if (ReplayFile[i].Key == 1)
					ReplayFile[i].Key = -1;
				if (ReplayFile[i].Key == 0)
					ReplayFile[i].Key = -2;
			}
			for (int i = 0; i < ReplayFile.Count(); i++)
			{
				if (ReplayFile[i].Key == -1)
					ReplayFile[i].Key = 0;
				if (ReplayFile[i].Key == -2)
					ReplayFile[i].Key = 1;
			}
		}

		private int GetSingleNumber(Keys keys)
		{
			return (int)keys;
		}

		public override void ReadBeatmap(List<string> file, int keyCount)
		{
			BeatmapFile = AddNotes(file);
		}

		private List<HitObject> AddNotes(List<string> file)
		{
			
			bool hitObjects = false;
			List<HitObject> objects = new List<HitObject>();
			foreach (string entry in file)
			{
				if (hitObjects)
				{
					string[] parts = entry.Split(',');
					int key = GetKey(int.Parse(parts[4]));
					objects.Add(new HitObject(int.Parse(parts[2]), key));
				}
				if (entry.StartsWith("[HitObjects]"))
					hitObjects = true;
			}
			return objects;
		}
		private int GetKey(int key)
		{
			int returnVal = 0;
			//red = 0, blue = 1, big red = 2, big blue = 3;
			if (key == 0 || key == 1)
				returnVal = 0;
			else if (key == 2 || key == 3 || key == 8 || key == 9 || key == 10 || key == 11)
				returnVal = 1;
			else if (key == 4 || key == 5)
				returnVal = 2;
			else if (key == 6 || key == 7 || key == 12 || key == 13 || key == 14 || key == 15)
				returnVal = 3;
			return returnVal;
		}
	}
}
