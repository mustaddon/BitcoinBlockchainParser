namespace BitcoinBlockchainParser.DataTypes;

public readonly struct PKH(byte[] bytes20) : IEquatable<PKH>, IComparable<PKH>, IComparable, IBytes
{
    private readonly byte[] _bytes = bytes20.Length == 20 ? bytes20 : throw new ArgumentException(nameof(bytes20));

    private static readonly byte[] _empty = new byte[20];

    public byte[] Bytes => _bytes ?? _empty;

    public override int GetHashCode() => GetHashCode(314159);

    public int GetHashCode(int seed)
    {
        var hc = _bytes.Length;
        hc = unchecked(hc * seed + (_bytes[0] | (_bytes[1] << 8) | (_bytes[2] << 16) | (_bytes[3] << 24)));
        hc = unchecked(hc * seed + (_bytes[4] | (_bytes[5] << 8) | (_bytes[6] << 16) | (_bytes[7] << 24)));
        hc = unchecked(hc * seed + (_bytes[8] | (_bytes[9] << 8) | (_bytes[10] << 16) | (_bytes[11] << 24)));
        hc = unchecked(hc * seed + (_bytes[12] | (_bytes[13] << 8) | (_bytes[14] << 16) | (_bytes[15] << 24)));
        hc = unchecked(hc * seed + (_bytes[16] | (_bytes[17] << 8) | (_bytes[18] << 16) | (_bytes[19] << 24)));
        return hc;
    }

    public override bool Equals(object? obj) => obj is PKH o && Equals(o);
    public bool Equals(PKH other) => Enumerable.SequenceEqual(Bytes, other.Bytes);

    public int CompareTo(object? obj) => obj is PKH o ? CompareTo(o) : 0;
    public int CompareTo(PKH other)
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