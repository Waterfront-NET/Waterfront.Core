# Waterfront

Open source docker authentication server

## Details

### Notes on the implementation design

*These notes describe the flow which is followed by
the reference implementation in packages like [Waterfront.AspNetCore](#) and applications like [Waterfront](#), however,
by combining implementations provided in [Waterfront.Net](#)
repositories with your own source code, you can implement the
behaviour you want to achieve*

Bearer token request processing consists of the two main steps:

1. Authentication
2. Authorization

Which are accompanied with supporting steps like

- Request parsing
- Choosing the signing algorithm
- Creating JWT payload
- Signing the JWT
- Sending back the response
- other

> `Authentication` step performs the authentication with all
> compatible `AuthenticationSchemes` (using `service` and `client_id` fields of the request to determine those who are compatible) *in sequence*,
> until a successful result is found
>
> That basically means that even if multiple `AuthenticationSchemes` *would* succeed in authenticating the user,
> only the user from first successful result will be then used
> to collect names of the `AclPolicies` to use while performing the `Authorization` step

> `Authorization` step works different: given a collection of `AclPolicy` names, it goes through all the available authorization schemes, invoking them, until the request is fully authorized,
> combining every subsequent `AuthorizationResult` with previous,
> favoring only the authorized scopes rather then forbidden ones
>
> For example:
> We have a user with policies defined as such: `[puller, mysql-pusher]`
> And we have authorization schemes `(A,B)`:
>
> 1. Scheme `A` allows `puller` to pull from any repository, and `mysql-pusher` to push to `mysql/dev`
> 2. Scheme `B` allows `mysql-pusher` to push to `mysql/*`
>
> The resulting permissions for user will allow him to pull from any repository as well as to push to any repository, starting with `mysql/`
