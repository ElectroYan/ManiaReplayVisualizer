using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuReplayVisualizer
{
	class ScrollVisualizer : Visualizer
	{
		public int ReplayXStart = 200;
		public int BeatmapXStart = 150;
		public int ReplayYStart = 200;
		public int BeatmapYStart = 300;
		public int ColumnSpacing { get => Math.Abs(ReplayXStart - BeatmapXStart); private set { } }
		public int RowSpacing { get => Math.Abs(ReplayYStart - BeatmapYStart); private set { } }
		public int ScrollSpeed { get; set; }
	}
}
