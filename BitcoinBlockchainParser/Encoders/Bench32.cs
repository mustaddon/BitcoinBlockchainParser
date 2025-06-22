using BitcoinBlockchainParser.Encoders.NBitcoin;

namespace BitcoinBlockchainParser.Encoders;


public static class Bench32
{
    public static string ToBench32(this byte[] bytes, byte[] hrp, byte witnessVerion) => new Bech32mEncoder(hrp).Encode(witnessVerion, bytes);

    public static string ToBench32(this byte[] bytes, Network network, byte witnessVerion = 0) => network.Bench32.Encode(witnessVerion, bytes);

    public static string ToBench32(this byte[] bytes, byte witnessVerion = 0) => ToBench32(bytes, Network.Default, witnessVerion);
}

