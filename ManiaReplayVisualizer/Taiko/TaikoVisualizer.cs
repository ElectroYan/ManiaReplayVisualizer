using SFML.Audio;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OsuReplayVisualizer
{
	class TaikoVisualizer : ScrollVisualizer
	{
		int HitPosition = 200;
		public TaikoVisualizer(TaikoParser taiko, string audioFile) : base (audioFile)
		{
			BeatmapFile = taiko.BeatmapFile;
			ReplayFile = taiko.ReplayFile;
		}

		public void Start()
		{
			NoteSize = 100;
			ScrollSpeed = 1000;

			Loop();
		}


		protected override void DrawDefaultStuff()
		{
			CircleShape hitPositionCircles = new CircleShape(NoteSize/2);
			hitPositionCircles.OutlineColor = Color.White;
			hitPositionCircles.FillColor = new Color(100,100,100);
			hitPositionCircles.Position = new SFML.System.Vector2f(HitPosition - NoteSize / 2, ReplayYStart - NoteSize / 2);
			Win.Draw(hitPositionCircles);
			hitPositionCircles.Position = new SFML.System.Vector2f(HitPosition - NoteSize / 2, BeatmapYStart - NoteSize / 2);
			Win.Draw(hitPositionCircles);
		}

		protected override void DrawNotes(long elapsedMilliseconds)
		{
			CircleShape replayNote = new CircleShape(NoteSize / 2);
			CircleShape beatmapNote = new CircleShape(NoteSize / 2);
			int actualNoteSize = NoteSize / 2;
			foreach (var item in ReplayFile)
			{
				float xPos = HitPosition + (item.Timing - elapsedMilliseconds) * ScrollSpeed / 1000 + MainWindow.NoteOffset;
				replayNote.FillColor = item.Key == 1 || item.Key == 2 ? Color.Red : Color.Blue;
				if (xPos < 2300 && xPos > HitPosition)
				{
					replayNote.Position = new SFML.System.Vector2f(xPos - NoteSize / 2, ReplayYStart - NoteSize / 2);
					Win.Draw(replayNote);
				}
			}
			foreach (var item in BeatmapFile)
			{

				actualNoteSize = item.Key >= 2 ? NoteSize : NoteSize / 2;
				float xPos = HitPosition + (item.Timing  - elapsedMilliseconds) * ScrollSpeed / 1000 + MainWindow.NoteOffset;
				beatmapNote.FillColor = item.Key == 0 || item.Key == 2 ? Color.Red : Color.Blue;
				beatmapNote.Radius = actualNoteSize;
				if (xPos < 2300 && xPos > HitPosition)
				{
					beatmapNote.Position = new SFML.System.Vector2f(xPos - actualNoteSize, BeatmapYStart - actualNoteSize);
					Win.Draw(beatmapNote);
				}
			}
		}
	}
}
