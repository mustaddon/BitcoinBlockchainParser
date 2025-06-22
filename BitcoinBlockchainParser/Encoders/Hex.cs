namespace BitcoinBlockchainParser.Encoders;

public static class Hex
{
    public static string ToHexReversed(this byte[] bytes) => ToHex(bytes.Reverse().ToArray());

    public static string ToHex(this byte[] bytes)
    {
#if NET9_0_OR_GREATER
        return Convert.ToHexStringLower(bytes);
#else
        return string.Join(string.Empty, bytes.Select(x => x.ToString("x2")));
#endif
    }
}
