using System.Security.Cryptography;
using BSHomework.Exceptions;
using BSHomework.Models;
using BSHomework.Services;
using Microsoft.AspNetCore.Mvc;

namespace BSHomework.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeworkController : ControllerBase
{
    private static CustomerScoreService customerScoreService = new();

    [HttpPost, Route("/customer/{customerId:int}/score/{score:decimal}")]
    public IActionResult addScore([FromRoute] int customerId, decimal score)
    {
        if (customerId <= 0)
        {
            throw new HttpResponseException(400, "The 'customerId' parameter must be > 0.");
        }

        decimal newScore = customerScoreService.addScore(customerId, score);
        return Ok(new CustomerScore(customerId, newScore));
    }

    [HttpGet, Route("/leaderboard")]
    public IActionResult getCustomersByRank([FromQuery] int start, int end)
    {
        if (start < 1)
        {
            throw new HttpResponseException(400, "The 'start' parameter must be >= 1.");
        }

        if (end < 1)
        {
            throw new HttpResponseException(400, "The 'end' parameter must be >= 1.");
        }

        if (start > end)
        {
            throw new HttpResponseException(400, "Parameter 'end' must greater then Parameter 'start'");
        }

        return Ok(customerScoreService.getCustomersByRank(start, end));
    }

    [HttpGet, Route("/leaderboard/{customerId}")]
    public IActionResult getCustomersByCustomerId([FromRoute] int customerId, [FromQuery] int high = 0, int low = 0)
    {
        if (high < 0)
        {
            throw new HttpResponseException(400, "The high parameter must be >= 0.");
        }

        if (low < 0)
        {
            throw new HttpResponseException(400, "The low parameter must be >= 0.");
        }

        return Ok(customerScoreService.getCustomersByCustomerId(customerId, high, low));
    }
}