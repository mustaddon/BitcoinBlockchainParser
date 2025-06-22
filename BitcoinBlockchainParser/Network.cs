using BitcoinBlockchainParser.Encoders.NBitcoin;

namespace BitcoinBlockchainParser;

public sealed class Network
{
    public static Network Default => Mainnet;

    public static readonly Network Mainnet = new()
    {
        Magic = [0xf9, 0xbe, 0xb4, 0xd9],
        AddressPKH = 0,
        AddressSH = 5,
        Bench32hrp = [98, 99],
    };

    public static readonly Network Testnet3 = new()
    {
        Magic = [0x0b, 0x11, 0x09, 0x07],
        AddressPKH = 111,
        AddressSH = 196,
        Bench32hrp = [116, 98],
    };

    public static readonly Network Regtest = new()
    {
        Magic = [0xfa, 0xbf, 0xb5, 0xda],
        AddressPKH = 111,
        AddressSH = 196,
        Bench32hrp = [98, 99, 114, 116],
    };


    internal byte[] Magic;
    internal byte AddressPKH;
    internal byte AddressSH;
    internal byte[] Bench32hrp;

    private Bech32mEncoder _bench32;
    internal Bech32mEncoder Bench32 => _bench32 ??= new(Bench32hrp);


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    internal Network() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
