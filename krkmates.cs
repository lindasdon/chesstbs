using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Chess{
public class MainClass
{
	public static void Main()
	{
		int rcapts=0,stmates=0,ckmates=0;
		var mmf = MemoryMappedFile.CreateFromFile("KRK.bin",FileMode.Open,"KRK");
		using(var accessor = mmf.CreateViewAccessor())
		{
			for(int wk=0;wk<64;wk++)
			for(int bk=0;bk<64;bk++)
			for(int wr=0;wr<64;wr++)
			{
				long pos=filepos(wk,bk,wr,1);
				short score=accessor.ReadInt16(pos);
				if(score==-1000)
				{
					if(Chess.IsKMove(bk,wr) && !Chess.IsKMove(wk,wr))
					{ accessor.Write(pos, (short)0); rcapts++; }
					else {
						bool hasMove=false;
						foreach(int sq in Chess.KMoves(bk))
						{
							long kmvpos = filepos(wk,sq,wr,0);
							score=accessor.ReadInt16(kmvpos);
							if(score==-1000)
							{ hasMove=true; break; }
						}
						if(!hasMove)
						{
							score=accessor.ReadInt16(pos-2);
							if(score>-2000)
							{
								accessor.Write(pos, (short)0);
								stmates++;
							}
							else
							{
								accessor.Write(pos,(short)(-500));
								ckmates++;
							}
						}
					}
				}
			}
		}
		Console.WriteLine("rcapts: " + rcapts.ToString() +
		", stmates: " + stmates.ToString() + ", ckmates: " +
		ckmates.ToString());
	}
	public static long filepos(int wk, int bk, int wr, int btm)
	{	return (wk*64*64*2 + bk*64*2 + wr*2 + btm)*sizeof(short); }
}
}
