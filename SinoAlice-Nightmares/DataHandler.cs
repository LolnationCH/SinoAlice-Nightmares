using Newtonsoft.Json;
using SinoAlice_Nightmares.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinoAlice_Nightmares
{
  public static class DataHandler
  {

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
  }
}
