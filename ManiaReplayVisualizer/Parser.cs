using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuReplayVisualizer
{
	abstract class Parser
	{
		public List<HitObject> ReplayFile { get; set; }
		public List<HitObject> BeatmapFile { get; set; }

		public abstract void ReadReplay(string path);
		public abstract void ReadBeatmap(List<string> file, int keyCount);
	}
}
