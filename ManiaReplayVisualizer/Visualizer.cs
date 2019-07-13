using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OsuReplayVisualizer
{
	abstract class Visualizer
	{
		public List<HitObject> ReplayFile;
		public List<HitObject> BeatmapFile;
		public RenderWindow Win = null;
		public readonly int WIN_HEIGHT = 900;
		public readonly int WIN_WIDTH = 1500;
		private bool Paused = false;
		public int NoteSize { get; set; }
		private string AudioFile;

		public Visualizer(string audioFile)
		{
			AudioFile = audioFile;
			InitializeSfmlWindow();
		}
		private void InitializeSfmlWindow()
		{
			VideoMode videoMode = new VideoMode((uint)WIN_WIDTH, (uint)WIN_HEIGHT, 24);
			Win = new RenderWindow(videoMode, "Replay Visualizer", Styles.Default);
			Win.KeyPressed += Win_KeyPressed;
			Win.Closed += Win_Closed;
		}

		protected void Loop()
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
			while (Win.IsOpen)
			{
				Win.DispatchEvents();
				if (!musicPlaying && !Paused)
				{
					music.Play();
					musicPlaying = true;
					watch.Start();
				}
				if (Paused)
				{
					watch.Stop();
					musicPlaying = false;
					music.Pause();
				}
				

				DrawNotes(watch.ElapsedMilliseconds);

				DrawDefaultStuff();

				Win.Display();

				Thread.Sleep(1);

				Win.Clear();
			}
		}

		protected abstract void DrawNotes(long mill);
		protected abstract void DrawDefaultStuff();



		private void Win_KeyPressed(object sender, KeyEventArgs e)
		{
			if (e.Code == Keyboard.Key.P || e.Code == Keyboard.Key.Escape)
			{
				Paused = !Paused;
			}
		}

		private void Win_Closed(object sender, EventArgs e)
		{
			Win.Close();
		}
	}
}
