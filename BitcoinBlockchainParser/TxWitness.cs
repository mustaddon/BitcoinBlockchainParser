namespace BitcoinBlockchainParser;

public class TxWitness(TxWitnessRaw raw)
{
    public TxWitnessRaw Raw => raw;
    public byte[][] StackItems => raw.StackItems;
}