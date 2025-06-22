namespace BitcoinBlockchainParser.Streams;

internal class ReadedBufferStream(Stream stream) : Stream
{
    public readonly List<byte[]> Buffer = [];

    bool _disabled;
    public void DisableBuffer() => _disabled = true;
    public void EnableBuffer() => _disabled = false;

    public byte[] BufferToArray()
    {
        var rv = new byte[Buffer.Sum(a => a.Length)];
        var offset = 0;
        foreach (var array in Buffer)
        {
            System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
            offset += array.Length;
        }
        return rv;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var r = stream.Read(buffer, offset, count);

        if (!_disabled && r > 0)
        {
            var buf = new byte[r];
            Array.Copy(buffer, offset, buf, 0, r);
            Buffer.Add(buf);
        }

        return r;
    }


    public override long Position { get => stream.Position; set => stream.Position = value; }
    public override long Length => stream.Length;
    public override void SetLength(long value) => stream.SetLength(value);
    public override bool CanRead => stream.CanRead;
    public override bool CanSeek => stream.CanSeek;
    public override bool CanWrite => stream.CanWrite;
    public override void Flush() => stream.Flush();
    public override long Seek(long offset, SeekOrigin origin) => stream.Seek(offset, origin);
    public override void Write(byte[] buffer, int offset, int count) => stream.Write(buffer, offset, count);

}
