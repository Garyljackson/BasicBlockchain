using System.Net.Http.Json;
using BlockchainApi.Models;

namespace BlockchainMiner;

internal class BlockSubmitter
{
    private readonly HttpClient _httpClient;

    internal BlockSubmitter()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7068/");
    }

    internal async Task SubmitBlock(Block block)
    {
        Console.WriteLine($"Mining Block: {block.Index}");

        using var response = await _httpClient.PostAsJsonAsync("api/Blockchain", block);

        try
        {
            var appendBlockResult = await response.Content.ReadFromJsonAsync<AppendBlockResult>();

            if (appendBlockResult != null)
            {
                if (response.IsSuccessStatusCode)
                    WriteSuccess(appendBlockResult);
                else
                    WriteError(appendBlockResult);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static void WriteSuccess(AppendBlockResult appendBlockResult)
    {
        var previousColour = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Block Appended Successfully");
        Console.WriteLine($"Block Id:{appendBlockResult.BlockValidationResult.Block.Index}");
        Console.ForegroundColor = previousColour;
        Console.WriteLine();
    }

    private static void WriteError(AppendBlockResult appendBlockResult)
    {
        var previousColour = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(
            $"Block Append Failed for block with index: {appendBlockResult.BlockValidationResult.Block.Index}");

        foreach (var validationError in appendBlockResult.BlockValidationResult.ValidationErrors)
            Console.WriteLine(validationError);

        Console.ForegroundColor = previousColour;
        Console.WriteLine();
    }
}