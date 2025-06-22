using BitcoinBlockchainParser.BouncyCastle.Math;

namespace BitcoinBlockchainParser.Encoders.NBitcoin;


internal class Base58Encoder : DataEncoder
{
    /// <summary>
    /// Fast check if the string to know if base58 str
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public virtual bool IsMaybeEncoded(string str)
    {
        bool maybeb58 = true;
        if (maybeb58)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!pszBase58Chars.Contains(str[i]))
                {
                    maybeb58 = false;
                    break;
                }
            }
        }
        return maybeb58 && str.Length > 0;
    }

    static readonly BigInteger bn58 = BigInteger.ValueOf(58);
    public override string EncodeData(byte[] data, int offset, int count)
    {

        BigInteger bn0 = BigInteger.Zero;

        // Convert big endian data to little endian
        // Extra zero at the end make sure bignum will interpret as a positive number
        var vchTmp = data.SafeSubarray(offset, count);

        // Convert little endian data to bignum
        var bn = new BigInteger(1, vchTmp);

        // Convert bignum to std::string
        StringBuilder builder = new StringBuilder();
        // Expected size increase from base58 conversion is approximately 137%
        // use 138% to be safe

        while (bn.CompareTo(bn0) > 0)
        {
            var r = bn.DivideAndRemainder(bn58);
            var dv = r[0];
            BigInteger rem = r[1];
            bn = dv;
            var c = rem.IntValue;
            builder.Append(pszBase58[c]);
        }

        // Leading zeros encoded as base58 zeros
        for (int i = offset; i < offset + count && data[i] == 0; i++)
            builder.Append(pszBase58[0]);

        // Convert little endian std::string to big endian
        var chars = builder.ToString().ToCharArray();
        Array.Reverse(chars);
        var str = new String(chars); //keep that way to be portable
        return str;
    }


    internal const string pszBase58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
    internal static readonly char[] pszBase58Chars = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz".ToCharArray();


    public override byte[] DecodeData(string encoded)
    {
        if (encoded == null)
            throw new ArgumentNullException(nameof(encoded));

        var result = new byte[0];
        if (encoded.Length == 0)
            return result;
        BigInteger bn = BigInteger.Zero;
        int i = 0;
        while (IsSpace(encoded[i]))
        {
            i++;
            if (i >= encoded.Length)
                return result;
        }

        for (int y = i; y < encoded.Length; y++)
        {
            var p1 = pszBase58.IndexOf(encoded[y]);
            if (p1 == -1)
            {
                while (IsSpace(encoded[y]))
                {
                    y++;
                    if (y >= encoded.Length)
                        break;
                }
                if (y != encoded.Length)
                    throw new FormatException("Invalid base 58 string");
                break;
            }
            var bnChar = BigInteger.ValueOf(p1);
            bn = bn.Multiply(bn58);
            bn = bn.Add(bnChar);
        }

        // Get bignum as little endian data
        var vchTmp = bn.ToByteArrayUnsigned();
        Array.Reverse(vchTmp);
        if (vchTmp.All(b => b == 0))
            vchTmp = new byte[0];

        // Trim off sign byte if present
        if (vchTmp.Length >= 2 && vchTmp[vchTmp.Length - 1] == 0 && vchTmp[vchTmp.Length - 2] >= 0x80)
            vchTmp = vchTmp.SafeSubarray(0, vchTmp.Length - 1);

        // Restore leading zeros
        int nLeadingZeros = 0;
        for (int y = i; y < encoded.Length && encoded[y] == pszBase58[0]; y++)
            nLeadingZeros++;


        result = new byte[nLeadingZeros + vchTmp.Length];
        Array.Copy(vchTmp.Reverse().ToArray(), 0, result, nLeadingZeros, vchTmp.Length);
        return result;
    }
}