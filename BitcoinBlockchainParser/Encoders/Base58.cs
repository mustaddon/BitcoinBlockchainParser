using BitcoinBlockchainParser.Encoders.NBitcoin;

namespace BitcoinBlockchainParser.Encoders;


public static class Base58
{
    private static readonly Base58Encoder _encoder = new();

    public static string ToBase58(this byte[] bytes) => _encoder.EncodeData(bytes);

    public static string? ToBase58Address(this byte[] bytes, byte type = 0)
    {
        if (bytes.Length != 20)
            return null;

        var checksum = Hashes.HASH256(bytes = [type, .. bytes])[0..4];
        return ToBase58([.. bytes, .. checksum]);
    }
}

