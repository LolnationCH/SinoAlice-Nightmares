using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinoAlice_Nightmares.Objects
{
  public class NightmareInfo
  {
    private string image_path;
    private string name;
    private int summoning_time;
    private int summoned_time;

    public string Image_path { get => image_path; set => image_path = value; }
    public string Name { get => name; set => name = value; }
    public int Summoning_time { get => summoning_time; set => summoning_time = value; }
    public int Summoned_time { get => summoned_time; set => summoned_time = value; }
  }
}
