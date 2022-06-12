using UnityEngine.UI;
using UnityEngine;

public class Tile: MonoBehaviour
{
    [SerializeField] private Text _valueText;
    [SerializeField] private int _value;
    public int Value { get { return _value; } }

    [SerializeField] private Image _backImage;

    private TileConfig _config;

    public void SetConfig(TileConfig config)
    {
        _config = config;
    }
    public void SetValue(int value)
    {
        _value = value;

        _valueText.text = value == 0 ? "" : _value.ToString();

        SetColor(_value);
    }

    private void SetColor(int value)
    {
        var color = _config.GetColor(value);
        _backImage.color = color;
    }
}