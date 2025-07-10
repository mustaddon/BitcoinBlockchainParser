namespace BitcoinBlockchainParser;

public class TxInput(TxInputRaw raw)
{
    public TxInputRaw Raw => raw;
    public byte[] TXID => raw.TXID;
    public uint VOUT => raw.VOUT.ToUint32();
    public TxScript ScriptSig => new(raw.ScriptSig);
    public LeField Sequence => new(raw.Sequence);
}
