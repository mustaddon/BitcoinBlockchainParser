using BitcoinBlockchainParser.Encoders;

namespace BitcoinBlockchainParser.DataTypes;

public readonly struct TxiScript(byte[] bytes)
{
    public byte[] Bytes => bytes;

    public string ToHex() => bytes.ToHex();

    public override string ToString() => TxScriptReader.ReadToString(bytes);

    public IEnumerable<(Opcodes Opcode, byte[]? Data)> Read() => TxScriptReader.Read(bytes);
}
