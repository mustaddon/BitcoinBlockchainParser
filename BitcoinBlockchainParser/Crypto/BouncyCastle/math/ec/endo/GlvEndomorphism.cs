﻿namespace BitcoinBlockchainParser.BouncyCastle.Math.EC.Endo
{
	internal interface GlvEndomorphism
		: ECEndomorphism
	{
		BigInteger[] DecomposeScalar(BigInteger k);
	}
}
