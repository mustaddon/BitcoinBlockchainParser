using BitcoinBlockchainParser;


var blockchain = new Blockchain(@"C:\Bitcoin\blocks");


/// Unordered Blocks

foreach (var block in blockchain.EnumerateBlocks().Take(5))
{
    Console.WriteLine($"Block: {block.Id}  Time: {block.Time:o}");

    foreach (var tx in block.Transactions.Take(3))
    {
        Console.WriteLine($"   TX: {tx.TXID}  Inputs: {tx.Inputs.Length}  Outputs: {tx.Outputs.Length}");
    }

    Console.WriteLine();
}


/// Ordered Blocks

foreach (var block in blockchain.EnumerateBlocksOrdered().Skip(50).Take(20))
{
    Console.WriteLine($"Block #{block.Height} {block.Id}  Time: {block.Time:o}");
}


