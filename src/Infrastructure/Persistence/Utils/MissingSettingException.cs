namespace Infrastructure.Persistence.Utils;

public class MissingSettingException : Exception
{
    public MissingSettingException(string settingName) :
        base($"Setting {settingName} is missing in appsettings.json") { }
}