using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuReplayVisualizer
{
	public class HitObject
	{
		/// <summary>
		/// Taiko
		/// </summary>
		/// <param name="timing"></param>
		/// <param name="key"></param>
		public HitObject(int timing, int key)
		{
			Timing = timing;
			Key = key;
		}
		/// <summary>
		/// Mania
		/// </summary>
		/// <param name="timing"></param>
		/// <param name="key"></param>
		/// <param name="length"></param>
		public HitObject(int timing, int key, int length = 0)
		{
			Timing = timing;
			Key = key;
			Length = length;
		}
		public int Timing;
		public int Key;
		public int Length;
	}
}
