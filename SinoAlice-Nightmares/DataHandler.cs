using Newtonsoft.Json;
using SinoAlice_Nightmares.Objects;
using System.Collections.Generic;
using System.IO;

namespace SinoAlice_Nightmares
{
  public static class DataHandler
  {
    const string SettingsPath = "settings.json";

    public static void SaveNightmareInfoToFile(List<NightmareInfo> nightmareInfos, string path)
    {
      JsonSerializer serializer = new JsonSerializer();
      serializer.Formatting = Formatting.Indented;
      using (StreamWriter sw = new StreamWriter(path))
      using (JsonWriter writer = new JsonTextWriter(sw))
      {
        serializer.Serialize(writer, nightmareInfos);
      }
    }

    public static List<NightmareInfo> ReadNightmareInfosFromFile(string path)
    {
      var jsonObject = File.ReadAllText(path);
      return JsonConvert.DeserializeObject<List<NightmareInfo>>(jsonObject);
    }

    public static Settings GetSettings()
    {
      if (!File.Exists(SettingsPath)) return new Settings();
      return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsPath));
    }

    public static void SetSettings(Settings settings)
    {
      JsonSerializer serializer = new JsonSerializer();
      serializer.Formatting = Formatting.Indented;
      using (StreamWriter sw = new StreamWriter(SettingsPath))
      using (JsonWriter writer = new JsonTextWriter(sw))
      {
        serializer.Serialize(writer, settings);
      }
    }
  }
}
