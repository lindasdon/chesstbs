using System;
using System.Collections.Generic;

namespace Chess{
public class Chess{
/*	public static void Main()
	{
	}
*/	
	public static bool distinct(params int[] pces)
	{
		for(int i=0; i<pces.Length-1;i++)
		for(int j=i+1; j<pces.Length;j++)
		if(pces[i]==pces[j]) return false;
		return true;
	}
	public static bool IsKMove(int a, int b)
	{
		return (a!=b) && (Math.Abs(a/8 - b/8)<2) &&
		(Math.Abs(a%8 - b%8)<2);
	}
	public static bool IsNMove(int a, int b)
	{
		int x=Math.Abs(a/8-b/8);
		int y=Math.Abs(a%8-b%8);
		if(x==1) return (y==2);
		if(x==2) return (y==1);
		return false;
	}
	public static bool IsRMove(
	int a, int b, params int[] blks)
	{
		if(a/8==b/8)
		{
			bool blocked=false;
			foreach(int blk in blks)
				if((blk/8==b/8)&&
				((blk>a)^(blk>b)))
					blocked=true;
			if(!blocked)
				return true;
		}
		if(a%8==b%8)
		{
			bool blocked=false;
			foreach(int blk in blks)
				if((blk%8==b%8)&&
				((blk>a)^(blk>b)))
					blocked=true;
			if(!blocked)
				return true;
		}
		return false;
	}
	public static bool IsBMove(
	int a, int b, params int[] blks)
	{
		bool blocked=false;
		if(Math.Abs(a/8-b/8)==
		Math.Abs(a%8-b%8))
		{
			foreach(int blk in blks)
				if((Math.Abs(blk/8-b/8)==
				Math.Abs(blk%8-b%8)) &&
				((blk>b)==(a>b)) &&
				((blk%8>b%8)==(a%8>b%8)) &&
				((blk>a)^(blk>b)))
					blocked=true;
			if(!blocked)
				return true;
		}
		return false;
	}
	public static bool IsQMove(
	int a, int b, params int[] blks)
	{
		return IsRMove(a, b, blks) ||
		IsBMove(a, b, blks);
	}
	public static IEnumerable<int> KMoves(int k)
	{
		int r=k/8, f=k%8;
		for(int rk = ((r==0) ? 0 : r-1);
		rk <= ((r==7) ? 7 : r+1); rk++)
		for(int fi = ((f==0) ? 0 : f-1);
		fi <= ((f==7) ? 7 : f+1); fi++)
		if(!((rk==r)&&(fi==f)))
			yield return rk*8 + fi;
	}
	public static IEnumerable<int> 
	RMoves(int rook, params int[] blks)
	{
		for(int dif=1; dif>=-1; dif-=2)
		{
			int sq=rook;
			while((sq+dif>=0) &&
			((sq+dif)/8==rook/8))
			{ sq+=dif;
			  bool broken=false;
			  foreach(int blk in blks)
			  if(blk==sq) 
			  { broken=true; break; }
			  if(broken) break;
			  yield return sq;
			}
		}
		for(int dif=8; dif>=-8; dif-=16)
		{
			int sq=rook;
			while((sq+dif>=0) && 
			(sq+dif<64))
			{ sq+=dif;
			  bool broken=false;
			  foreach(int blk in blks)
			  if(blk==sq)
			  { broken=true; break; }
			  if(broken) break;
			  yield return sq;
			}
		}
	}
	public static IEnumerable<int> 
	BMoves(int b, params int[] blks)
	{
		for(int rd=8;rd>=-8;rd-=16)
		for(int fd=1;fd>=-1;fd-=2)
		{
			int sq=b;
			while((sq+rd>=0)&&(sq+rd<64)&&
			(sq+fd>=0)&&((sq+fd)/8==sq/8))
			{
				sq+=rd+fd;
				bool blocked=false;
				foreach(int blk in blks)
				if(blk==sq)
				{ blocked=true; break; }
				if(blocked) break;
				yield return sq;
			}
		}
	}
	public static IEnumerable<int> 
	QMoves(int q, params int[] blks)
	{
		foreach(int i in RMoves(q, blks))
			yield return i;
		foreach(int i in BMoves(q, blks))
			yield return i;
	}
	public static IEnumerable<int> NMoves(int n)
	{
		int[,] arr={{1,2},{1,-2},{-1,2},
		{-1,-2},{2,1},{2,-1},{-2,1},{-2,-1}};
		for(int x=0;x<8;x++)
			if((n+(arr[x,0]*8) < 64)&&
			(n+(arr[x,0]*8) >= 0)&&
			(n+arr[x,1]>=0)&&
			((n+arr[x,1])/8==n/8))
				yield return n + arr[x,0]*8 +
				arr[x,1];
	}
}
}
