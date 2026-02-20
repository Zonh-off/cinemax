namespace API.Utils;

public static class RatingDistributionHelper
{
    public static Dictionary<int, int> GenerateFakeDistribution(double avg, int total)
    {
        var dist = new Dictionary<int, int>(10);
        for (int i = 1; i <= 10; i++)
        {
            double weight = Math.Exp(-Math.Pow(i - avg, 2) / 4);
            dist.Add(i, (int)(total * weight / 5));
        }

        return dist;
    }
}