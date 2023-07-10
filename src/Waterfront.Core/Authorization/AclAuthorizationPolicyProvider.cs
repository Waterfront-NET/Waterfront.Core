using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Waterfront.Common.Authorization;
using Waterfront.Common.Tokens.Requests;

namespace Waterfront.Core.Authorization;

public class AclAuthorizationPolicyProvider : IAclAuthorizationPolicyProvider
{
    private readonly Dictionary<string, AclAuthorizationPolicy> _policyMap;

    public IEnumerable<AclAuthorizationPolicy> Policies => PolicyMap.Values;
    public IReadOnlyDictionary<string, AclAuthorizationPolicy> PolicyMap => _policyMap;

    protected ILogger<AclAuthorizationPolicyProvider> Logger { get; }

    public AclAuthorizationPolicyProvider(ILogger<AclAuthorizationPolicyProvider> logger)
    {
        Logger = logger;
        _policyMap = new Dictionary<string, AclAuthorizationPolicy>();
    }

    public Task<IEnumerable<AclAuthorizationPolicy>> GetPoliciesAsync(TokenRequest request)
    {
        return Task.FromResult(Policies);
    }

    public void AddPolicy(AclAuthorizationPolicy policy)
    {
        _policyMap[policy.Name] = policy;
    }

    public void AddPolicy(Action<AclAuthorizationPolicyBuilder> buildPolicy)
    {
        var builder = AclAuthorizationPolicyBuilder.Create();
        buildPolicy(builder);
        AddPolicy(builder.Build());
    }

    public bool HasPolicy(string name)
    {
        return _policyMap.ContainsKey(name);
    }
}
