using System;
using System.Security.Cryptography;

namespace Nexon.CSO
{
	public sealed class Ice : SymmetricAlgorithm
	{
		public Ice() : this(0)
		{
		}

		public Ice(int n)
		{
			if (n < 0)
			{
				throw new ArgumentOutOfRangeException("n");
			}
			this.n = n;
			this.ModeValue = CipherMode.ECB;
			this.PaddingValue = PaddingMode.None;
			this.BlockSizeValue = 64;
			this.LegalBlockSizesValue = new KeySizes[]
			{
				new KeySizes(this.BlockSizeValue, this.BlockSizeValue, 0)
			};
			this.KeySizeValue = Math.Max(n << 6, 64);
			this.LegalKeySizesValue = new KeySizes[]
			{
				new KeySizes(this.KeySizeValue, this.KeySizeValue, 0)
			};
		}

		public override CipherMode Mode
		{
			get
			{
				return base.Mode;
			}
			set
			{
				if (value != CipherMode.ECB)
				{
					throw new NotSupportedException("Only ECB is currently supported.");
				}
				base.Mode = value;
			}
		}

		public override PaddingMode Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				if (value != PaddingMode.None)
				{
					throw new NotSupportedException("No padding is currently supported.");
				}
				base.Padding = value;
			}
		}

		public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
		{
			if (rgbKey == null)
			{
				throw new ArgumentNullException("rgbKey");
			}
			if (rgbKey.Length != this.KeySizeValue >> 3)
			{
				throw new ArgumentException("Key size is not valid.", "rgbKey");
			}
			return new IceCryptoTransform(this.n, rgbKey, false);
		}

		public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
		{
			if (rgbKey == null)
			{
				throw new ArgumentNullException("rgbKey");
			}
			if (rgbKey.Length != this.KeySizeValue >> 3)
			{
				throw new ArgumentException("Key size is not valid.", "rgbKey");
			}
			return new IceCryptoTransform(this.n, rgbKey, true);
		}

		public override void GenerateIV()
		{
			RandomNumberGenerator rng = RandomNumberGenerator.Create();
			byte[] iv = new byte[8];
			rng.GetBytes(iv);
			this.IVValue = iv;
		}

		public override void GenerateKey()
		{
			RandomNumberGenerator rng = RandomNumberGenerator.Create();
			byte[] key = new byte[this.KeySizeValue >> 3];
			rng.GetBytes(key);
			this.KeyValue = key;
		}

		private int n;
	}
}
