namespace BlockchainApi.Models;

public record BlockCandidate(uint Index, DateTime Timestamp, string PreviousHash, uint Proof, string Data);