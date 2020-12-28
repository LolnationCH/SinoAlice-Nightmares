using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinoAlice_Nightmares.Objects
{
  public class NightmareInfo : NightmareBase
  {
    private int summoning_time;
    private int summoned_time;
    private int rarity;
    private string icon;

    public string ImagePath { get => $"Images\\CardS{(icon.Length < 4 ? "0" + icon : icon)}.png"; }
    public string ImageUrl { get => $"https://sinoalice.game-db.tw/images/card/CardS{(icon.Length < 4 ? "0" + icon : icon)}.png"; }
    public int Summoning_time { get => summoning_time; set => summoning_time = value; }
    public int Summoned_time { get => summoned_time; set => summoned_time = value; }
    public int Rarity { get => rarity; set => rarity = value; }
    public string Icon { get => icon; set => icon = value; }
  }
}
