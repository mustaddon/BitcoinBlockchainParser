﻿namespace BitcoinBlockchainParser.DataTypes;

public readonly struct TxoScript(byte[] bytes) : ITxScript
{
    public byte[] Bytes => bytes;

    public TxoScriptType Type => bytes.GetScriptType();

    public override string ToString() => TxScriptReader.ReadToString(bytes);
}
