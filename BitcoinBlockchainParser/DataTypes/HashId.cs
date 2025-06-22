using BitcoinBlockchainParser.Encoders;

namespace BitcoinBlockchainParser.DataTypes;

public readonly struct HashId(byte[] hash) : IEquatable<HashId>
{
    public byte[] Hash => hash;
    public string Id => hash.ToHexReversed();

    public override string ToString() => Id;
    public override int GetHashCode() => hash.GetHashCode(314233);
    public override bool Equals(object? obj) => obj is HashId o && Equals(o);
    public bool Equals(HashId other) => Enumerable.SequenceEqual(other.Hash, hash);
}
