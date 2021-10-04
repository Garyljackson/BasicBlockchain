using BlockchainApi.Models;

namespace BlockchainApi.Contracts;

public interface IBlockValidator
{
    BlockValidationResult ValidateBlock(Block currentBlock, Block previousBlock);
}