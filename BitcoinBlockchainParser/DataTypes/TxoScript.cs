namespace BitcoinBlockchainParser.DataTypes;

public readonly struct TxoScript(byte[] bytes)
{
    public byte[] Bytes => bytes;

    public TxoScriptType Type => bytes.GetScriptType();
}
