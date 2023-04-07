using System.Collections.Generic;
using System.Linq;
using Sprache;
using Waterfront.Common.Acl;
using Waterfront.Common.Tokens;

namespace Waterfront.Core.Utility.Parsing.Acl;

public static class AclEntityParser
{
    public static AclResourceType ParseResourceType(string input) =>
    Grammar.ResourceType.Parse(input);

    public static AclResourceAction ParseResourceAction(string input) =>
    Grammar.ResourceAction.Parse(input);

    public static IEnumerable<AclResourceAction> ParseResourceActionList(string input) =>
    Grammar.ResourceActionList.Parse(input);

    public static TokenRequestScope ParseTokenRequestScope(string input) =>
    Grammar.RequestScope.Parse(input);

    private static class Grammar
    {
        private const char SCOPE_PART_DELIMITER = ':';
        private const char ACTION_DELIMITER     = ',';

        private static readonly Parser<char> ScopePartDelimiter = Parse.Char(SCOPE_PART_DELIMITER);
        private static readonly Parser<char> ActionDelimiter    = Parse.Char(ACTION_DELIMITER);

        private static readonly Parser<AclResourceType> Repository = Parse.IgnoreCase("repository")
        .Return(AclResourceType.Repository);

        private static readonly Parser<AclResourceType> Registry = Parse.IgnoreCase("registry")
        .Return(AclResourceType.Registry);

        public static readonly Parser<AclResourceType> ResourceType = Repository.Or(Registry);

        private static readonly Parser<AclResourceAction> Pull = Parse.IgnoreCase("pull")
        .Return(AclResourceAction.Pull);

        private static readonly Parser<AclResourceAction> Push = Parse.IgnoreCase("push")
        .Return(AclResourceAction.Push);

        private static readonly Parser<AclResourceAction> Any =
        Parse.Char('*').Return(AclResourceAction.Any);

        public static readonly Parser<AclResourceAction> ResourceAction = Pull.Or(Push).Or(Any);

        public static readonly Parser<IEnumerable<AclResourceAction>> ResourceActionList =
        ResourceAction.DelimitedBy(ActionDelimiter);

        private static readonly Parser<string> ResourceName =
        Parse.AnyChar.Except(ScopePartDelimiter).AtLeastOnce().Text();

        // Parses ^(repository|registry):([^:]*):((pull|push|\*)(,(pull|push|\*))*)*$
        public static readonly Parser<TokenRequestScope> RequestScope = ResourceType
        .Then(type => ScopePartDelimiter.Return(type))
        .Then(type => ResourceName.Select(name => (type, name)))
        .Then(typeAndName => ScopePartDelimiter.Return(typeAndName))
        .Then(
            typeAndName => ResourceActionList.Select(
                actionList => (typeAndName.type, typeAndName.name, actionList)
            )
        )
        .Select(
            tuple => new TokenRequestScope {
                Type    = tuple.type,
                Name    = tuple.name,
                Actions = tuple.actionList.ToArray()
            }
        );
    }
}
