using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TilesConfig", menuName = "Tiles/New Tiles Config")]
public class TileConfig : ScriptableObject
{
    [Serializable]
    private class TileColorConfig
    {
        [SerializeField] private int _number;
        [SerializeField] private Color _color;

        public int Number => _number;
        public Color Color => _color;
    }

    [SerializeField] private List<TileColorConfig> _tilesConfigs;

    public Color GetColor(int value)
    {
        if(_tilesConfigs == null)
        {
            Debug.LogWarning("Config == null");
            return Color.white;
        }
        return _tilesConfigs.FirstOrDefault(c => c.Number == value).Color;
    }
}
 