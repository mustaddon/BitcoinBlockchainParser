namespace BitcoinBlockchainParser.Encoders;


// Source:  https://github.com/bitcoin/bitcoin/blob/master/src/script/descriptor.cpp

public static class Descriptor
{
    const string INPUT_CHARSET = @"0123456789()[],'/*abcdefgh@:$%{}IJKLMNOPQRSTUVWXYZ&+-.;<=>?!^_|~ijklmnopqrstuvwxyzABCDEFGH`#""\ ";
    const string CHECKSUM_CHARSET = "qpzry9x8gf2tvdw0s3jn54khce6mua7l";

    public static string AddChecksum(string desc) => $"{desc}#{Checksum(desc)}";

    public static string Checksum(string str)
    {
        ulong c = 1;
        int cls = 0;
        int clscount = 0;
        foreach (var ch in str)
        {
            var pos = INPUT_CHARSET.IndexOf(ch);
            if (pos < 0) return string.Empty;
            c = PolyMod(c, pos & 31); // Emit a symbol for the position inside the group, for every character.
            cls = cls * 3 + (pos >> 5); // Accumulate the group numbers
            if (++clscount == 3)
            {
                // Emit an extra symbol representing the group numbers, for every 3 characters.
                c = PolyMod(c, cls);
                cls = 0;
                clscount = 0;
            }
        }
        if (clscount > 0) c = PolyMod(c, cls);
        for (int j = 0; j < 8; ++j) c = PolyMod(c, 0); // Shift further to determine the checksum.
        c ^= 1; // Prevent appending zeroes from not affecting the checksum.

        return new string([.. Enumerable.Range(0, 8).Select(j => CHECKSUM_CHARSET[(int)((c >> (5 * (7 - j))) & 31)])]);
    }

    static ulong PolyMod(ulong c, int val)
    {
        byte c0 = (byte)(c >> 35);
        c = ((c & 0x7ffffffff) << 5) ^ (ulong)val;
        if ((c0 & 1) > 0) c ^= 0xf5dee51989;
        if ((c0 & 2) > 0) c ^= 0xa9fdca3312;
        if ((c0 & 4) > 0) c ^= 0x1bab10e32d;
        if ((c0 & 8) > 0) c ^= 0x3706b1677a;
        if ((c0 & 16) > 0) c ^= 0x644d626ffd;
        return c;
    }
}
