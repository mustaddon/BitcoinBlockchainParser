namespace BitcoinBlockchainParser;

public class BlockFiled(BlockRaw raw, FilePosition filePosition) : Block(raw)
{
    public FilePosition FilePosition => filePosition;
}