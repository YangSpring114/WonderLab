using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WonderLab.Modules.Base;

namespace WonderLab.Modules.Models
{
    public class InfoBarModel : ViewModelBase
    {
        public bool Removed { get; set; } = false;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public IControl Button { get; set; }

        public int Delay { get; set; } = 5000;

        public InfoBarSeverity Severity { get; set; } = InfoBarSeverity.Informational;
    }
}
