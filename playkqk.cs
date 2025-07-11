using System;
using System.IO;

namespace Chess{
public class MainClass
{
	public static void Main()
	{
		int wins=0,losses=0;
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
		int curloss=-500;
		while(true)
		{
			bool hasnull=false;
			for(int wk=0;wk<64;wk++)
			{ for(int bk=0;bk<64;bk++)
			  { for(int wq=0;wq<64;wq++)
			    { for(int btm=0;btm<2;btm++)
				  { if(KQK[wk,bk,wq,btm]==-1000)
				    { hasnull=true; break; }
				  }
				  if(hasnull) break;
				}
				if(hasnull) break;
			  } 
			  if(hasnull) break;
			}
			if(!hasnull) break;
			wins=0;
			for(int wk=0; wk<64; wk++)
			for(int bk=0; bk<64; bk++)
			for(int wq=0; wq<64; wq++)
			if(KQK[wk,bk,wq,1]==curloss)
			{
				foreach(int sq in Chess.KMoves(wk))
				if(KQK[sq,bk,wq,0]==-1000)
				{ KQK[sq,bk,wq,0]=-curloss; wins++; }
				foreach(int sq in Chess.QMoves(wq,wk))
				if(KQK[wk,bk,sq,0]==-1000)
				{ KQK[wk,bk,sq,0]=-curloss; wins++; }
			}
			Console.WriteLine(wins.ToString() + " wins of " +
			(-curloss).ToString());

			losses=0;
			for(int wk=0;wk<64;wk++)
			for(int bk=0;bk<64;bk++)
			for(int wq=0;wq<64;wq++)
			if(KQK[wk,bk,wq,1]==-1000)
			{
				bool hasMove=false;
				foreach(int sq in Chess.KMoves(bk))
				if(KQK[wk,sq,wq,0]==-1000)
				{ hasMove=true; break; }
				if(!hasMove)
				{ KQK[wk,bk,wq,1]=curloss+1; losses++; }
			}
			Console.WriteLine(losses.ToString() + 
			" losses of " + (curloss+1).ToString());

			curloss++;
		}
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
	}
}
}
