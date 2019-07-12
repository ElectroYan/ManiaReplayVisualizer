using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuReplayVisualizer
{
	class ManiaColumns
	{
		public static List<int> GetColumnsByKeycount(int keyCount)
		{
			List<int> columns = new List<int>();
			if (keyCount == 4)
				columns = new List<int> { 64, 192, 320, 448 };
			else if (keyCount == 7)
				columns = new List<int> { 36, 109, 182, 256, 329, 402, 475 };
			return columns;
		}
	}

	
}
