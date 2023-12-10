namespace BSHomework.Models;

public class CustomerScoreRanked
{
    public int CustomerId { get; set; }
    public decimal Score { get; set; }
    public int rank { get; set; }

    public CustomerScoreRanked(CustomerScore src, int rank)
    {
        this.CustomerId = src.CustomerId;
        this.Score = src.Score;
        this.rank = rank;
    }
}