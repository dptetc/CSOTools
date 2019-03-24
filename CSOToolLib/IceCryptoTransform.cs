using System;
using System.Security.Cryptography;

namespace Nexon.CSO
{
	internal sealed class IceCryptoTransform : ICryptoTransform, IDisposable
	{
		internal IceCryptoTransform(int n, byte[] key, bool encrypt)
		{
			this.encrypt = encrypt;
			IceCryptoTransform.InitializeSBox();
			if (n == 0)
			{
				this.size = 1;
				this.rounds = 8;
			}
			else
			{
				this.size = n;
				this.rounds = n << 4;
			}
			this.keySchedule = new uint[this.rounds, 3];
			this.SetKey(key);
		}

		private static uint GFMultiply(uint a, uint b, uint m)
		{
			uint res = 0u;
			while (b != 0u)
			{
				if ((b & 1u) != 0u)
				{
					res ^= a;
				}
				a <<= 1;
				b >>= 1;
				if (a >= 256u)
				{
					a ^= m;
				}
			}
			return res;
		}

		private static uint GFExp7(uint b, uint m)
		{
			if (b == 0u)
			{
				return 0u;
			}
			uint x = IceCryptoTransform.GFMultiply(b, b, m);
			x = IceCryptoTransform.GFMultiply(b, x, m);
			x = IceCryptoTransform.GFMultiply(x, x, m);
			return IceCryptoTransform.GFMultiply(b, x, m);
		}

		private static uint Perm32(uint x)
		{
			uint res = 0u;
			int pbox = 0;
			while (x != 0u)
			{
				if ((x & 1u) != 0u)
				{
					res |= IceCryptoTransform.PBox[pbox];
				}
				pbox++;
				x >>= 1;
			}
			return res;
		}

		private static void InitializeSBox()
		{
			if (IceCryptoTransform.SBox == null)
			{
				IceCryptoTransform.SBox = new uint[4, 1024];
				for (int i = 0; i < 1024; i++)
				{
					int col = i >> 1 & 255;
					int row = (i & 1) | (i & 512) >> 8;
					IceCryptoTransform.SBox[0, i] = IceCryptoTransform.Perm32(IceCryptoTransform.GFExp7((uint)(col ^ IceCryptoTransform.SXor[0, row]), (uint)IceCryptoTransform.SMod[0, row]) << 24);
					IceCryptoTransform.SBox[1, i] = IceCryptoTransform.Perm32(IceCryptoTransform.GFExp7((uint)(col ^ IceCryptoTransform.SXor[1, row]), (uint)IceCryptoTransform.SMod[1, row]) << 16);
					IceCryptoTransform.SBox[2, i] = IceCryptoTransform.Perm32(IceCryptoTransform.GFExp7((uint)(col ^ IceCryptoTransform.SXor[2, row]), (uint)IceCryptoTransform.SMod[2, row]) << 8);
					IceCryptoTransform.SBox[3, i] = IceCryptoTransform.Perm32(IceCryptoTransform.GFExp7((uint)(col ^ IceCryptoTransform.SXor[3, row]), (uint)IceCryptoTransform.SMod[3, row]));
				}
			}
		}

		private void BuildSchedule(ushort[] keyBuilder, int n, int keyRotationOffset)
		{
			for (int i = 0; i < 8; i++)
			{
				int keyRotation = IceCryptoTransform.KeyRotation[keyRotationOffset + i];
				int subKeyIndex = n + i;
				this.keySchedule[subKeyIndex, 0] = 0u;
				this.keySchedule[subKeyIndex, 1] = 0u;
				this.keySchedule[subKeyIndex, 2] = 0u;
				for (int j = 0; j < 15; j++)
				{
					for (int k = 0; k < 4; k++)
					{
						ushort currentKeyBuilder = keyBuilder[keyRotation + k & 3];
						ushort bit = (ushort)(currentKeyBuilder & 1);
						this.keySchedule[subKeyIndex, j % 3] = (this.keySchedule[subKeyIndex, j % 3] << 1 | (uint)bit);
						keyBuilder[keyRotation + k & 3] = (ushort)(currentKeyBuilder >> 1 | (int)(bit ^ 1) << 15);
					}
				}
			}
		}

