using System.Collections.Generic;
using Windows.UI;

namespace ColorfulCanvas
{
    public class Preset
    {
        public List<Color> Source { get; set; }
        public string Name { get; internal set; }
    }
}