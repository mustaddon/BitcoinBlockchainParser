namespace BitcoinBlockchainParser.Extensions;

internal static class StreamExt
{
    public static byte[] ReadBytes(this Stream stream, int count)
    {
        var buf = new byte[count];
        var readedTotal = 0;
        int readedLast;
        while (readedTotal < count && (readedLast = stream.Read(buf, readedTotal, count - readedTotal)) > 0)
        {
            readedTotal += readedLast;
        }
        return buf;
    }

    public static DateTime ReadUixTime(this Stream stream) => DateTime.UnixEpoch.AddSeconds(stream.ReadUint32());

    public static uint ReadUint32(this Stream stream) => stream.ReadBytes(4).ToUint32();
    public static ulong ReadUint64(this Stream stream) => stream.ReadBytes(8).ToUint64();


    public static ulong ReadCompactSize(this Stream stream) => ReadCompactSize(stream, (byte)stream.ReadByte());
    public static ulong ReadCompactSize(this Stream stream, byte b)
    {
        return b switch
        {
            0xFD => stream.ReadBytes(2).ToUint16(),
            0xFE => stream.ReadBytes(4).ToUint32(),
            0xFF => stream.ReadBytes(8).ToUint64(),
            _ => b,
        };
    }

}
