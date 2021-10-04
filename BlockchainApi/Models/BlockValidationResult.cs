namespace BlockchainApi.Models;

public record BlockValidationResult(Block Block, bool IsValid, ICollection<string> ValidationErrors);