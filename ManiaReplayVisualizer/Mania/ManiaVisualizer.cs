using SFML.Graphics;
using SFML.Audio;
using SFML.Window;
using SFML.System;
using System;
using System.Threading;
using System.Diagnostics;
using System.IO;
using NAudio.Wave;
using System.Collections.Generic;
using System.Linq;

namespace OsuReplayVisualizer
{
	internal class ManiaVisualizer : ScrollVisualizer
	{
		int NoteMinHeight = 25;
		

		int HitPosition = 800;
		
		
		string AudioFile = "";
		

		public ManiaVisualizer(ManiaParser mania, string audioFile) : base (audioFile)
		{
			ReplayFile = mania.ReplayFile;
			BeatmapFile = mania.BeatmapFile;
			AudioFile = audioFile;
		}
		internal void Start()
		{
			NoteSize = 100;
			ScrollSpeed = 1000;
			
			Loop();
		}



		protected override void DrawDefaultStuff()
		{
			RectangleShape hitPositionBar = new RectangleShape(new Vector2f(WIN_WIDTH, 5));
			hitPositionBar.FillColor = Color.White;
			hitPositionBar.Position = new Vector2f(0, HitPosition);
			Win.Draw(hitPositionBar);
			int keyCount = BeatmapFile.Max(x => x.Key);
			RectangleShape columnLine = new RectangleShape(new Vector2f(2, WIN_HEIGHT));
			columnLine.FillColor = new Color(200,200,200);
			for (int i = 1; i <= keyCount; i++)
			{
				float xPos = Math.Min(ReplayXStart, BeatmapXStart) + i * (ColumnSpacing + NoteSize);
				columnLine.Position = new Vector2f(xPos, 0);
				Win.Draw(columnLine);
			}
		}

		

		protected override void DrawNotes(long elapsedMilliseconds)
		{
			RectangleShape replayNote = new RectangleShape(new Vector2f(NoteSize, NoteMinHeight));
			replayNote.FillColor = new Color(0, 255, 0, 100);
			RectangleShape beatmapNote = new RectangleShape(new Vector2f(NoteSize, NoteMinHeight));
			beatmapNote.FillColor = new Color(255, 0, 0, 100);

			foreach (var item in ReplayFile)
			{
				float yPos = HitPosition + (-item.Timing + elapsedMilliseconds) * ScrollSpeed / 1000 + MainWindow.NoteOffset;
				float yPosEnd = HitPosition + (-item.Timing + elapsedMilliseconds + item.Length) * ScrollSpeed / 1000 + MainWindow.NoteOffset;
				float noteHeight = Math.Max(NoteMinHeight, Math.Abs(yPosEnd-yPos));
				replayNote.Size = new Vector2f(NoteSize, noteHeight);
				if (yPos > -noteHeight && yPos < 900 + noteHeight)
				{
					replayNote.Position = new Vector2f(ReplayXStart + item.Key * (NoteSize + ColumnSpacing), yPos - noteHeight);
					Win.Draw(replayNote);
				}
			}
			foreach (var item in BeatmapFile)
			{
				float yPos = HitPosition + (-item.Timing + elapsedMilliseconds) * ScrollSpeed / 1000 + MainWindow.NoteOffset;
				float yPosEnd = HitPosition + (-item.Timing + elapsedMilliseconds + item.Length) * ScrollSpeed / 1000 + MainWindow.NoteOffset;
				float noteHeight = Math.Max(NoteMinHeight, Math.Abs(yPosEnd - yPos)); //item.length
				beatmapNote.Size = new Vector2f(NoteSize, noteHeight);
				if (yPos > -noteHeight && yPos < 900 + noteHeight)
				{
					beatmapNote.Position = new Vector2f(BeatmapXStart + item.Key * (NoteSize + ColumnSpacing), yPos - noteHeight);
					Win.Draw(beatmapNote);
				}
			}

		}

		

		
	}
}