		private void SetKey(byte[] key)
		{
			ushort[] keyBuilder = new ushort[4];
			if (this.rounds == 8)
			{
				if (key.Length != 8)
				{
					throw new ArgumentException("Key size is not valid.", "key");
				}
				for (int i = 0; i < 4; i++)
				{
					keyBuilder[3 - i] = (ushort)((int)key[i << 1] << 8 | (int)key[(i << 1) + 1]);
				}
				this.BuildSchedule(keyBuilder, 0, 0);
				return;
			}
			else
			{
				if (key.Length != this.size << 3)
				{
					throw new ArgumentException("Key size is not valid.", "key");
				}
				for (int i = 0; i < this.size; i++)
				{
					int pos = i << 3;
					for (int j = 0; j < 4; j++)
					{
						keyBuilder[3 - j] = (ushort)((int)key[pos + (j << 1)] << 8 | (int)key[pos + (j << 1) + 1]);
					}
					this.BuildSchedule(keyBuilder, pos, 0);
					this.BuildSchedule(keyBuilder, this.rounds - 8 - pos, 8);
				}
				return;
			}
		}

		private uint Transform(uint value, int subKeyIndex)
		{
			uint tl = (value >> 16 & 1023u) | ((value >> 14 | value << 18) & 1047552u);
			uint tr = (value & 1023u) | (value << 2 & 1047552u);
			uint al = this.keySchedule[subKeyIndex, 2] & (tl ^ tr);
			uint ar = al ^ tr;
			al ^= tl;
			al ^= this.keySchedule[subKeyIndex, 0];
			ar ^= this.keySchedule[subKeyIndex, 1];
			return IceCryptoTransform.SBox[(int)((UIntPtr)0), (int)((UIntPtr)(al >> 10))] | IceCryptoTransform.SBox[(int)((UIntPtr)1), (int)((UIntPtr)(al & 1023u))] | IceCryptoTransform.SBox[(int)((UIntPtr)2), (int)((UIntPtr)(ar >> 10))] | IceCryptoTransform.SBox[(int)((UIntPtr)3), (int)((UIntPtr)(ar & 1023u))];
		}

		private void Encrypt(byte[] input, int inputOffset, byte[] output, int outputOffset)
		{
			uint i = (uint)((int)input[inputOffset] << 24 | (int)input[inputOffset + 1] << 16 | (int)input[inputOffset + 2] << 8 | (int)input[inputOffset + 3]);
			uint r = (uint)((int)input[inputOffset + 4] << 24 | (int)input[inputOffset + 5] << 16 | (int)input[inputOffset + 6] << 8 | (int)input[inputOffset + 7]);
			for (int j = 0; j < this.rounds; j += 2)
			{
				i ^= this.Transform(r, j);
				r ^= this.Transform(i, j + 1);
			}
			output[outputOffset] = (byte)(r >> 24 & 255u);
			output[outputOffset + 1] = (byte)(r >> 16 & 255u);
			output[outputOffset + 2] = (byte)(r >> 8 & 255u);
			output[outputOffset + 3] = (byte)(r & 255u);
			output[outputOffset + 4] = (byte)(i >> 24 & 255u);
			output[outputOffset + 5] = (byte)(i >> 16 & 255u);
			output[outputOffset + 6] = (byte)(i >> 8 & 255u);
			output[outputOffset + 7] = (byte)(i & 255u);
		}

		private void Decrypt(byte[] input, int inputOffset, byte[] output, int outputOffset)
		{
			uint i = (uint)((int)input[inputOffset] << 24 | (int)input[inputOffset + 1] << 16 | (int)input[inputOffset + 2] << 8 | (int)input[inputOffset + 3]);
			uint r = (uint)((int)input[inputOffset + 4] << 24 | (int)input[inputOffset + 5] << 16 | (int)input[inputOffset + 6] << 8 | (int)input[inputOffset + 7]);
			for (int j = this.rounds - 1; j > 0; j -= 2)
			{
				i ^= this.Transform(r, j);
				r ^= this.Transform(i, j - 1);
			}
			output[outputOffset] = (byte)(r >> 24 & 255u);
			output[outputOffset + 1] = (byte)(r >> 16 & 255u);
			output[outputOffset + 2] = (byte)(r >> 8 & 255u);
			output[outputOffset + 3] = (byte)(r & 255u);
			output[outputOffset + 4] = (byte)(i >> 24 & 255u);
			output[outputOffset + 5] = (byte)(i >> 16 & 255u);
			output[outputOffset + 6] = (byte)(i >> 8 & 255u);
			output[outputOffset + 7] = (byte)(i & 255u);
		}

