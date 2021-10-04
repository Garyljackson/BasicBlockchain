using BlockchainApi.Models;

namespace BlockchainApi.Contracts;

public interface IBlockchain
{
    AppendBlockResult AppendBlock(Block block);
    IReadOnlyCollection<Block> GetChain();
    Block GetChainHead();
    ICollection<BlockValidationResult> ValidateChain();
}