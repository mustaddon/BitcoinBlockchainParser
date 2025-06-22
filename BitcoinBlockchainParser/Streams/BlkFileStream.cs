namespace BitcoinBlockchainParser.Streams;

public class BlkFileStream(string filepath, byte[]? xor = null, Network? network = null) : Stream
{
    private readonly FileStream _fileStream = new(filepath, FileMode.Open, FileAccess.Read);

    private readonly byte[]? _xor = xor?.Length > 0 ? xor : null;

    private readonly Network _network = network ?? Network.Default;

    public override int Read(byte[] buffer, int offset, int count)
    {
        var start = Position;
        var len = _fileStream.Read(buffer, offset, count);

        if (_xor != null && NeedDecode())
        {
            for (var i = 0; i < len; i++)
            {
                var bi = offset + i;
                var xi = (start + i) % xor!.Length;
                buffer[bi] = (byte)(buffer[bi] ^ xor[xi]);
            }
        }

        return len;
    }

    private bool? _needDecode;

    private bool NeedDecode() => _needDecode ?? (_needDecode = !CheckMagic()) ?? true;

    private bool CheckMagic()
    {
        var pos = _fileStream.Position;
        _fileStream.Position = 0;
        var magic = _fileStream.ReadBytes(4);
        _fileStream.Position = pos;
        return magic.SequenceEqual(_network.Magic);
    }


    private bool _isDisposed;

    protected override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            _isDisposed = true;

            if (disposing) _fileStream.Dispose();
        }

        base.Dispose(disposing);
    }

    public override bool CanRead => _fileStream.CanRead;
    public override bool CanSeek => _fileStream.CanSeek;
    public override bool CanWrite => _fileStream.CanWrite;
    public override long Length => _fileStream.Length;
    public override long Position { get => _fileStream.Position; set => _fileStream.Position = value; }
    public override void Flush() => _fileStream.Flush();
    public override long Seek(long offset, SeekOrigin origin) => _fileStream.Seek(offset, origin);
    public override void SetLength(long value) => _fileStream.SetLength(value);
    public override void Write(byte[] buffer, int offset, int count) => _fileStream.Write(buffer, offset, count);
}
