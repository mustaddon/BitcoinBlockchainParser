﻿namespace BitcoinBlockchainParser.Encoders.NBitcoin;

internal class ASCIIEncoder : DataEncoder
{
    public static readonly ASCIIEncoder Instance = new();

    //Do not using Encoding.ASCII (not portable)
    public override byte[] DecodeData(string encoded)
    {
        if (String.IsNullOrEmpty(encoded))
            return new byte[0];
        return encoded.ToCharArray().Select(o => (byte)o).ToArray();
    }

    public override string EncodeData(byte[] data, int offset, int count)
    {
        return new String(data.Skip(offset).Take(count).Select(o => (char)o).ToArray()).Replace("\0", "");
    }
}
