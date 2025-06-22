namespace BitcoinBlockchainParser;

public class BlockReader(Stream stream, Network network)
{
    private readonly Network _network = network ?? Network.Default;

    public Block? Read()
    {
        var begin = stream.Position;

        if (!Enumerable.SequenceEqual(stream.ReadBytes(4), _network.Magic))
        {
            stream.Position = begin;
            return null;
        }
        var block = new BlockRaw();
        block.Size = stream.ReadUint32();
        block.Version = stream.ReadBytes(4);
        block.PreviousBlock = stream.ReadBytes(32);
        block.MerkleRoot = stream.ReadBytes(32);
        block.Time = stream.ReadBytes(4);
        block.Bits = stream.ReadBytes(4);
        block.Nonce = stream.ReadBytes(4);

        var nextBlockPosition = begin + block.Size + 8;

        block.Transactions = new(() =>
        {
            using var _ = new ReturnStreamPosition(stream);
            stream.Position = begin + 88;
            var txCount = (int)stream.ReadCompactSize();
            var txReader = new TransactionReader(new SubRangeStream(stream, stream.Position, nextBlockPosition - stream.Position), _network);
            return Enumerable.Range(0, txCount).Select(x => txReader.Read()).ToArray();
        });

        stream.Position = nextBlockPosition;

        return new Block(block);
    }
}

public sealed class BlockRaw
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public uint Size;
    public byte[] Version;
    public byte[] PreviousBlock;
    public byte[] MerkleRoot;
    public byte[] Time;
    public byte[] Bits;
    public byte[] Nonce;
    public Lazy<Transaction[]> Transactions;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}

