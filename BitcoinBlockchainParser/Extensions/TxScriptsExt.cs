using BitcoinBlockchainParser.Encoders;
using System.Net.WebSockets;

namespace BitcoinBlockchainParser.Extensions;

internal static class TxScriptsExt
{
    public static TxoScriptType GetScriptType(this byte[] scriptPubKey)
    {
        scriptPubKey = scriptPubKey.TrimNops();

        if (((scriptPubKey.Length == 67 && scriptPubKey[0] == 0x41) 
            || (scriptPubKey.Length == 35 && scriptPubKey[0] == 0x21))
            && scriptPubKey[^1] == 0xac)
            return TxoScriptType.P2PK;

        if (scriptPubKey.Length == 22
            && scriptPubKey[0] == 0x00 && scriptPubKey[1] == 0x14)
            return TxoScriptType.P2WPKH;

        if (scriptPubKey.Length == 25
            && scriptPubKey[0] == 0x76 && scriptPubKey[1] == 0xa9 && scriptPubKey[2] == 0x14
            && scriptPubKey[^2] == 0x88 && scriptPubKey[^1] == 0xac)
            return TxoScriptType.P2PKH;

        if (scriptPubKey.Length == 34
            && scriptPubKey[0] == 0x00 && scriptPubKey[1] == 0x20)
            return TxoScriptType.P2WSH;

        if (scriptPubKey.Length == 23
            && scriptPubKey[0] == 0xa9 && scriptPubKey[1] == 0x14
            && scriptPubKey[^1] == 0x87)
            return TxoScriptType.P2SH;

        if (scriptPubKey.Length == 34
            && scriptPubKey[0] == 0x51 && scriptPubKey[1] == 0x20)
            return TxoScriptType.P2TR;

        if (scriptPubKey.Length > 36
            && scriptPubKey[^1] == 0xae
            && scriptPubKey[0] >= 0x51 && scriptPubKey[0] <= 0x60
            && scriptPubKey[^2] >= 0x51 && scriptPubKey[^2] <= 0x60)
            return TxoScriptType.P2MS;

        if (scriptPubKey.Length > 1
            && scriptPubKey[0] == 0x6a)
            return TxoScriptType.OP_RETURN;

        return TxoScriptType.Unknown;
    }

    static byte[] TrimNops(this byte[] script)
    {
        int i = 0;

        for (; i < script.Length; i++)
            if (script[i] != (byte)Opcodes.OP_NOP)
                break;

        int j = script.Length - 1;

        for (; j > i; j--)
            if (script[j] != (byte)Opcodes.OP_NOP)
                break;

        if (i == 0 && j == script.Length - 1)
            return script;

        return script[i..^(script.Length - 1 - j)];
    }

    public static byte[] GetPK(this byte[] scriptPubKey) => scriptPubKey.TrimNops()[1..^1];
    public static byte[] GetPKH(this byte[] scriptPubKey) => scriptPubKey.TrimNops()[3..^2];
    public static byte[] GetSH(this byte[] scriptPubKey) => scriptPubKey.TrimNops()[2..^1];
    public static byte[] GetWPKH(this byte[] scriptPubKey) => scriptPubKey.TrimNops()[2..];
    public static byte[] GetWSH(this byte[] scriptPubKey) => scriptPubKey.TrimNops()[2..];
    public static byte[] GetTR(this byte[] scriptPubKey) => scriptPubKey.TrimNops()[2..];

    public static string? ToAddressP2PKH(this byte[] bytes, Network network)
    {
        if (bytes.Length != 20)
            return null;

        var checksum = (bytes = [network.AddressPKH, .. bytes]).HASH256()[0..4];
        return Base58.ToBase58([.. bytes, .. checksum]);
    }

    public static string? ToAddressP2SH(this byte[] bytes, Network network)
    {
        if (bytes.Length != 20)
            return null;

        var checksum = (bytes = [network.AddressSH, .. bytes]).HASH256()[0..4];
        return Base58.ToBase58([.. bytes, .. checksum]);
    }
}
