using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BlockchainApi.Models;

namespace BlockchainApi;

public static class BlockCandidateHasher
{
    public static string HashBlockCandidate(this BlockCandidate block)
    {
        var blockCandidateJson = JsonSerializer.Serialize(block);

        using var sha256Hash = SHA256.Create();

        var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(blockCandidateJson));

        var builder = new StringBuilder();
        foreach (var t in bytes) builder.Append(t.ToString("x2"));

        return builder.ToString();
    }
}