using BlockchainApi.Contracts;
using BlockchainApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlockchainHost.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlockchainController : ControllerBase
{
    private readonly IBlockchain _blockchain;

    public BlockchainController(IBlockchain blockchain)
    {
        _blockchain = blockchain;
    }


    [HttpPost]
    public ActionResult AppendBlock(Block block)
    {
        var result = _blockchain.AppendBlock(block);

        if (!result.BlockValidationResult.IsValid) return BadRequest(result);

        return Ok(result);
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<Block>> GetChain()
    {
        return Ok(_blockchain.GetChain());
    }

    [HttpGet]
    [Route("Head")]
    public ActionResult<Block> GetChainHead()
    {
        return Ok(_blockchain.GetChainHead());
    }

    [HttpGet]
    [Route("Validate")]
    public ActionResult<ICollection<BlockValidationResult>> ValidateChain()
    {
        return Ok(_blockchain.ValidateChain());
    }
}