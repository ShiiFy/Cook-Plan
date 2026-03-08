using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Themes.Light
{
    public class LightThemeFactory : IThemeFactory
    {
        public IButtonStyle CreateButtonStyle() => new LightButtonStyle();
        public IWindowStyle CreateWindowStyle() => new LightWindowStyle();
    }
}
