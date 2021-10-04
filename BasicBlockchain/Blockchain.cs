using BlockchainApi.Contracts;
using BlockchainApi.Models;

namespace BasicBlockchain;

public class Blockchain : IBlockchain
{
    private readonly IBlockValidator _blockValidator;
    private readonly List<Block> _chain;

    public Blockchain(IBlockValidator blockValidator)
    {
        _blockValidator = blockValidator;
        _chain = new List<Block> {CreateGenesisBlock()};
    }

    public AppendBlockResult AppendBlock(Block block)
    {
        var previousBlock = GetChainHead();
        var validationResult = _blockValidator.ValidateBlock(block, previousBlock);

        if (validationResult.IsValid) _chain.Add(block);

        return new AppendBlockResult(validationResult);
    }

    public IReadOnlyCollection<Block> GetChain()
    {
        return _chain.AsReadOnly();
    }

    public Block GetChainHead()
    {
        return _chain.Last();
    }

    public ICollection<BlockValidationResult> ValidateChain()
    {
        var validationResults = new List<BlockValidationResult>();

        var previousBlock = _chain.First();

        foreach (var block in _chain.Where(block => block.Index != 0))
        {
            var result = _blockValidator.ValidateBlock(block, previousBlock);

            if (!result.IsValid) validationResults.Add(result);

            previousBlock = block;
        }

        return validationResults;
    }

    private static Block CreateGenesisBlock()
    {
        var block = new Block(0, DateTime.MinValue, "0", "0", 0, "");
        return block;
    }
    
}