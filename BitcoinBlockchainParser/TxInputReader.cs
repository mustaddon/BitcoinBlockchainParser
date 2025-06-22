namespace BitcoinBlockchainParser;


internal class TxInputReader(Stream stream)
{
    public TxInput Read()
    {
        var txIn = new TxInputRaw();

        txIn.TXID = stream.ReadBytes(32);
        txIn.VOUT = stream.ReadBytes(4);
        txIn.ScriptSig = stream.ReadBytes((int)stream.ReadCompactSize());
        txIn.Sequence = stream.ReadBytes(4);

        return new(txIn);
    }
}

public sealed class TxInputRaw
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public byte[] TXID;
    public byte[] VOUT;
    public byte[] ScriptSig;
    public byte[] Sequence;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}