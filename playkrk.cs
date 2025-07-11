using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Chess{
public class MainClass
{
	public static void Main()
	{
		int curloss=-500, wins,losses;
		using(var mmf = MemoryMappedFile.CreateFromFile("KRK.bin",FileMode.Open,"KRK"))
		using(var acsr = mmf.CreateViewAccessor())
		{
			while(true)
			{
				bool hasnull=false;
				for(long ckpos = 0; ckpos<64*64*64*2*2; ckpos+=2)
					if(acsr.ReadInt16(ckpos)==-1000)
					{ hasnull=true; break; }
				if(!hasnull) break;

				wins=0;
				for(int wk=0;wk<64;wk++)
				for(int bk=0;bk<64;bk++)
				for(int wr=0;wr<64;wr++)
				if(ReadKRK(acsr,wk,bk,wr,1)==curloss)
				{
					foreach(int sq in Chess.KMoves(wk))
						if(ReadKRK(acsr,sq,bk,wr,0)==-1000)
						{
							WriteKRK(acsr,sq,bk,wr,0,(short)(-curloss));
							wins++;
						}
					foreach(int sq in Chess.RMoves(wr,wk))
						if(ReadKRK(acsr,wk,bk,sq,0)==-1000)
						{
							WriteKRK(acsr,wk,bk,sq,0,(short)(-curloss));
							wins++;
						}
				}
				Console.WriteLine("{0} wins of {1}", wins, -curloss);
						
				losses=0;
				for(int wk=0;wk<64;wk++)
				for(int bk=0;bk<64;bk++)
				for(int wr=0;wr<64;wr++)
				if(ReadKRK(acsr,wk,bk,wr,1)==-1000)
				{
					bool hasMove=false;
					foreach(int sq in Chess.KMoves(bk))
					if(ReadKRK(acsr,wk,sq,wr,0)==-1000)
					{ hasMove=true; break; }
					if(!hasMove)
					{
						WriteKRK(acsr,wk,bk,wr,1,(short)(curloss+1));
						losses++;
					}
				}
				Console.WriteLine("{0} losses of {1}", losses, curloss+1);

				curloss++;
			}
		}
	}
	public static short ReadKRK(MemoryMappedViewAccessor acsr,
	int wk,int bk,int wr,int btm)
	{ return acsr.ReadInt16((wk*64*64*2 + bk*64*2 + wr*2 + btm)*2); }
	public static void WriteKRK(MemoryMappedViewAccessor acsr,
	int wk,int bk,int wr,int btm, short val)
	{ acsr.Write((wk*64*64*2 + bk*64*2 + wr*2 + btm)*2,val); }
}	
}
