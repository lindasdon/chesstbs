using System;
using System.IO;

namespace Chess{
public class MainClass
{
	public static void Main()
	{
		int wtmct=0, btmct=0;
		int[,,,] KQK = new int[64,64,64,2];
		for(int wk=0;wk<64;wk++)
		for(int bk=0;bk<64;bk++)
		for(int wq=0;wq<64;wq++)
		if(!Chess.IsKMove(wk,bk) && (wk!=bk) &&
		(wk!=wq) && (bk!=wq))
		{
			KQK[wk,bk,wq,1]=-1000;
			btmct++;
			if(!Chess.IsQMove(wq,bk,wk))
			{
				KQK[wk,bk,wq,0]=-1000;
				wtmct++;
			}
			else
				KQK[wk,bk,wq,0]=-2000;
		}
		else KQK[wk,bk,wq,0]=KQK[wk,bk,wq,1]=-2000;

		Console.WriteLine("wtm: " + wtmct.ToString() +
		", btm: " + btmct.ToString());

		using(var stream = File.Open("KQK.bin", FileMode.Create))
		{
			using(var writer = new BinaryWriter(stream))
			{
				for(int wk=0;wk<64;wk++)
				for(int bk=0;bk<64;bk++)
				for(int wq=0;wq<64;wq++)
				for(int btm=0;btm<2;btm++)
					writer.Write(KQK[wk,bk,wq,btm]);
			}
		}
		Console.WriteLine("KQK.bin saved");
	}
}
}
