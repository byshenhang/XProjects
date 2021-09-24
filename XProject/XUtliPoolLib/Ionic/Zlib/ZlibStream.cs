﻿using System;
using System.IO;

namespace Ionic.Zlib
{

	public class ZlibStream : Stream
	{

		public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				bool disposed = this._disposed;
				if (disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				bool disposed = this._disposed;
				if (disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				bool flag = this._baseStream._workingBuffer != null;
				if (flag)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				bool flag2 = value < 1024;
				if (flag2)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				bool flag = !this._disposed;
				if (flag)
				{
					bool flag2 = disposing && this._baseStream != null;
					if (flag2)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		public override bool CanRead
		{
			get
			{
				bool disposed = this._disposed;
				if (disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				bool disposed = this._disposed;
				if (disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		public override void Flush()
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Flush();
		}

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				bool flag = this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer;
				long result;
				if (flag)
				{
					result = this._baseStream._z.TotalBytesOut;
				}
				else
				{
					bool flag2 = this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader;
					if (flag2)
					{
						result = this._baseStream._z.TotalBytesIn;
					}
					else
					{
						result = 0L;
					}
				}
				return result;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			bool disposed = this._disposed;
			if (disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		internal ZlibBaseStream _baseStream;

		private bool _disposed;
	}
}
