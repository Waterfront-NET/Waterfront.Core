namespace Waterfront.Common.Configuration;

public interface IServiceOptionsProvider<out T> where T : class, new()
{
    T Get(string service);
}
