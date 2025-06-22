# BitcoinBlockchainParser [![NuGet version](https://badge.fury.io/nu/BitcoinBlockchainParser.svg?1000)](http://badge.fury.io/nu/BitcoinBlockchainParser)
.NET Library provides parsing functionality over files containing the Bitcoin blockchain.

## Features
- Detects outputs types
- Detects addresses in outputs
- Interprets scripts
- Supports SegWit and Taproot
- Supports ordered block parsing
- Supports file XOR-deobfuscation (Bitcoin Core 28.0 or newer)


## Examples
Below are two basic examples for parsing the blockchain. 

### Unordered Blocks
This blockchain parser parses raw blocks saved in Bitcoin Core's `blk` file format. 
Bitcoin Core does not guarantee that these blocks are saved in order. 
If your application does not require that blocks are parsed in order, the `EnumerateBlocks` method can be used

```C#
using BitcoinBlockchainParser;

var blockchain = new Blockchain(@"C:\Bitcoin\blocks");

foreach (var block in blockchain.EnumerateBlocks().Take(5))
{
    Console.WriteLine($"Block: {block.Id}  Time: {block.Time:o}");

    foreach (var tx in block.Transactions.Take(3))
    {
        Console.WriteLine($"   TX: {tx.TXID}  Ins: {tx.Inputs.Length}  Outs: {tx.Outputs.Length}");
    }

    Console.WriteLine();
}
```

### Ordered Blocks
If maintaining block order is necessary for your application, you should use the `EnumerateBlocksOrdered` method.

```C#
using BitcoinBlockchainParser;

var blockchain = new Blockchain(@"C:\Bitcoin\blocks");

foreach (var block in blockchain.EnumerateBlocksOrdered().Skip(50).Take(20))
{
    Console.WriteLine($"Block #{block.Index} {block.Id}  Time: {block.Time:o}");
}
```

[Program.cs](https://github.com/mustaddon/BitcoinBlockchainParser/blob/main/ExampleApp/Program.cs)