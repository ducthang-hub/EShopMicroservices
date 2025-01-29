namespace BuildingBlocks.HttpClient.Extension;

public class ServiceOptions
{
    public const string OptionName = "Services";
    public DiscountService DiscountService { get; set; }
    public CatalogService CatalogService { get; set; }
    public AuthenticationService AuthenticationService { get; set; }
}

public class ServiceConfig
{
    public string Name { get; set; }
    public string BaseAddress { get; set; }
}

public class DiscountService : ServiceConfig
{
}

public class CatalogService : ServiceConfig
{
}

public class AuthenticationService : ServiceConfig
{
}