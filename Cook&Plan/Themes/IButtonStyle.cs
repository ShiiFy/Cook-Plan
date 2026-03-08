using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Themes
{
    public interface IButtonStyle
    {
        string Background { get; } 
        string Foreground { get; }   // цвет текста
        string BorderColor { get; }
    }
}
