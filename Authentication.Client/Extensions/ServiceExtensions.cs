using OpenIddict.Client;

namespace Authentication.Client.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigAuthentication(this IServiceCollection services)
    {
        services.AddOpenIddict()

            // Register the OpenIddict client components.
            .AddClient(options =>
            {
                // Allow grant_type=password to be negotiated.
                options.AllowPasswordFlow();

                // Disable token storage, which is not necessary for non-interactive flows like
                // grant_type=password, grant_type=client_credentials or grant_type=refresh_token.
                options.DisableTokenStorage();

                // Register the System.Net.Http integration and use the identity of the current
                // assembly as a more specific user agent, which can be useful when dealing with
                // providers that use the user agent as a way to throttle requests (e.g Reddit).
                options.UseSystemNetHttp()
                    .SetProductInformation(typeof(Program).Assembly);

                // Add a client registration without a client identifier/secret attached.
                options.AddRegistration(new OpenIddictClientRegistration
                {
                    Issuer = new Uri("https://localhost:5057/", UriKind.Absolute)
                });
            });

        return services;
    }
}