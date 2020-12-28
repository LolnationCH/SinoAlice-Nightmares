using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinoAlice_Nightmares.Objects
{
  public class NightmareBase
  {
    private string id;
    private string uniqueId;
    private string name;
    private bool isGlobal;

    public string Name { get => name; set => name = value; }
    public string Id { get => id; set => id = value; }
    public string UniqueId { get => uniqueId; set => uniqueId = value; }
    public bool IsGlobal { get => isGlobal; set => isGlobal = value; }
  }
}
