using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderLab.Modules.Models
{
    public class ModInfoModels
    {
        public enum ModType
        {
            Forge = 0,
            Fabric = 1,
            ForgeAndFabric = 2
        }

        public string FileName { get; set; }

        public ModType Type { get; set; } = 0;

        public string Name { get; set; }

        public string Description { get; set; }

        public string[] Authors { get; set; }

        public string Version { get; set; }

        public bool Enable { get; set; }
    }
}
