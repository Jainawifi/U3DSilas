using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 直接赋给滑动条  彩色滑动条效果
/// </summary>
public class ColorFade : MonoBehaviour
{
    private Color[] colors = new Color[]{
        Color.red,
        Color.yellow,
        Color.green,
        Color.cyan,
        Color.blue,
        Color.magenta,
        Color.red};

    private Slider slider_color;
    private Image img_handle;

    private void Awake()
    {
        InitializeSlider();
        InitializeHandle();
        InitializeBackground();
    }

    private void InitializeSlider()
    {
        slider_color = transform.GetComponent<Slider>();
        slider_color.onValueChanged.AddListener(OnColorSliderValueChanged);
        slider_color.fillRect.GetComponent<Image>().enabled = false;
    }

    private void InitializeHandle()
    {
        img_handle = slider_color.handleRect.GetComponent<Image>();
        img_handle.color = Color.red;
    }

    private void InitializeBackground()
    {
        var hueTex = CreateHueTexture(colors);
        slider_color.transform.Find("Background").GetComponent<Image>().sprite =
            Sprite.Create(hueTex, new Rect(Vector2.zero, new Vector2(colors.Length, 1)), Vector2.one * 0.5f);
    }

    private Texture2D CreateHueTexture(Color[] colors)
    {
        var hueTex = new Texture2D(colors.Length, 1);
        hueTex.SetPixels(colors);
        hueTex.Apply();
        return hueTex;
    }

    private void OnColorSliderValueChanged(float _value)
    {
        img_handle.color = GetColorFromValue(_value);
    }

    private Color GetColorFromValue(float value)
    {
        float scaledValue = value * (colors.Length - 1);
        int index = Mathf.FloorToInt(scaledValue);

        if (index >= colors.Length - 1)
        {
            return colors[colors.Length - 1];
        }

        float t = scaledValue - index;
        return Color.Lerp(colors[index], colors[index + 1], t);
    }
}