namespace BitcoinBlockchainParser.Encoders.NBitcoin;

internal static class ByteArrayExtensions
{
    internal static bool StartWith(this byte[] data, byte[] versionBytes)
    {
        if (data.Length < versionBytes.Length)
            return false;
        for (int i = 0; i < versionBytes.Length; i++)
        {
            if (data[i] != versionBytes[i])
                return false;
        }
        return true;
    }
    internal static byte[] SafeSubarray(this byte[] array, int offset, int count)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (offset < 0 || offset > array.Length)
            throw new ArgumentOutOfRangeException("offset");
        if (count < 0 || offset + count > array.Length)
            throw new ArgumentOutOfRangeException("count");
        if (offset == 0 && array.Length == count)
            return array;
        var data = new byte[count];
        Buffer.BlockCopy(array, offset, data, 0, count);
        return data;
    }

    internal static byte[] SafeSubarray(this byte[] array, int offset)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (offset < 0 || offset > array.Length)
            throw new ArgumentOutOfRangeException("offset");

        var count = array.Length - offset;
        var data = new byte[count];
        Buffer.BlockCopy(array, offset, data, 0, count);
        return data;
    }

    internal static byte[] Concat(this byte[] arr, params byte[][] arrs)
    {
        var len = arr.Length + arrs.Sum(a => a.Length);
        var ret = new byte[len];
        Buffer.BlockCopy(arr, 0, ret, 0, arr.Length);
        var pos = arr.Length;
        foreach (var a in arrs)
        {
            Buffer.BlockCopy(a, 0, ret, pos, a.Length);
            pos += a.Length;
        }
        return ret;
    }

}