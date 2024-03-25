using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class DataConstants
    {
        private static DataConstants _instance;
        public static DataConstants Instance => _instance ??= new DataConstants();
        
        public readonly int DefaultDotSpawnValue = 2;
        public readonly List<int> DotStartingValues = new() {2, 4, 8};
        public readonly int GridSize = 5;
        
        public readonly List<Color> ColorGradient = new List<Color>
        {
            new Color(0.905882f, 0.403922f, 0.568627f, 1f), //magenta pink
            new Color(1f, 0.34902f, 0.223529f, 1f), //red
            new Color(0.9921569f, 0.7333333f, 0.1882353f, 1f), //yellow
            new Color(0.929412f, 0.6f, 0.360784f, 1f), //orange
            new Color(0.52549f, 0.831373f, 0.4f, 1f), //green
            new Color(0.305882f, 0.823529f, 0.647059f, 1f), //teal
            new Color(0.411765f, 0.741176f, 0.913725f, 1f), //blue
            new Color(0.423529f, 0.572549f, 0.862745f, 1f), //dark blue
            new Color(0.819608f, 0.568627f, 0.811765f, 1f), // dark pink
            new Color(0.407843f, 0.258824f, 0.54902f, 1f) //purple
        };
    }
}
