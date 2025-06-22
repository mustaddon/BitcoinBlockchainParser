namespace BitcoinBlockchainParser.Streams;

internal class ReturnStreamPosition(Stream stream) : IDisposable
{
    readonly long _pos = stream.Position;

    public void Dispose() => stream.Position = _pos;
}
