using System.Net.Http.Json;
using BlockchainApi;
using BlockchainApi.Models;

namespace BlockchainMiner;

internal class BlockMiner
{
    private readonly HttpClient _httpClient;

    internal BlockMiner()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7068/");
    }

    internal async Task<Block> MineBlockAsync(string data)
    {
        var blockMined = false;
        var minedBlockHash = string.Empty;
        uint currentNonce = 0;

        var chainHead = await _httpClient.GetFromJsonAsync<Block>("api/Blockchain/Head");

        var blockCandidate =
            new BlockCandidate(chainHead!.Index + 1, DateTime.UtcNow, chainHead.Hash, currentNonce, data);

        while (!blockMined)
        {
            blockCandidate = blockCandidate with {Proof = currentNonce};
            minedBlockHash = blockCandidate.HashBlockCandidate();

            if (minedBlockHash.StartsWith(BlockchainProperties.BlockDifficulty)) blockMined = true;

            currentNonce += 1;
        }

        return new Block(blockCandidate.Index, blockCandidate.Timestamp, minedBlockHash, blockCandidate.PreviousHash,
            blockCandidate.Proof, blockCandidate.Data);
    }
}