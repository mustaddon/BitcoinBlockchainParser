using BitcoinBlockchainParser.Encoders;

namespace BitcoinBlockchainParser;

public interface IBytes
{
    public byte[] Bytes { get; }
}

public static class IBytesExt
{
    public static string ToHex(this ITxScript script) => script.Bytes.ToHex();
}