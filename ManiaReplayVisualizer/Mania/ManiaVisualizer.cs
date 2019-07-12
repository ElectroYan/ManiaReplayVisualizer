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

namespace OsuReplayVisualizer
{
	internal class ManiaVisualizer : ScrollVisualizer
	{
		int NoteMinHeight = 25;
		

		int HitPosition = 800;
		
		
		string AudioFile = "";
		

		public ManiaVisualizer(ManiaParser mania, string audioFile)
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

		private void Loop()
		{
			Stopwatch watch = new Stopwatch();
			bool musicPlaying = false;
			Music music;
			if (AudioFile.EndsWith(".mp3"))
			{
				new AudioConverter().ConvertMp3ToWav(AudioFile);
				music = new Music("Audio.wav");
			}
			else
				music = new Music(AudioFile);
			watch.Start();
			while (Win.IsOpen)
			{
				Win.DispatchEvents();
				if (!musicPlaying)
				{
					music.Play();
					musicPlaying = true;
				}


				DrawNotes(watch.ElapsedMilliseconds);

				DrawDefaultStuff();

				Win.Display();

				Thread.Sleep(1);

				Win.Clear();
			}
		}

		private void DrawDefaultStuff()
		{
			RectangleShape hitPositionBar = new RectangleShape(new Vector2f(WIN_WIDTH, 5));
			hitPositionBar.FillColor = Color.White;
			hitPositionBar.Position = new Vector2f(0, HitPosition);
			Win.Draw(hitPositionBar);
		}

		

		private void DrawNotes(long elapsedMilliseconds)
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