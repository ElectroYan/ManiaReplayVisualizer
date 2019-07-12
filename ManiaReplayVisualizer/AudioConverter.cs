using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuReplayVisualizer
{
	class AudioConverter
	{
		public void ConvertMp3ToWav(string _inPath_)
		{
			using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
			{
				using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
				{
					WaveFileWriter.CreateWaveFile("Audio.wav", pcm);
				}
			}
		}
	}
}
