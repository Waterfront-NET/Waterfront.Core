using Microsoft.Extensions.DependencyInjection;

namespace Waterfront.Core.Extensions.DependencyInjection;

public interface IWaterfrontBuilder
{
    IServiceCollection Services { get; }
}
