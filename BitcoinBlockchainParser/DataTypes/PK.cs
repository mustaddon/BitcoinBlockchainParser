namespace BitcoinBlockchainParser.DataTypes;

public readonly struct PK(byte[] bytes) : IEquatable<PK>, IComparable<PK>, IComparable, IBytes
{
    public byte[] Bytes => bytes;

    public override int GetHashCode() => Bytes.GetHashCode(334297);

    public override bool Equals(object? obj) => obj is PK o && Equals(o);
    public bool Equals(PK other) => Enumerable.SequenceEqual(Bytes, other.Bytes);

    public int CompareTo(object? obj) => obj is PK o ? CompareTo(o) : 0;
    public int CompareTo(PK other)
    {
        var b = other.Bytes;

        for (int i = 0; i < Bytes.Length; i++)
        {
            if (Bytes[i] > b[i])
                return 1;
            else if (Bytes[i] < b[i])
                return -1;
        }

        return 0;
    }
}