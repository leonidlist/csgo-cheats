using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Hacking
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.CursorVisible = false;
			Memory mem = new Memory();
			mem.Main();
			Console.ReadKey();
		}
	}
}
