namespace BitcoinBlockchainParser.DataTypes;

public readonly struct TxScript(byte[] bytes) : ITxScript
{
    public byte[] Bytes => bytes;

    public override string ToString() => TxScriptReader.ReadToString(bytes);
}
