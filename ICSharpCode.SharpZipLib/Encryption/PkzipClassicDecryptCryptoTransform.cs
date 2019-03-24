using System;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
	internal class PkzipClassicDecryptCryptoTransform : PkzipClassicCryptoBase, ICryptoTransform, IDisposable
	{
		internal PkzipClassicDecryptCryptoTransform(byte[] keyBlock)
		{
			base.SetKeys(keyBlock);
		}

		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			byte[] array = new byte[inputCount];
			this.TransformBlock(inputBuffer, inputOffset, inputCount, array, 0);
			return array;
		}

		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			for (int i = inputOffset; i < inputOffset + inputCount; i++)
			{
				byte b = (byte)(inputBuffer[i] ^ base.TransformByte());
				outputBuffer[outputOffset++] = b;
				base.UpdateKeys(b);
			}
			return inputCount;
		}

		public bool CanReuseTransform
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
				return 1;
			}
		}

		public int OutputBlockSize
		{
			get
			{
				return 1;
			}
		}

		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		public void Dispose()
		{
			base.Reset();
		}
	}
}
