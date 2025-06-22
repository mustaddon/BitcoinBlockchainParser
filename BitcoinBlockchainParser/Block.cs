namespace BitcoinBlockchainParser;

public class Block(BlockRaw raw)
{
    public BlockRaw Raw => raw;
    public uint Size => raw.Size;
    public LeField Version => new(raw.Version);
    public HashId PreviousBlock => new(raw.PreviousBlock);
    public HashId MerkleRoot => new(raw.MerkleRoot);
    public DateTime Time => raw.Time.ToUnixTime();
    public LeField Bits => new(raw.Bits);
    public uint Nonce => raw.Nonce.ToUint32();
    public Transaction[] Transactions => raw.Transactions.Value;

    private byte[]? _hash;
    public byte[] Hash => _hash ??= Hashes.HASH256([.. raw.Version, .. raw.PreviousBlock, .. raw.MerkleRoot, .. raw.Time, .. raw.Bits, .. raw.Nonce]);

    public string Id => new HashId(Hash).Id;

    public override string ToString() => Id;
    public override int GetHashCode() => new HashId(Hash).GetHashCode();
    public override bool Equals(object? obj) => obj is Block o && Enumerable.SequenceEqual(o.Hash, Hash);
}



