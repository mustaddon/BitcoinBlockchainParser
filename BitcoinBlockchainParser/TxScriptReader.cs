using BitcoinBlockchainParser.Encoders;

namespace BitcoinBlockchainParser;

public static class TxScriptReader
{
    public static IEnumerable<(Opcodes Opcode, byte[]? Data)> Read(byte[] script)
    {
        for (var i = 0; i < script.Length; i++)
        {
            var opcode = (Opcodes)script[i];

            if (opcode >= Opcodes.OP_PUSHBYTES_1 && opcode <= Opcodes.OP_PUSHBYTES_75)
            {
                var count = script[i];
                yield return (opcode, script[(i + 1)..(i + 1 + count)]);
                i += count;
            }
            else if (opcode == Opcodes.OP_PUSHDATA1)
            {
                var count = script[i + 1];
                yield return (opcode, script[(i + 2)..(i + 2 + count)]);
                i += count;
            }
            else if (opcode == Opcodes.OP_PUSHDATA2)
            {
                var count = (int)script[(i + 1)..(i + 3)].ToUint16();
                yield return (opcode, script[(i + 3)..(i + 3 + count)]);
                i += count;
            }
            else if (opcode == Opcodes.OP_PUSHDATA4)
            {
                var count = (int)script[(i + 1)..(i + 5)].ToUint32();
                yield return (opcode, script[(i + 5)..(i + 5 + count)]);
                i += count;
            }
            else
            {
                yield return (opcode, null);
            }
        }
    }

    public static string ReadToString(byte[] script)
    {
        var result = new StringBuilder();

        try
        {
            foreach (var (Opcode, Data) in Read(script))
            {
                result.AppendLine(Opcode.ToString());

                if (Data != null)
                    result.AppendLine(Data.ToHex());
            }
        }
        catch
        {
            result.AppendLine("SCRIPT ERROR");
        }

        if (result.Length > 2)
            result.Remove(result.Length - 2, 2);

        return result.ToString();
    }
}
