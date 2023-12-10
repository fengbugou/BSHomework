namespace BSHomework.Models;

public class CustomerScore : IComparable<CustomerScore>, IEquatable<CustomerScore>
{
    public int CustomerId { get; set; }
    public  decimal Score { get; set; }

    public CustomerScore(int customerId, decimal score)
    {
        CustomerId = customerId;
        Score = score;
    }

    public int CompareTo(CustomerScore? other)
    {
        if (other == null)
        {
            return 1;
        }

        var result = other.Score.CompareTo(Score);
        if (result != 0)
        {
            return result;
        }

        return CustomerId.CompareTo(other.CustomerId);
    }

    public bool Equals(CustomerScore? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return CustomerId == other.CustomerId && Score == other.Score;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CustomerScore)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CustomerId, Score);
    }
}