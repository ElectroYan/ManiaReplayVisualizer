
using osu_database_reader.BinaryFiles;
using osu_database_reader.Components.Player;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using Window = System.Windows.Window;

namespace OsuReplayVisualizer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		
		public static int NoteOffset = 0;
		
		public MainWindow()
		{
			InitializeComponent();

			//ScoresDb scores = ScoresDb.Read(@"Y:\_Games\Rhythm Games\Osu\scores.db");
			//List<Replay> allReplays = new List<Replay>();
			//scores.Beatmaps.Select(x => x.Value).ToList().ForEach(x=> allReplays.AddRange(x));

			//Scores.ItemsSource = allReplays;
			ReplayFilePath.Text = @"Y:\_Games\Rhythm Games\Osu\Replays\ElectroYan - Various Artists - 4K Luminal v2 [δ-★] (2019-06-01) OsuMania.osr";
			BeatmapFilePath.Text = @"Y:\_Games\Rhythm Games\Osu\Songs\Various Artists - 4K Luminal v2\Various Artists - 4K Luminal v2 (Shoegazer) [â┬-üÜ].osu";
			ThisWin = this;
		}
		MainWindow ThisWin = null;
		ScrollVisualizer visualizer = null;

		private void StartReplay_Click(object sender, RoutedEventArgs e)
		{
			string path = BeatmapFilePath.Text;
			try
			{
				List<string> beatmapFile = File.ReadAllLines(path).ToList();
				int keyCount = int.Parse(beatmapFile.FirstOrDefault(x => x.StartsWith("CircleSize:")).Split(':')[1].Trim());
				string audioFile = path.Substring(0, path.LastIndexOf('\\') + 1) + beatmapFile.FirstOrDefault(x => x.StartsWith("AudioFilename:")).Split(':')[1].Trim();
				int mode = int.Parse(beatmapFile.FirstOrDefault(x => x.StartsWith("Mode:")).Split(':')[1].Trim());
				if (mode == 3)
				{
					ManiaParser mania = new ManiaParser();
					try
					{
						mania.ReadReplay(ReplayFilePath.Text.Trim('"'));
					}
					catch
					{
						MessageBox.Show("Invalid Replay Filepath");
						return;
					}
					try
					{
						mania.ReadBeatmap(beatmapFile, keyCount);
					}
					catch
					{
						MessageBox.Show("Invalid Beatmap Filepath");
						return;
					}
					new Thread(() =>
					{
						ManiaVisualizer maniaVis = new ManiaVisualizer(mania, audioFile);
						ThisWin.Dispatcher.Invoke(()=>visualizer = maniaVis);
						maniaVis.Start();
					}).Start();
				}

				if (mode == 1)
				{
					TaikoParser taiko = new TaikoParser();
					try
					{
						taiko.ReadReplay(ReplayFilePath.Text.Trim('"'));
					}
					catch
					{
						MessageBox.Show("Invalid Replay Filepath");
						return;
					}
					try
					{
						taiko.ReadBeatmap(beatmapFile, keyCount);
					}
					catch
					{
						MessageBox.Show("Invalid Beatmap Filepath");
						return;
					}
					new Thread(() =>
					{
						TaikoVisualizer taikoVis = new TaikoVisualizer(taiko, audioFile);
						ThisWin.Dispatcher.Invoke(() => visualizer = taikoVis);
						taikoVis.Start();
					}).Start();
				}

			}
			catch (Exception)
			{
				MessageBox.Show("Invalid Beatmap Filepath");

			}
		}

		private void OffsetSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			OffsetValue.Text = ((int)OffsetSlider.Value).ToString();
			NoteOffset = (int)OffsetSlider.Value;
		}

		private void BeatmapFilePath_Drop(object sender, DragEventArgs e)
		{
			BeatmapFilePath.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
		}

		private void ReplayFilePath_Drop(object sender, DragEventArgs e)
		{
			ReplayFilePath.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
		}

		private void BeatmapFilePath_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = true;
		}

		private void ReplayFilePath_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = true;
		}

		private void ScrollSpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			try
			{
				visualizer.ScrollSpeed = (int)ScrollSpeedSlider.Value;
				ScrollSpeedValue.Text = visualizer.ScrollSpeed.ToString();

			}
			catch 
			{
			}
		}

		private void Scores_Drop(object sender, DragEventArgs e)
		{

		}
	}
	
}
