using UnityEngine.UI;
using UnityEngine;

public class Tile: MonoBehaviour
{
    [SerializeField] private Text _valueText;
    [SerializeField] private int _value;
    public int Value { get { return _value; } }

    [SerializeField] private Image _backImage;

    [SerializeField] private Color _emptyColor;
    [SerializeField] private Color _number2Color;
    [SerializeField] private Color _number4Color;
    [SerializeField] private Color _number8Color;
    [SerializeField] private Color _number16Color;
    [SerializeField] private Color _number32Color;
    [SerializeField] private Color _number64Color;
    [SerializeField] private Color _number128Color;
    [SerializeField] private Color _number256Color;
    [SerializeField] private Color _number512Color;
    [SerializeField] private Color _number1024Color;
    [SerializeField] private Color _number2048Color;

    public void SetValue(int value)
    {
        _value = value;

        _valueText.text = value == 0 ? "" : _value.ToString();

        SetColor(value);
    }

    private void SetColor(int value)
    {
        switch (value)
        {
            case 2:
                _backImage.color = _number2Color;
                break;
            case 4:
                _backImage.color = _number4Color;
                break;
            case 8:
                _backImage.color = _number8Color;
                break;
            case 16:
                _backImage.color = _number16Color;
                break;
            case 32:
                _backImage.color = _number32Color;
                break;
            case 64:
                _backImage.color = _number64Color;
                break;
            case 128:
                _backImage.color = _number128Color;
                break;
            case 256:
                _backImage.color = _number256Color;
                break;
            case 512:
                _backImage.color = _number512Color;
                break;
            case 1024:
                _backImage.color = _number1024Color;
                break;
            case 2048:
                _backImage.color = _number2048Color;
                break;

            default:
                _backImage.color = _emptyColor;
                break;
        }
    }
}