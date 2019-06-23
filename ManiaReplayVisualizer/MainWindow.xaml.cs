
using System;
using System.Threading;
using System.Windows;
using Window = System.Windows.Window;

namespace ManiaReplayVisualizer
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
			//ReplayFilePath.Text = @"Y:\_Games\Rhythm Games\Osu\Replays\ElectroYan - Various Artists - 4K Luminal v2 [δ-★] (2019-06-01) OsuMania.osr";
			//BeatmapFilePath.Text = @"Y:\_Games\Rhythm Games\Osu\Songs\Various Artists - 4K Luminal v2\Various Artists - 4K Luminal v2 (Shoegazer) [â┬-üÜ].osu";
		}

		private void StartReplay_Click(object sender, RoutedEventArgs e)
		{
			int mode = 1;//(int)Math.Round(ModeSlider.Value);
			string audioFile = "";
			if (mode == 1)
			{
				ManiaParser.ReadReplay(ReplayFilePath.Text.Trim('"'));
				try
				{
				}
				catch
				{
					MessageBox.Show("Invalid Replay Filepath");
					return;
				}
				try
				{
					audioFile = ManiaParser.ReadBeatmap(BeatmapFilePath.Text.Trim('"'));
				}
				catch
				{
					MessageBox.Show("Invalid Beatmap Filepath");
					return;
				}
				new Thread(() => ManiaVisualizer.Start(audioFile)).Start();
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
			ManiaVisualizer.ScrollSpeed = (int)ScrollSpeedSlider.Value;
			ScrollSpeedValue.Text = ManiaVisualizer.ScrollSpeed.ToString();
		}
	}
	
}
