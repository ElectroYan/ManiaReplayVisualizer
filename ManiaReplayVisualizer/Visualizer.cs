using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuReplayVisualizer
{
	class Visualizer
	{
		public List<HitObject> ReplayFile;
		public List<HitObject> BeatmapFile;
		public RenderWindow Win = null;
		public readonly int WIN_HEIGHT = 900;
		public readonly int WIN_WIDTH = 1500;
		public int NoteSize { get; set; }

		public Visualizer()
		{
			InitializeSfmlWindow();
		}
		private void InitializeSfmlWindow()
		{
			VideoMode videoMode = new VideoMode((uint)WIN_WIDTH, (uint)WIN_HEIGHT, 24);
			Win = new RenderWindow(videoMode, "Replay Visualizer", Styles.Default);
			Win.Closed += Win_Closed;
		}
		private void Win_Closed(object sender, EventArgs e)
		{
			Win.Close();
		}
	}
}
