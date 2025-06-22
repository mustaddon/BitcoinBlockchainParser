using BitcoinBlockchainParser.Encoders.NBitcoin;

namespace BitcoinBlockchainParser.Encoders;


public static class Base58
{
    public static string ToBase58(this byte[] bytes) => _encoder.EncodeData(bytes);

    private static readonly Base58Encoder _encoder = new();
}

