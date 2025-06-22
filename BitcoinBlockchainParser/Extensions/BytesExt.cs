using BitcoinBlockchainParser.Encoders;

namespace BitcoinBlockchainParser.Extensions;

internal static class BytesExt
{
    //static readonly DateTime _unixEpoch = new DateTime(621355968000000000, DateTimeKind.Utc);

    public static DateTime ToUnixTime(this byte[] bytes) => DateTime.UnixEpoch.AddSeconds(bytes.ToUint32());


    public static ulong ToUint64(this byte[] bytes)
    {
        return bytes[0] | ((ulong)bytes[1] << 8) | ((ulong)bytes[2] << 16) | ((ulong)bytes[3] << 24) | ((ulong)bytes[4] << 32) | ((ulong)bytes[5] << 40) | ((ulong)bytes[6] << 48) | ((ulong)bytes[7] << 56);
    }

    public static uint ToUint32(this byte[] bytes)
    {
        return bytes[0] | ((uint)bytes[1] << 8) | ((uint)bytes[2] << 16) | ((uint)bytes[3] << 24);
    }

    public static int ToInt32(this byte[] bytes)
    {
        return bytes[0] | ((int)bytes[1] << 8) | ((int)bytes[2] << 16) | ((int)bytes[3] << 24);
    }

    public static uint ToUint16(this byte[] bytes)
    {
        return bytes[0] | ((uint)bytes[1] << 8);
    }

    public static int GetHashCode(this byte[] bytes, int seed)
    {
        var hc = bytes.Length;
        foreach (var x in bytes)
        {
            hc = unchecked(hc * seed + x);
        }
        return hc;
    }
}
