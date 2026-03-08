using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Themes
{
    public interface IWindowStyle
    {
        string Background { get; } 
        string Foreground { get; } // цвет текста
    }
}
