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
        
        public readonly List<Color> ColorGradient = new()
        {
            new Color(231, 103,145), //magenta pink
            new Color(255, 89,57), //red
            new Color(255, 205, 97), //yellow
            new Color(237, 153, 92), //orange
            new Color(134, 212, 102), //green
            new Color(78, 210, 165), //teal
            new Color(105, 189, 233), //blue
            new Color(108, 146, 220), //dark blue
            new Color(209, 145, 207), // dark pink
            new Color(104, 66, 140) //dark purple
        };
    }
}
