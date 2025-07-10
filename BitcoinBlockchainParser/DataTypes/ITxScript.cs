using BitcoinBlockchainParser.Encoders;

namespace BitcoinBlockchainParser;

public interface ITxScript : IBytes
{

}

public static class ITxScriptExt
{
    public static string ToAsm(this ITxScript script) => TxScriptReader.ReadToString(script.Bytes);

    public static IEnumerable<(Opcodes Opcode, byte[]? Data)> Read(this ITxScript script) => TxScriptReader.Read(script.Bytes);
}