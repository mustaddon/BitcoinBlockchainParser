namespace BitcoinBlockchainParser;

internal class TransactionReader
{
    public TransactionReader(Stream stream, Network network)
    {
        _stream = new(stream);
        _inReader = new(_stream);
        _outReader = new(_stream, network);
        _witnessReader = new(_stream);
    }


    readonly ReadedBufferStream _stream;
    readonly TxInputReader _inReader;
    readonly TxOutputReader _outReader;
    readonly TxWitnessReader _witnessReader;

    public Transaction Read()
    {
        var tx = new TransactionRaw();

        _stream.Buffer.Clear();
        tx.Version = _stream.ReadBytes(4);

        _stream.DisableBuffer();
        tx.Marker = (byte)_stream.ReadByte();

        if (tx.Marker == 0)
            Bip141(tx);
        else
            Old(tx);

        tx.TxidData = _stream.BufferToArray();

        return new(tx);
    }

    void Old(TransactionRaw tx)
    {
        _stream.EnableBuffer();
        _stream.Buffer.Add([tx.Marker]);

        tx.Inputs = Enumerable.Range(0, (int)_stream.ReadCompactSize(tx.Marker)).Select(x => _inReader.Read()).ToArray();
        tx.Outputs = Enumerable.Range(0, (int)_stream.ReadCompactSize()).Select(x => _outReader.Read()).ToArray();
        tx.Locktime = _stream.ReadBytes(4);
    }

    void Bip141(TransactionRaw tx)
    {
        tx.Flag = (byte)_stream.ReadByte();

        _stream.EnableBuffer();
        tx.Inputs = Enumerable.Range(0, (int)_stream.ReadCompactSize()).Select(x => _inReader.Read()).ToArray();
        tx.Outputs = Enumerable.Range(0, (int)_stream.ReadCompactSize()).Select(x => _outReader.Read()).ToArray();

        _stream.DisableBuffer();
        tx.Witness = Enumerable.Range(0, tx.Inputs.Length).Select(x => _witnessReader.Read()).ToArray();

        _stream.EnableBuffer();
        tx.Locktime = _stream.ReadBytes(4);
    }
}

public sealed class TransactionRaw
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public byte[] Version;
    public byte Marker;
    public byte Flag;
    public TxInput[] Inputs;
    public TxOutput[] Outputs;
    public TxWitness[] Witness;
    public byte[] Locktime;
    public byte[] TxidData;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}