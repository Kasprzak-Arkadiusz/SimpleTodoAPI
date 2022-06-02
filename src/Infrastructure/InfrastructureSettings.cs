using Infrastructure.Persistence.Utils;

namespace Infrastructure;

public class InfrastructureSettings
{
    public string DbConnectionString { get; set; }

    public bool SeedWithCustomData { get; set; }
}