using BitcoinBlockchainParser.Encoders;

namespace BitcoinBlockchainParser;

public class TxOutput(TxOutputRaw raw, Network network)
{
    public TxOutputRaw Raw => raw;
    public ulong Amount => raw.Amount.ToUint64();
    public TxoScript ScriptPubKey => new(raw.ScriptPubKey);

    private TxoScriptType? _type;
    public TxoScriptType Type => _type ??= ScriptPubKey.Type;

    public byte[]? Taproot => Type == TxoScriptType.P2TR ? raw.ScriptPubKey.GetTR() : null;

    public byte[]? PubKey => Type == TxoScriptType.P2PK ? raw.ScriptPubKey.GetPK() : null;

    public byte[]? PubHash => Type switch
    {
        TxoScriptType.P2PKH => raw.ScriptPubKey.GetPKH(),
        TxoScriptType.P2WPKH => raw.ScriptPubKey.GetWPKH(),
        _ => PubKey?.HASH160()
    };

    public byte[]? ScriptHash => Type switch
    {
        TxoScriptType.P2SH => raw.ScriptPubKey.GetSH(),
        TxoScriptType.P2WSH => raw.ScriptPubKey.GetWSH(),
        _ => null
    };

    private string? _address;
    public string? Address => _address ??= Type switch
    {
        TxoScriptType.P2PK => PubHash?.ToAddressP2PKH(network),
        TxoScriptType.P2PKH => PubHash?.ToAddressP2PKH(network),
        TxoScriptType.P2SH => ScriptHash?.ToAddressP2SH(network),
        TxoScriptType.P2WPKH => PubHash?.ToBench32(network),
        TxoScriptType.P2WSH => ScriptHash?.ToBench32(network),
        TxoScriptType.P2TR => Taproot?.ToBench32(network, 1),
        //TxoScriptType.OP_RETURN => nameof(TxoScriptType.OP_RETURN),
        _ => null,
    };
}
