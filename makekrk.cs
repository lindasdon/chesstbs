using System;
using System.IO;

namespace Chess{
public class MainClass
{
	public static void Main()
	{
		int wtmct=0, btmct=0;
		using(var fs = new FileStream("KRK.bin",FileMode.Create))
		using(var writer = new BinaryWriter(fs))
		for(int wk=0;wk<64;wk++)
		for(int bk=0;bk<64;bk++)
		for(int wr=0;wr<64;wr++)
		if(!Chess.IsKMove(wk,bk) && (wk!=bk) &&
		(wk!=wr) && (bk!=wr))
		{
			if(!Chess.IsRMove(wr,bk,wk))
			{
				writer.Write((short)(-1000));
				wtmct++;
			}
			else
				writer.Write((short)(-2000));
			writer.Write((short)(-1000));
			btmct++;
		}
		else {
				writer.Write((short)(-2000));
				writer.Write((short)(-2000));
		}

		Console.WriteLine("wtm: " + wtmct.ToString() +
		", btm: " + btmct.ToString());
		Console.WriteLine("Saved file while generating");
	}
}
}
