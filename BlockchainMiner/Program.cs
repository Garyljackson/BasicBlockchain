using BlockchainApi.Models;
using BlockchainMiner;

var blockMiner = new BlockMiner();
var blockSubmitter = new BlockSubmitter();

for (var i = 1; i <= 10; i++)
{
    var minedBlock = await blockMiner.MineBlockAsync($"Hello blockchain {i}");
    await blockSubmitter.SubmitBlock(minedBlock);
}

// Try submit a dodgy block
await blockSubmitter.SubmitBlock(new Block(100, new DateTime(2000, 2, 3), "Previous", "BadHash", 100, "No you don't!"));

Console.WriteLine("Mining Completed");


Console.ReadKey();