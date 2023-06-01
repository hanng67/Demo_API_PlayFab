using UnityEngine;

public class ColorUtils
{
    public static int HexToDec(string hex)
    {
        return System.Convert.ToInt32("0x" + hex, 16); // format 0xFFFFFF
    }

    public static string DecToHex(int dec)
    {
        return dec.ToString("X2");
    }

    public static string FloatNormalizedToHex(float normalized)
    {
        return DecToHex((int)(normalized * 255));
    }

    public static float HexToFloatNormalized(string hex)
    {
        return HexToDec(hex) / 255f;
    }

    public static Color GetColorFromString(string hexString)
    {
        Color color = new Color();
        color.r = HexToFloatNormalized(hexString.Substring(0, 2));
        color.g = HexToFloatNormalized(hexString.Substring(2, 2));
        color.b = HexToFloatNormalized(hexString.Substring(4, 2));
        color.a = 1f;

        if (hexString.Length >= 8)
            color.a = HexToFloatNormalized(hexString.Substring(6, 2));

        return color;
    }

    public static string GetStringFromColor(Color color, bool isUseAlpha = false)
    {
        string r = FloatNormalizedToHex(color.r);
        string g = FloatNormalizedToHex(color.g);
        string b = FloatNormalizedToHex(color.b);

        if (!isUseAlpha)
        {
            return r + g + b;
        }
        else
        {
            string a = FloatNormalizedToHex(color.a);
            return r + g + b + a;
        }
    }
}
