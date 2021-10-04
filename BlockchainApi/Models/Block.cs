namespace BlockchainApi.Models;

public record Block(uint Index, DateTime Timestamp, string Hash, string PreviousHash, uint Proof, string Data);