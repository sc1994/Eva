namespace Eva.HttpAggregator.ServiceInterfaces;

public interface IDemandsService
{
    Task<string> CreateAsync(object request);
}