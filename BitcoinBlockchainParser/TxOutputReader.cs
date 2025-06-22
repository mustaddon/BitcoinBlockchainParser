namespace BitcoinBlockchainParser;

internal class TxOutputReader(Stream stream, Network network)
{
    public TxOutput Read()
    {
        var txOut = new TxOutputRaw();

        txOut.Amount = stream.ReadBytes(8);
        txOut.ScriptPubKey = stream.ReadBytes((int)stream.ReadCompactSize());

        return new(txOut, network);
    }
}

public sealed class TxOutputRaw
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public byte[] Amount;
    public byte[] ScriptPubKey;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}