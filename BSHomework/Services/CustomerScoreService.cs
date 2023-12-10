using System.Net;
using BSHomework.Exceptions;
using BSHomework.Models;

namespace BSHomework.Services;

public class CustomerScoreService
{
    private static readonly SortedList<CustomerScore, CustomerScore> SortByRank = new();
    private static readonly Dictionary<int, CustomerScore> MapByCustomerId = new();
    private static ReaderWriterLockSlim Lock = new(LockRecursionPolicy.SupportsRecursion);

    public decimal addScore(int customerId, decimal score)
    {
        Lock.EnterWriteLock();
        
        CustomerScore customerScore;
        if (!MapByCustomerId.TryGetValue(customerId, out customerScore))
        {
            customerScore = new CustomerScore(customerId, score);
            MapByCustomerId[customerId] = customerScore;
        }
        else
        {
            SortByRank.Remove(customerScore);
            customerScore.Score += score;
        }

        SortByRank.Add(customerScore, customerScore);

        var newScore = customerScore.Score;
        
        Lock.ExitWriteLock();
        
        return newScore;
    }

    public List<CustomerScoreRanked> getCustomersByRank(int rankStartIncluded, int rankEndIncluded)
    {
        if (SortByRank.Count < rankEndIncluded)
        {
            throw new HttpResponseException(400,
                "The end parameter out of range, not enough data in leaderboard.");
        }

        Lock.EnterReadLock();

        var sortedScores = SortByRank.Keys;
        var result = Enumerable.Range(rankStartIncluded, rankEndIncluded - rankStartIncluded + 1)
            .ToList()
            .Select(rank =>
            {
                var customerScore = sortedScores.ElementAt(rank - 1);
                return new CustomerScoreRanked(customerScore, rank);
            })
            .ToList();

        Lock.ExitReadLock();

        return result;
    }

    public List<CustomerScoreRanked> getCustomersByCustomerId(int customerId, int higherRankMemberCount,
        int lowerRankMemberCount)
    {
        CustomerScore customerScore;
        if (!MapByCustomerId.TryGetValue(customerId, out customerScore))
        {
            throw new HttpResponseException(404, "No such customer with id=" + customerId);
        }

        Lock.EnterReadLock();

        var rank = SortByRank.Keys.IndexOf(customerScore) + 1;
        var rankFrom = rank - higherRankMemberCount;
        var rankTo = rank + lowerRankMemberCount;
        if (rankFrom <= 0)
        {
            rankFrom = 1;
        }

        if (rankTo > SortByRank.Count)
        {
            rankTo = SortByRank.Count;
        }

        var result = getCustomersByRank(rankFrom, rankTo);

        Lock.ExitReadLock();

        return result;
    }
}