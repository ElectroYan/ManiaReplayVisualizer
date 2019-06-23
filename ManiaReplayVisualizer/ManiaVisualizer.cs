using SFML.Graphics;
using SFML.Audio;
using SFML.Window;
using SFML.System;
using System;
using System.Threading;
using System.Diagnostics;
using System.IO;
using NAudio.Wave;

namespace ManiaReplayVisualizer
{
	internal class ManiaVisualizer
	{
		public static readonly int WIN_HEIGHT = 900;
		public static readonly int WIN_WIDTH = 1500;
		static int NoteWidth = 100;
		static int NoteMinHeight = 25;
		public static int ScrollSpeed = 1000;
		static int ReplayXStart = 200;
		static int BeatmapXStart = 150;
		static int HitPosition = 800;
		static int ColumnSpacing = Math.Abs(ReplayXStart - BeatmapXStart);

		public static RenderWindow Win = null;
		internal static void Start(string audioFile)
		{
			InitializeSfmlWindow();
			Loop(audioFile);
		}

		private static void Loop(string audioFile)
		{
			Stopwatch watch = new Stopwatch();
			bool musicPlaying = false;
			Music music;
			if (audioFile.EndsWith(".mp3"))
			{
				ConvertMp3ToWav(audioFile);
				music = new Music("Audio.wav");
			}
			else
				music = new Music(audioFile);
			watch.Start();
			while (Win.IsOpen)
			{
				Win.DispatchEvents();
				if (watch.ElapsedMilliseconds >= HitPosition && !musicPlaying)
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

		private static void DrawDefaultStuff()
		{
			RectangleShape hitPositionBar = new RectangleShape(new Vector2f(WIN_WIDTH, 5));
			hitPositionBar.FillColor = Color.White;
			hitPositionBar.Position = new Vector2f(0, HitPosition);
			Win.Draw(hitPositionBar);
		}

		private static void ConvertMp3ToWav(string _inPath_)
		{
			using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
			{
				using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
				{
					WaveFileWriter.CreateWaveFile("Audio.wav", pcm);
				}
			}
		}

		private static void DrawNotes(long elapsedMilliseconds)
		{
			RectangleShape replayNote = new RectangleShape(new Vector2f(NoteWidth, NoteMinHeight));
			replayNote.FillColor = new Color(0, 255, 0, 100);
			RectangleShape beatmapNote = new RectangleShape(new Vector2f(NoteWidth, NoteMinHeight));
			beatmapNote.FillColor = new Color(255, 0, 0, 100);

			foreach (var item in ManiaParser.ReplayFile)
			{
				float yPos = (((int)elapsedMilliseconds - HitPosition - item.Timing) * ScrollSpeed / 1000f) + HitPosition + MainWindow.NoteOffset;
				float yPosEnd = (((int)elapsedMilliseconds - HitPosition - item.Timing + item.Length) * ScrollSpeed / 1000f) + HitPosition + MainWindow.NoteOffset;
				float noteHeight = Math.Max(NoteMinHeight, yPosEnd-yPos);
				replayNote.Size = new Vector2f(NoteWidth, noteHeight);
				if (yPos > -noteHeight && yPos < 900 + noteHeight)
				{
					replayNote.Position = new Vector2f(ReplayXStart + item.Key * (NoteWidth + ColumnSpacing), yPos);
					Win.Draw(replayNote);
				}
			}
			foreach (var item in ManiaParser.BeatmapFile)
			{
				float yPos = (((int)elapsedMilliseconds - HitPosition - item.Timing) * ScrollSpeed / 1000f) + HitPosition + MainWindow.NoteOffset;
				float yPosEnd = (((int)elapsedMilliseconds - HitPosition - item.Timing + item.Length) * ScrollSpeed / 1000f) + HitPosition + MainWindow.NoteOffset;
				float noteHeight = Math.Max(NoteMinHeight, yPosEnd - yPos); //item.length
				beatmapNote.Size = new Vector2f(NoteWidth, noteHeight);
				if (yPos > -noteHeight && yPos < 900 + noteHeight)
				{
					beatmapNote.Position = new Vector2f(BeatmapXStart + item.Key * (NoteWidth + ColumnSpacing), yPos);
					Win.Draw(beatmapNote);
				}
			}

		}

		private static void InitializeSfmlWindow()
		{
			VideoMode videoMode = new VideoMode((uint)WIN_WIDTH, (uint)WIN_HEIGHT, 24);
			Win = new RenderWindow(videoMode, "Replay Visualizer", Styles.Default);
			Win.Closed += Win_Closed;
		}

		private static void Win_Closed(object sender, EventArgs e)
		{
			Win.Close();
		}
	}
}