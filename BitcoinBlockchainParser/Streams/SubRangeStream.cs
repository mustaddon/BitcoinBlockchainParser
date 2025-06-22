namespace BitcoinBlockchainParser.Streams;

internal class SubRangeStream(Stream stream, long pos, long length) : Stream
{
    public override long Position { get => stream.Position - pos; set => stream.Position = pos + value; }
    public override long Length => length;
    public override void SetLength(long value) => length = value;
    public override bool CanRead => stream.CanRead;
    public override bool CanSeek => stream.CanSeek;
    public override bool CanWrite => stream.CanWrite;
    public override int Read(byte[] buffer, int offset, int count) => stream.Read(buffer, offset, (int)Math.Min(Length - Position, count));
    public override void Write(byte[] buffer, int offset, int count) => stream.Write(buffer, offset, (int)Math.Min(Length - Position, count));
    public override long Seek(long offset, SeekOrigin origin) => stream.Seek(offset, origin);
    public override void Flush() => stream.Flush();
}
