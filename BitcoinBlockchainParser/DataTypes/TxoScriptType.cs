namespace BitcoinBlockchainParser.DataTypes;

public enum TxoScriptType
{
    Unknown = 0,
    P2PK = 1,
    P2PKH = 2,
    P2MS = 3,
    P2SH = 4,
    P2WPKH = 5,
    P2WSH = 6,
    P2TR = 7,
    OP_RETURN = 8,
}
