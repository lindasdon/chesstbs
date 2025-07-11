using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Chess{
public class MainClass
{
	public static void Main()
	{
		int wtmct=0, btmct=0;
		using(var fs = File.Open("KRBKNN.bin", FileMode.Create))
		using(var writer = new BinaryWriter(fs))
		{
			for(int wk=0;wk<64;wk++)
			for(int bk=0;bk<64;bk++)
			for(int wr=0;wr<64;wr++)
			for(int wb=0;wb<64;wb++)
			for(int bn1=0;bn1<64;bn1++)
			for(int bn2=0;bn2<64;bn2++)
			if(Chess.distinct(wk,bk,wr,wb,bn1,bn2) && !Chess.IsKMove(wk,bk))
			{
				if(Chess.IsRMove(wr,bk,wk,wb,bn1,bn2) ||
				Chess.IsBMove(wb,bk,wk,wr,bn1,bn2))
					writer.Write((short)(-2000));
				else {
					writer.Write((short)(-1000));
					wtmct++;
				}
				if(Chess.IsNMove(bn1,wk) || Chess.IsNMove(bn2,wk))
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
