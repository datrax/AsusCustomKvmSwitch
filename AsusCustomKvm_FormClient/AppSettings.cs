using System.Text.Json;


namespace AsusCustomKvm_FormClient;
public class AppSettings
{
    public Device HubDevice { get; set; }
    public uint OnConnectedVPCCode { get; set; } = 0x00;
    public uint OnDisconnectedVPCCode { get; set; } = 0x00;

    private static string configFilePath = Path.Combine(AppContext.BaseDirectory, "config.json");

    public void SaveToFile()
    {
        File.WriteAllText(configFilePath, JsonSerializer.Serialize(this));
    }

    public static AppSettings ReadFromFile()
    {
        if (!File.Exists(configFilePath))
        {
            var defaultAppSettings = new AppSettings();
            File.WriteAllText(configFilePath, JsonSerializer.Serialize(defaultAppSettings));
        }
        return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(configFilePath))!;
    }
}