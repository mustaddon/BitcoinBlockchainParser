namespace BitcoinBlockchainParser.DataTypes;

public readonly struct FilePosition(string name, int number, long position) : IEquatable<FilePosition>
{
    public string File => name;
    public string FileName => Path.GetFileName(name);
    public int Number => number;
    public long Position => position;

    public override string ToString() => $"File: {Number}, Position: {Position}";
    public override int GetHashCode() => HashCode.Combine(Number, Position);
    public override bool Equals(object? obj) => obj is FilePosition o && Equals(o);
    public bool Equals(FilePosition other) => other.Number == Number && other.Position == Position;
}
