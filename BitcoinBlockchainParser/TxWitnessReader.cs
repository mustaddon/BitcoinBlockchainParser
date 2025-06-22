namespace BitcoinBlockchainParser;

internal class TxWitnessReader(Stream stream)
{
    public TxWitness Read()
    {
        var witness = new TxWitnessRaw();

        witness.StackItems = Enumerable.Range(0, (int)stream.ReadCompactSize())
            .Select(x => stream.ReadBytes((int)stream.ReadCompactSize()))
            .ToArray();

        return new(witness);
    }
}


public sealed class TxWitnessRaw
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public byte[][] StackItems;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}