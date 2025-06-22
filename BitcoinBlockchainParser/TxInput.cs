namespace BitcoinBlockchainParser;

public class TxInput(TxInputRaw raw)
{
    public TxInputRaw Raw => raw;
    public byte[] TXID => raw.TXID;
    public uint VOUT => raw.VOUT.ToUint32();
    public byte[] ScriptSig => raw.ScriptSig;
    public LeField Sequence => new(raw.Sequence);
}
