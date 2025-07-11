using System;
using System.IO;

namespace Chess{
public class MainClass
{
	public static void Main()
	{
		int qcapts=0,stmates=0,ckmates=0;
		int[,,,] KQK = new int[64,64,64,2];
		using(var stream = File.Open("KQK.bin", FileMode.Open))
		{
			using(var reader = new BinaryReader(stream))
			{
				for(int wk=0;wk<64;wk++)
				for(int bk=0;bk<64;bk++)
				for(int wq=0;wq<64;wq++)
				for(int btm=0;btm<2;btm++)
					KQK[wk,bk,wq,btm]=reader.ReadInt32();
			}
		}
		for(int wk=0;wk<64;wk++)
		for(int bk=0;bk<64;bk++)
		for(int wq=0;wq<64;wq++)
		if(KQK[wk,bk,wq,1]==-1000)
		{
			if(Chess.IsKMove(bk,wq) && !Chess.IsKMove(wk,wq))
			{ KQK[wk,bk,wq,1]=0; qcapts++; }
			else {
				bool hasMove=false;
				foreach(int sq in Chess.KMoves(bk))
					if(KQK[wk,sq,wq,0]==-1000)
					{ hasMove=true; break; }
				if(!hasMove)
				{
					if(KQK[wk,bk,wq,0]>-2000)
					{ KQK[wk,bk,wq,1]=0; stmates++; }
					else
					{ KQK[wk,bk,wq,1]=-500; ckmates++; }
				}
			}
		}

		Console.WriteLine("qcapts: " + qcapts.ToString() +
		", stmates: " + stmates.ToString() + ", ckmates: " +
		ckmates.ToString());

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
