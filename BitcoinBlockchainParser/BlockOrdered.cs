namespace BitcoinBlockchainParser;

public class BlockOrdered(BlockRaw raw, FilePosition filePosition, int index) : BlockFiled(raw, filePosition)
{
    public int Index => index;
}