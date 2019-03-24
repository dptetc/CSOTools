using System;
using System.IO;
using System.Security.Cryptography;

namespace Nexon.CSO
{
	public static class ModelHelper
	{
		private static void TransformChunk(ICryptoTransform transform, byte[] data, int offset, int length)
		{
			if (length == 0)
			{
				return;
			}
			while (length > 0)
			{
				int tempLength = length;
				if (tempLength > 1024)
				{
					tempLength = 1024;
				}
				if ((tempLength & 7) != 0 || tempLength == 0)
				{
					return;
				}
				int decryptedLength = transform.TransformBlock(data, offset, tempLength, data, offset);
				length -= decryptedLength;
				offset += decryptedLength;
			}
		}

		public static ModelResult DecryptModel(string path)
		{
			ModelResult result;
			using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read, 4096, FileOptions.RandomAccess))
			{
				result = ModelHelper.DecryptModel(fs);
			}
			return result;
		}

		public static ModelResult DecryptModel(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanRead)
			{
				throw new ArgumentException("Cannot read from stream.", "stream");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException("Cannot write to stream.", "stream");
			}
			if (!stream.CanSeek)
			{
				throw new ArgumentException("Cannot seek in stream.", "stream");
			}
			stream.Seek(0L, SeekOrigin.Begin);
			if (stream.Length > 2147483647L || stream.Length < 244L)
			{
				return ModelResult.InvalidModel;
			}
			BinaryReader reader = new BinaryReader(stream);
			BinaryWriter writer = new BinaryWriter(stream);
			int version;
			try
			{
				if (reader.ReadInt32() != 1414743113)
				{
					return ModelResult.InvalidModel;
				}
				version = reader.ReadInt32();
				stream.Seek(72L, SeekOrigin.Begin);
				if ((long)reader.ReadInt32() < stream.Length)
				{
					return ModelResult.InvalidModel;
				}
			}
			catch (EndOfStreamException)
			{
				return ModelResult.InvalidModel;
			}
			SymmetricAlgorithm algorithm;
			switch (version)
			{
			case 20:
				algorithm = new Ice(4);
				algorithm.Key = ModelHelper.Version20Key;
				break;
			case 21:
				algorithm = new Ice(4);
				algorithm.Key = ModelHelper.Version21Key;
				break;
			default:
				return ModelResult.NotEncrypted;
			}
			using (algorithm)
			{
				using (ICryptoTransform transform = algorithm.CreateDecryptor())
				{
					stream.Seek(4L, SeekOrigin.Begin);
					writer.Write(9);
					try
					{
						stream.Seek(180L, SeekOrigin.Begin);
						int numTextures = reader.ReadInt32();
						int textureIndex = reader.ReadInt32();
						int i = 0;
						while (i < numTextures && textureIndex >= 0)
						{
							try
							{
								stream.Seek((long)(textureIndex + 68), SeekOrigin.Begin);
								int width = reader.ReadInt32();
								int height = reader.ReadInt32();
								int index = reader.ReadInt32();
								if (width >= 0 && height >= 0 && index >= 0)
								{
									stream.Seek((long)index, SeekOrigin.Begin);
									byte[] textureData = reader.ReadBytes(width * height + 768);
									ModelHelper.TransformChunk(transform, textureData, 0, textureData.Length);
									stream.Seek((long)index, SeekOrigin.Begin);
									writer.Write(textureData, 0, textureData.Length);
								}
							}
							catch (EndOfStreamException)
							{
							}
							i++;
							textureIndex += 80;
						}
					}
					catch (EndOfStreamException)
					{
					}
					try
					{
						stream.Seek(204L, SeekOrigin.Begin);
						int numBodyParts = reader.ReadInt32();
						int bodyPartIndex = reader.ReadInt32();
						int j = 0;
						while (j < numBodyParts && bodyPartIndex >= 0)
						{
							try
							{
								stream.Seek((long)(bodyPartIndex + 64), SeekOrigin.Begin);
								int numModels = reader.ReadInt32();
								stream.Seek(4L, SeekOrigin.Current);
								int modelIndex = reader.ReadInt32();
								if (modelIndex >= 0)
								{
									int k = 0;
									while (k < numModels && modelIndex >= 0)
									{
										try
										{
											stream.Seek((long)(modelIndex + 80), SeekOrigin.Begin);
											int numVerts = reader.ReadInt32();
											stream.Seek(4L, SeekOrigin.Current);
											int vertsIndex = reader.ReadInt32();
											if (vertsIndex >= 0)
											{
												stream.Seek((long)vertsIndex, SeekOrigin.Begin);
												byte[] vertexData = reader.ReadBytes(numVerts * 12);
												ModelHelper.TransformChunk(transform, vertexData, 0, vertexData.Length);
												stream.Seek((long)vertsIndex, SeekOrigin.Begin);
												writer.Write(vertexData, 0, vertexData.Length);
											}
										}
										catch (EndOfStreamException)
										{
										}
										k++;
										modelIndex += 112;
									}
								}
							}
							catch (EndOfStreamException)
							{
							}
							j++;
							bodyPartIndex += 76;
						}
					}
					catch (EndOfStreamException)
					{
					}
				}
			}
			return ModelResult.Success;
		}

		private static readonly byte[] Version20Key = new byte[]
		{
			50,
			166,
			33,
			224,
			171,
			107,
			244,
			44,
			147,
			198,
			241,
			150,
			251,
			56,
			117,
			104,
			186,
			112,
			19,
			134,
			224,
			179,
			113,
			244,
			227,
			155,
			7,
			34,
			12,
			254,
			136,
			58
		};

		private static readonly byte[] Version21Key = new byte[]
		{
			34,
			122,
			25,
			111,
			123,
			134,
			125,
			224,
			140,
			198,
			241,
			150,
			251,
			56,
			117,
			104,
			136,
			122,
			120,
			134,
			120,
			134,
			103,
			112,
			217,
			145,
			7,
			58,
			20,
			116,
			254,
			34
		};
	}
}
