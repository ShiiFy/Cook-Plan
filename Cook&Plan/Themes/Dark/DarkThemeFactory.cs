using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Themes.Dark
{
    public class DarkThemeFactory : IThemeFactory
    {
        public IButtonStyle CreateButtonStyle() => new DarkButtonStyle();
        public IWindowStyle CreateWindowStyle() => new DarkWindowStyle();
    }
}
