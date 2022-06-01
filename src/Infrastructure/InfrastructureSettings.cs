using Infrastructure.Persistence.Utils;

namespace Infrastructure;

public class InfrastructureSettings
{
    private string? _dbConnectionString;

    public string DbConnectionString
    {
        get
        {
            if (string.IsNullOrEmpty(_dbConnectionString))
            {
                throw new MissingSettingException(nameof(DbConnectionString));
            }

            return _dbConnectionString;
        }
        set => _dbConnectionString = value;
    }
}