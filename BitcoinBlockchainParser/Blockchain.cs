namespace BitcoinBlockchainParser;

public class Blockchain(string blocksDir, Network? network = null)
{
    private readonly Network _network = network ?? Network.Default;

    public IEnumerable<BlockFiled> EnumerateBlocks() => EnumerateBlocks(0, 0);

    public IEnumerable<BlockFiled> EnumerateBlocks(int startFile, long position = 0)
    {
        foreach (var file in GetBlkFiles())
        {
            var num = GetBlkNumber(file) ?? -1;

            if (num < startFile)
                continue;

            foreach (var block in EnumerateFileBlocks(file, num, num == startFile ? position : 0))
                yield return block;
        }
    }

    private static readonly HashId _zeroblock = new(new byte[32]);

    public IEnumerable<BlockOrdered> EnumerateBlocksOrdered()
    {
        var lastHash = _zeroblock;
        var lastTime = DateTime.MinValue;
        var buffer = new Dictionary<HashId, BlockFiled>();
        var fileIndex = -1;
        var blockIndex = -1;

        foreach (var file in GetBlkFiles())
        {
            var num = GetBlkNumber(file) ?? -1;

            if (++fileIndex != num)
                break;

            foreach (var block in EnumerateFileBlocks(file, num, 0))
            {
                var prev = block.PreviousBlock;

                if (!lastHash.Equals(prev))
                {
                    buffer[prev] = block;
                    continue;
                }

                lastHash = new(block.Hash);
                lastTime = block.Time;
                yield return new(block.Raw, block.FilePosition, ++blockIndex);

                while (buffer.TryGetValue(lastHash, out var next))
                {
                    buffer.Remove(lastHash);
                    lastHash = new(next.Hash);
                    lastTime = next.Time;

                    if (next.FilePosition.Number == block.FilePosition.Number)
                    {
                        yield return new(next.Raw, next.FilePosition, ++blockIndex);
                    }
                    else
                    {
                        using var stream = new BlkFileStream(next.FilePosition.File, _xor.Value, _network);
                        stream.Position = next.FilePosition.Position;
                        yield return new(new BlockReader(stream, _network).Read()!.Raw, next.FilePosition, ++blockIndex);
                    }
                }
            }
        }

        if (blockIndex < 0)
            throw new OperationCanceledException($"Could not find block #0.");
    }

    public IEnumerable<BlockFiled> EnumerateFileBlocks(int file, long position = 0)
    {
        var filepath = GetBlkFiles().FirstOrDefault(x => GetBlkNumber(x) == file);

        if (filepath == null)
            return [];

        return EnumerateFileBlocks(filepath, file, position);
    }

    public IEnumerable<BlockFiled> EnumerateFileBlocks(string filepath, long position = 0)
    {
        return EnumerateFileBlocks(filepath, GetBlkNumber(filepath) ?? -1, position);
    }

    private IEnumerable<BlockFiled> EnumerateFileBlocks(string filepath, int num, long position)
    {
        using var stream = new BlkFileStream(filepath, _xor.Value, _network);
        stream.Position = position;
        var reader = new BlockReader(stream, _network);

        while (true)
        {
            var pos = stream.Position;
            var block = reader.Read();

            if (block == null)
                break;

            yield return new(block.Raw, new(filepath, num, pos));
        }
    }

    private readonly Lazy<byte[]?> _xor = new(() =>
    {
        var path = Path.GetFullPath(Path.Combine(blocksDir, "xor.dat"));
        return File.Exists(path) ? File.ReadAllBytes(path) : null;
    });

    private IEnumerable<string> GetBlkFiles()
    {
        return Directory.GetFiles(Path.GetFullPath(blocksDir), "blk*.dat", SearchOption.TopDirectoryOnly).OrderBy(static x => x);
    }

    private static int? GetBlkNumber(string filepath)
    {
        var name = Path.GetFileNameWithoutExtension(filepath);
        return name.Length > 3 && int.TryParse(name.AsSpan(3), out var val) ? val : null;
    }
}


