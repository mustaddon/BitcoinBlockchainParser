using System;

namespace BitcoinBlockchainParser;

public class BlockOrdered(BlockRaw raw, FilePosition filePosition, int height) : BlockFiled(raw, filePosition)
{
    public int Height => height;

    [Obsolete("This property is deprecated, use 'Height' instead.")]
    public int Index => height;
}