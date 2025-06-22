namespace BitcoinBlockchainParser.Encoders.NBitcoin;

internal class Bech32mEncoder(byte[] hrp) : Bech32Encoder(hrp)
{
    const uint BECH32M_CONST = 0x2bc830a3;

    protected override byte[] CreateChecksum(byte[] data, int offset, int count)
    {
        if (data[0] == 0)
            return base.CreateChecksum(data, offset, count);

        var values = new byte[_HrpExpand.Length + count + 6];
        var valuesOffset = 0;
        Array.Copy(_HrpExpand, 0, values, valuesOffset, _HrpExpand.Length);
        valuesOffset += _HrpExpand.Length;
        Array.Copy(data, offset, values, valuesOffset, count);
        valuesOffset += count;
        var polymod = Polymod(values) ^ BECH32M_CONST;
        var ret = new byte[6];
        foreach (var i in Enumerable.Range(0, 6))
        {
            ret[i] = (byte)((polymod >> 5 * (5 - i)) & 31);
        }
        return ret;
    }
}
