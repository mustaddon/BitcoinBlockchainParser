namespace BitcoinBlockchainParser;

public interface ITxScript : IBytes
{

}

public static class ITxScriptExt
{
    public static string ToAsm(this ITxScript script) => TxScriptReader.ReadToString(script.Bytes);

    public static IEnumerable<(Opcodes Opcode, byte[]? Data)> Read(this ITxScript script) => TxScriptReader.Read(script.Bytes);

    public static bool TryReadAll(this ITxScript script, out List<(Opcodes Opcode, byte[]? Data)> result)
    {
        result = [];

        try
        {
            foreach (var x in TxScriptReader.Read(script.Bytes))
            {
                result.Add(x);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

}