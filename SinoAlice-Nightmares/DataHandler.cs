using Newtonsoft.Json;
using SinoAlice_Nightmares.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SinoAlice_Nightmares
{
  public static class DataHandler
  {
    const string SettingsPath = "settings.json";

    const string UrlNightmareInfo = "https://sinoalice.game-db.tw/package/alice_nightmares_global-en.js";

    public static void SaveNightmareInfoToFile(List<NightmareInfo> nightmareInfo, string path)
    {
      var nightmareBase = nightmareInfo.Select(x => new NightmareBase()
      {
        Name = x.Name,
        Id = x.Id,
        UniqueId = x.UniqueId,
        IsGlobal = x.IsGlobal
      }).ToList();
      JsonSerializer serializer = new JsonSerializer();
      serializer.Formatting = Formatting.Indented;
      using (StreamWriter sw = new StreamWriter(path))
      using (JsonWriter writer = new JsonTextWriter(sw))
      {
        serializer.Serialize(writer, nightmareBase);
      }
    }

    public static List<NightmareBase> ReadNightmareInfosFromFile(string path)
    {
      var jsonObject = File.ReadAllText(path);
      return JsonConvert.DeserializeObject<List<NightmareBase>>(jsonObject);
    }

    public static List<NightmareInfo> FetchNightmareInfos(List<NightmareBase> nightmareBases)
    {
      var temp = FetchNightmareInfos();
      return nightmareBases.Select(x => temp.Where(y => y.Id == x.Id).FirstOrDefault()).ToList();
    }

    public static List<NightmareInfo> FetchNightmareInfos()
    {
      using (var client = new WebClient())
      {
        client.DownloadFile(UrlNightmareInfo, "temp.js");
      }

      // Get row info
      var jsonObject = File.ReadAllText("temp.js");
      string rows = JsonConvert.DeserializeObject<dynamic>(jsonObject)["Rows"].ToString();
      var listNightmare = JsonConvert.DeserializeObject<List<string>>(rows).Select(x => x.Split('|'));

      // Get the columns position
      string cols = JsonConvert.DeserializeObject<dynamic>(jsonObject)["Cols"].ToString();
      var columns = cols.Split('|').ToList();

      var nameIndex = columns.IndexOf("NameEN");
      var uniqueIdIndex = columns.IndexOf("UniqueID");
      var idIndex = columns.IndexOf("ID");
      var gvgSkillLeadIndex = columns.IndexOf("GvgSkillLead");
      var gvgSkillDurIndex = columns.IndexOf("GvgSkillDur");
      var rarityIndex = columns.IndexOf("Rarity");
      var globalIndex = columns.IndexOf("Global");
      var iconIndex = columns.IndexOf("Icon");

      // Parse row info
      var nightmareInfos = listNightmare.Select(x => new NightmareInfo()
      {
        Id = x[idIndex],
        UniqueId = x[uniqueIdIndex],
        Name = x[nameIndex],
        Icon = x[iconIndex],
        Summoning_time = Int32.Parse(x[gvgSkillLeadIndex]),
        Summoned_time = Int32.Parse(x[gvgSkillDurIndex]),
        Rarity = Int32.Parse(x[rarityIndex]),
        IsGlobal = x[globalIndex] == "1"
      }).Where(x => x.Rarity == 6 && x.IsGlobal);

      // Cleanup
      File.Delete("temp.js");
      return nightmareInfos.ToList();
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
