using CoalMemory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game_Hacking
{
	class Memory
	{
		private Process _csgoProcess;
		private string _processName = "csgo";
		private IntPtr _clientLibAddress = (IntPtr)0x0000;
		private int _entityListOffset = 0x4D3B69C;
		private int _nextPlayerOffset = 0x10;
		private int _playerHealthOffsetFromEntity = 0x0100;
		private int _isCanJumpOffsetFromEntity = 0x0104;
		private int _forceJumpOffset = 0x51DEE88;
		private int _flashDurationOffset = 0xA410;

		[DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern int GetAsyncKeyState(int vKey);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ReadProcessMemory(IntPtr pHandle, IntPtr Address, byte[] Buffer, int Size, IntPtr NumberOfBytesRead);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WriteProcessMemory(IntPtr pHandle, IntPtr Address, byte[] Buffer, int Size, out int NumberOfBytesWritten);

		public void Main()
		{
			CMemory cMemory = new CMemory(Process.GetProcessesByName("csgo").FirstOrDefault());
			_clientLibAddress = cMemory.GetLibraryAddress("client_panorama.dll");
			if (_clientLibAddress == IntPtr.Zero)
				return;

			var localPlayerEntityAddress = cMemory.ReadInt(_clientLibAddress + _entityListOffset);

			var forceJump = _clientLibAddress + _forceJumpOffset;

			while (true)
			{
				var flashDuration = cMemory.ReadInt((IntPtr)localPlayerEntityAddress + _flashDurationOffset);

				if (flashDuration > 0)
					cMemory.WriteMemory((IntPtr)localPlayerEntityAddress + _flashDurationOffset, 0);

				while (GetAsyncKeyState(32) > 0)
				{
					var isCanJump = cMemory.ReadBool((IntPtr)localPlayerEntityAddress + 0x104);

					if(isCanJump)
					{
						cMemory.WriteMemory(forceJump, 5);
						Thread.Sleep(10);
						cMemory.WriteMemory(forceJump, 4);
						Console.WriteLine("Jumping", Console.ForegroundColor = ConsoleColor.Green);
					}
				}

				Console.WriteLine("Standing", Console.ForegroundColor = ConsoleColor.Yellow);
				Thread.Sleep(10);
			}
		}
	}
}
