namespace BitcoinBlockchainParser;

public class Transaction(TransactionRaw raw)
{
    public TransactionRaw Raw => raw;
    public uint Version => raw.Version.ToUint32();
    public byte Marker => raw.Marker;
    public byte Flag => raw.Flag;
    public TxInput[] Inputs => raw.Inputs;
    public TxOutput[] Outputs => raw.Outputs;
    public TxWitness[] Witness => raw.Witness;
    public uint Locktime => raw.Locktime.ToUint32();

    private byte[]? _txid;
    public HashId TXID => new(_txid ??= Hashes.HASH256(raw.TxidData));

    public override string ToString() => TXID.Id;
    public override int GetHashCode() => TXID.GetHashCode();
    public override bool Equals(object? obj) => obj is Transaction o && TXID.Equals(o.TXID);
}