		public bool CanReuseTransform
		{
			get
			{
				return false;
			}
		}

		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		public int InputBlockSize
		{
			get
			{
				return 8;
			}
		}

		public int OutputBlockSize
		{
			get
			{
				return 8;
			}
		}

		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if (inputOffset + inputCount > inputBuffer.Length)
			{
				throw new ArgumentOutOfRangeException("inputCount");
			}
			if (outputBuffer == null)
			{
				throw new ArgumentNullException("outputBuffer");
			}
			if (outputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("outputOffset");
			}
			if (outputOffset + inputCount > outputBuffer.Length)
			{
				throw new ArgumentOutOfRangeException("inputCount");
			}
			if (this.encrypt)
			{
				for (int i = 0; i < inputCount; i += 8)
				{
					this.Encrypt(inputBuffer, inputOffset, outputBuffer, outputOffset);
					inputOffset += 8;
					outputOffset += 8;
				}
			}
			else
			{
				for (int i = 0; i < inputCount; i += 8)
				{
					this.Decrypt(inputBuffer, inputOffset, outputBuffer, outputOffset);
					inputOffset += 8;
					outputOffset += 8;
				}
			}
			return inputCount;
		}

		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if (inputOffset + inputCount > inputBuffer.Length)
			{
				throw new ArgumentOutOfRangeException("inputCount");
			}
			byte[] outputBuffer = new byte[inputCount + 7 & -8];
			int outputOffset = 0;
			if (this.encrypt)
			{
				for (int i = 0; i < inputCount; i += 8)
				{
					this.Encrypt(inputBuffer, inputOffset, outputBuffer, outputOffset);
					inputOffset += 8;
					outputOffset += 8;
				}
			}
			else
			{
				for (int i = 0; i < inputCount; i += 8)
				{
					this.Decrypt(inputBuffer, inputOffset, outputBuffer, outputOffset);
					inputOffset += 8;
					outputOffset += 8;
				}
			}
			return outputBuffer;
		}

		public void Dispose()
		{
			this.size = 0;
			this.rounds = 0;
			for (int i = 0; i < this.keySchedule.GetLength(0); i++)
			{
				for (int j = 0; j < this.keySchedule.GetLength(1); j++)
				{
					this.keySchedule[i, j] = 0u;
				}
			}
			this.keySchedule = null;
		}

		private static uint[,] SBox;

		private static readonly int[,] SMod = new int[,]
		{
			{
				333,
				313,
				505,
				369
			},
			{
				379,
				375,
				319,
				391
			},
			{
				361,
				445,
				451,
				397
			},
			{
				397,
				425,
				395,
				505
			}
		};

		private static readonly int[,] SXor = new int[,]
		{
			{
				131,
				133,
				155,
				205
			},
			{
				204,
				167,
				173,
				65
			},
			{
				75,
				46,
				212,
				51
			},
			{
				234,
				203,
				46,
				4
			}
		};

		private static readonly uint[] PBox = new uint[]
		{
			1u,
			128u,
			1024u,
			8192u,
			524288u,
			2097152u,
			16777216u,
			1073741824u,
			8u,
			32u,
			256u,
			16384u,
			65536u,
			8388608u,
			67108864u,
			536870912u,
			4u,
			16u,
			512u,
			32768u,
			131072u,
			4194304u,
			134217728u,
			268435456u,
			2u,
			64u,
			2048u,
			4096u,
			262144u,
			1048576u,
			33554432u,
			2147483648u
		};

		private static readonly int[] KeyRotation = new int[]
		{
			0,
			1,
			2,
			3,
			2,
			1,
			3,
			0,
			1,
			3,
			2,
			0,
			3,
			1,
			0,
			2
		};

		private bool encrypt;

		private int size;

		private int rounds;

		private uint[,] keySchedule;
	}
}
