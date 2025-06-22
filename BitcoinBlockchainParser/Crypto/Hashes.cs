using BitcoinBlockchainParser.Crypto.BouncyCastle.crypto.digests;

namespace BitcoinBlockchainParser.Crypto;

public static class Hashes
{
    public static byte[] SHA256(this byte[] data)
    {
#if NET6_0_OR_GREATER
        return System.Security.Cryptography.SHA256.HashData(data);
#else
        using var sha256 = new System.Security.Cryptography.SHA256Managed();
        var hash = String.Empty;
        return sha256.ComputeHash(data);
#endif
    }

    public static byte[] RIPEMD160(this byte[] data)
    {
        var ripemd = new RipeMD160Digest();
        ripemd.BlockUpdate(data, 0, data.Length);
        var rv = new byte[20];
        ripemd.DoFinal(rv, 0);
        return rv;
    }

    public static byte[] HASH256(this byte[] bytes) => SHA256(SHA256(bytes));

    public static byte[] HASH160(this byte[] bytes) => RIPEMD160(SHA256(bytes));
}
