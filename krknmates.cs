using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Chess{
public class MainClass
{
	public static void Main()
	{
		int rcapts=0,bstmates=0,bckmates=0;
		var mmf = MemoryMappedFile.CreateFromFile("KRKN.bin",FileMode.Open,"KRKN");
		using(var accessor = mmf.CreateViewAccessor())
		{
			int wk,bk,wr,bn,btm,
				wmates=0, bmatesinone=0;
			int[,] wmts = new int[8,4]{{0,2,8,17},{0,16,1,10},{7,5,15,22},{7,23,6,13},
			{56,58,48,41},{56,40,57,50},{63,61,55,46},{63,47,62,53}};
			for(int row = 0; row < 8; row++)
			{
				wk=wmts[row,0];bk=wmts[row,1];wr=wmts[row,2];bn=wmts[row,3];
				long pos=filepos(wk,bk,wr,bn,0);
				accessor.Write(pos, (short)(-500)); wmates++;
				foreach(int sq in Chess.NMoves(bn))
				if((sq!=wk) && (sq!=bk))
				{
					long pos2=filepos(wk,bk,wr,sq,1);
					accessor.Write(pos2, (short)500);
					bmatesinone++;
				}
			}
			Console.WriteLine("white is mated {0} times",wmates);
			Console.WriteLine("black mates in one {0} times",bmatesinone);
			for(wk=0;wk<64;wk++)
			for(bk=0;bk<64;bk++)
			for(wr=0;wr<64;wr++)
			for(bn=0;bn<64;bn++)
			{
				long pos=filepos(wk,bk,wr,bn,1);
				short score=accessor.ReadInt16(pos);
				if(score==-1000)
				{
					bool hasMove=false;
					if((Chess.IsKMove(bk,wr) && !Chess.IsKMove(wk,wr)) ||
					Chess.IsNMove(bn,wr))
					{ accessor.Write(pos, (short)0); rcapts++; hasMove=true; }
					if(!hasMove)
					foreach(int sq in Chess.KMoves(bk))
					{
						long kmvpos = filepos(wk,sq,wr,bn,0);
						score=accessor.ReadInt16(kmvpos);
						if(score==-1000)
						{ hasMove=true; break; }
					}
					if(!hasMove)
					foreach(int sq in Chess.NMoves(bn))
					{
						long nmvpos = filepos(wk,bk,wr,sq,0);
						score=accessor.ReadInt16(nmvpos);
						if(score==-1000)
						{ hasMove=true; break; }
					}
					if(!hasMove)
					{
						score=accessor.ReadInt16(pos-2);
						if(score>-2000)
						{
							accessor.Write(pos, (short)0);
							bstmates++;
						}
						else
						{
							accessor.Write(pos,(short)(-500));
							bckmates++;
						}
					}
				}
			}
		}
		Console.WriteLine("rcapts: " + rcapts.ToString() +
		", b stmates: " + bstmates.ToString() + ", b mates: " +
		bckmates.ToString());
	}
	public static long filepos(int wk, int bk, int wr, int bn, int btm)
	{	return (wk*64*64*64*2 + bk*64*64*2 + wr*64*2 + bn*2 + btm)*sizeof(short); }
}
}
