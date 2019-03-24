using System;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
	internal class InflaterDynHeader
	{
		public bool Decode(StreamManipulator input)
		{
			for (;;)
			{
				switch (this.mode)
				{
				case 0:
					this.lnum = input.PeekBits(5);
					if (this.lnum < 0)
					{
						return false;
					}
					this.lnum += 257;
					input.DropBits(5);
					this.mode = 1;
					goto IL_61;
				case 1:
					goto IL_61;
				case 2:
					goto IL_B9;
				case 3:
					break;
				case 4:
					goto IL_1A8;
				case 5:
					goto IL_1EE;
				default:
					continue;
				}
				IL_13B:
				while (this.ptr < this.blnum)
				{
					int num = input.PeekBits(3);
					if (num < 0)
					{
						return false;
					}
					input.DropBits(3);
					this.blLens[InflaterDynHeader.BL_ORDER[this.ptr]] = (byte)num;
					this.ptr++;
				}
				this.blTree = new InflaterHuffmanTree(this.blLens);
				this.blLens = null;
				this.ptr = 0;
				this.mode = 4;
				IL_1A8:
				int symbol;
				while (((symbol = this.blTree.GetSymbol(input)) & -16) == 0)
				{
					this.litdistLens[this.ptr++] = (this.lastLen = (byte)symbol);
					if (this.ptr == this.num)
					{
						return true;
					}
				}
				if (symbol < 0)
				{
					return false;
				}
				if (symbol >= 17)
				{
					this.lastLen = 0;
				}
				else if (this.ptr == 0)
				{
					goto Block_10;
				}
				this.repSymbol = symbol - 16;
				this.mode = 5;
				IL_1EE:
				int bitCount = InflaterDynHeader.repBits[this.repSymbol];
				int num2 = input.PeekBits(bitCount);
				if (num2 < 0)
				{
					return false;
				}
				input.DropBits(bitCount);
				num2 += InflaterDynHeader.repMin[this.repSymbol];
				if (this.ptr + num2 > this.num)
				{
					goto Block_12;
				}
				while (num2-- > 0)
				{
					this.litdistLens[this.ptr++] = this.lastLen;
				}
				if (this.ptr == this.num)
				{
					return true;
				}
				this.mode = 4;
				continue;
				IL_B9:
				this.blnum = input.PeekBits(4);
				if (this.blnum < 0)
				{
					return false;
				}
				this.blnum += 4;
				input.DropBits(4);
				this.blLens = new byte[19];
				this.ptr = 0;
				this.mode = 3;
				goto IL_13B;
				IL_61:
				this.dnum = input.PeekBits(5);
				if (this.dnum < 0)
				{
					return false;
				}
				this.dnum++;
				input.DropBits(5);
				this.num = this.lnum + this.dnum;
				this.litdistLens = new byte[this.num];
				this.mode = 2;
				goto IL_B9;
			}
			return false;
			Block_10:
			throw new SharpZipBaseException();
			Block_12:
			throw new SharpZipBaseException();
		}

		public InflaterHuffmanTree BuildLitLenTree()
		{
			byte[] array = new byte[this.lnum];
			Array.Copy(this.litdistLens, 0, array, 0, this.lnum);
			return new InflaterHuffmanTree(array);
		}

		public InflaterHuffmanTree BuildDistTree()
		{
			byte[] array = new byte[this.dnum];
			Array.Copy(this.litdistLens, this.lnum, array, 0, this.dnum);
			return new InflaterHuffmanTree(array);
		}

		private const int LNUM = 0;

		private const int DNUM = 1;

		private const int BLNUM = 2;

		private const int BLLENS = 3;

		private const int LENS = 4;

		private const int REPS = 5;

		private static readonly int[] repMin = new int[]
		{
			3,
			3,
			11
		};

		private static readonly int[] repBits = new int[]
		{
			2,
			3,
			7
		};

		private static readonly int[] BL_ORDER = new int[]
		{
			16,
			17,
			18,
			0,
			8,
			7,
			9,
			6,
			10,
			5,
			11,
			4,
			12,
			3,
			13,
			2,
			14,
			1,
			15
		};

		private byte[] blLens;

		private byte[] litdistLens;

		private InflaterHuffmanTree blTree;

		private int mode;

		private int lnum;

		private int dnum;

		private int blnum;

		private int num;

		private int repSymbol;

		private byte lastLen;

		private int ptr;
	}
}
