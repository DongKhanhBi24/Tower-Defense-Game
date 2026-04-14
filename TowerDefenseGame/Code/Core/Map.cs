using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TowerDefenseGame
{
    public class Map
    {
        // =================== Properties ===================
        public List<Vector2> EnemyPath { get; private set; }
        public List<Vector2> TowerSpots { get; private set; }
        public List<Vector2> ValidTowerSpots { get; private set; }

        // =================== Constructor ===================
        public Map()
        {
            EnemyPath = new List<Vector2>();
            TowerSpots = new List<Vector2>();

            // Predefined valid tower spots
            ValidTowerSpots = new List<Vector2>
            {
                new Vector2(220, 250),
                new Vector2(240, 490),
                new Vector2(340, 230),
                new Vector2(660, 555),
                new Vector2(720, 240),
                new Vector2(760, 610),
                new Vector2(910, 375),
                new Vector2(1100, 590)
            };
        }

        // =================== Load Enemy Path ===================
        public void LoadPathFromFile(string pathFile)
        {
            EnemyPath.Clear();

            if (!File.Exists(pathFile))
                return;

            foreach (var line in File.ReadAllLines(pathFile))
            {
                var parts = line.Split(' ');

                if (parts.Length >= 2 &&
                    float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float x) &&
                    float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float y))
                {
                    EnemyPath.Add(new Vector2(x, y));
                }
            }
        }
    }
}
