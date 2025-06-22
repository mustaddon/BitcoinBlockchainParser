namespace BitcoinBlockchainParser.Encoders.NBitcoin;

internal abstract class DataEncoder
{
    // char.IsWhiteSpace fits well but it match other whitespaces 
    // characters too and also works for unicode characters.
    public static bool IsSpace(char c)
    {
        switch (c)
        {
            case ' ':
            case '\t':
            case '\n':
            case '\v':
            case '\f':
            case '\r':
                return true;
        }
        return false;
    }

    internal DataEncoder()
    {
    }

    public string EncodeData(byte[] data)
    {
        return EncodeData(data, 0, data.Length);
    }

    public abstract string EncodeData(byte[] data, int offset, int count);

    public abstract byte[] DecodeData(string encoded);
}

internal static class Encoders
{
    public static readonly ASCIIEncoder ASCII = new();

    public static readonly Base58Encoder Base58 = new();
    public static Bech32Encoder Bech32(string hrp)
    {
        return new Bech32Encoder(hrp);
    }
    public static Bech32Encoder Bech32(byte[] hrp)
    {
        return new Bech32Encoder(hrp);
    }
}
