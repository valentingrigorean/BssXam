using System.Drawing;
using System.Globalization;

namespace Bss.XamCore.Extensions
{
    public static class ColorExtensions
    {
        public static Color ParseColor(this string hex)
        {
            return Color.FromArgb(int.Parse(hex.Replace("#", ""), NumberStyles.HexNumber));
        }
    }
}
