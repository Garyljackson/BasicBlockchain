using BlockchainApi.Contracts;
using BlockchainApi.Models;

namespace BlockchainApi;

public class BlockValidator : IBlockValidator
{
    public BlockValidationResult ValidateBlock(Block currentBlock, Block previousBlock)
    {
        var blockValidationResult = new BlockValidationResult(currentBlock, false, new List<string>());

        if (!IsPreviousHashLinkValid(currentBlock, previousBlock, out var previousHashErrorMessage))
            blockValidationResult.ValidationErrors.Add(previousHashErrorMessage);

        if (!IsBlockDifficultyValid(currentBlock, out var blockDifficultyErrorMessage))
            blockValidationResult.ValidationErrors.Add(blockDifficultyErrorMessage);

        if (!IsBlockIndexValid(currentBlock, previousBlock, out var blockIndexErrorMessage))
            blockValidationResult.ValidationErrors.Add(blockIndexErrorMessage);

        if (!IsBlockProofValid(currentBlock, out var blockProofValidErrorMessage))
            blockValidationResult.ValidationErrors.Add(blockProofValidErrorMessage);


        if (!blockValidationResult.ValidationErrors.Any())
            blockValidationResult = blockValidationResult with {IsValid = true};

        return blockValidationResult;
    }

    private static bool IsPreviousHashLinkValid(Block currentBlock, Block previousBlock, out string errorMessage)
    {
        if (currentBlock.PreviousHash != previousBlock.Hash)
        {
            errorMessage = "Previous hash value for current block doesn't match the hash of the last block";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsBlockDifficultyValid(Block currentBlock, out string errorMessage)
    {
        if (!currentBlock.Hash.StartsWith(BlockchainProperties.BlockDifficulty))
        {
            errorMessage = "Block hash does not satisfy difficulty requirement";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsBlockProofValid(Block block, out string errorMessage)
    {
        var blockCandidate =
            new BlockCandidate(block.Index, block.Timestamp, block.PreviousHash, block.Proof, block.Data);

        var hash = blockCandidate.HashBlockCandidate();

        if (hash != block.Hash)
        {
            errorMessage = "Block hash is invalid";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsBlockIndexValid(Block currentBlock, Block previousBlock, out string errorMessage)
    {
        if (currentBlock.Index != previousBlock.Index + 1)
        {
            errorMessage = "The block index must be equal to the previous block index + 1";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}