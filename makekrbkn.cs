using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Chess{
public class MainClass
{
	public static void Main()
	{
		int wtmct=0, btmct=0;
		using(var fs = File.Open("KRBKN.bin", FileMode.Create))
		using(var writer = new BinaryWriter(fs))
		{
			for(int wk=0;wk<64;wk++)
			for(int bk=0;bk<64;bk++)
			for(int wr=0;wr<64;wr++)
			for(int wb=0;wb<64;wb++)
			for(int bn=0;bn<64;bn++)
			if(Chess.distinct(wk,bk,wr,wb,bn) && !Chess.IsKMove(wk,bk))
			{
				if(Chess.IsRMove(wr,bk,wk,wb,bn) ||
				Chess.IsBMove(wb,bk,wk,wr,bn))
					writer.Write((short)(-2000));
				else {
					writer.Write((short)(-1000));
					wtmct++;
				}
				if(Chess.IsNMove(bn,wk))
					writer.Write((short)(-2000));
				else {
					writer.Write((short)(-1000));
					btmct++;
				}
			} else {
				writer.Write((short)(-2000));
				writer.Write((short)(-2000));
			}			
		}
		Console.WriteLine("wtm: " + wtmct.ToString() + ", btm: " + btmct.ToString());
	}
}
}
