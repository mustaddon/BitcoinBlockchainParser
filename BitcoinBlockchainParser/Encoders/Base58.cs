using BitcoinBlockchainParser.Encoders.NBitcoin;

namespace BitcoinBlockchainParser.Encoders;


public static class Base58
{
    private static readonly Base58Encoder _encoder = new();

    public static string ToBase58(this byte[] bytes) => _encoder.EncodeData(bytes);

    public static string? ToBase58WithChecksum(this byte[] bytes)
    {
        var checksum = Hashes.HASH256(bytes)[0..4];
        return ToBase58([.. bytes, .. checksum]);
    }

    public static string? ToBase58Address(this byte[] bytes, byte type = 0)
    {
        if (bytes.Length != 20)
            return null;

        return ToBase58WithChecksum([type, .. bytes]);
    }

    public static string? ToBase58PrivatKey(this byte[] bytes, bool compression = true, byte prefix = 0x80)
    {
        if (bytes.Length != 32)
            return null;

        return ToBase58WithChecksum(compression ? [prefix, .. bytes, 1] : [prefix, .. bytes]);
    }
}

