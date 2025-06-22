using BitcoinBlockchainParser.Encoders;

namespace BitcoinBlockchainParser.DataTypes;

public readonly struct LeField(byte[] bytes) : IEquatable<LeField>
{
    public byte[] Bytes => bytes;
    public string Text => "0x" + bytes.ToHexReversed();

    public uint ToUint32() => bytes.ToUint32();
    public int ToInt32() => bytes.ToInt32();
    public DateTime ToUnixTime() => bytes.ToUnixTime();
    public override string ToString() => Text;
    public override int GetHashCode() => bytes.GetHashCode(324239);
    public override bool Equals(object? obj) => obj is LeField o && Equals(o);
    public bool Equals(LeField other) => Enumerable.SequenceEqual(other.Bytes, bytes);
}
