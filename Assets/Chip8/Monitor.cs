using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour
{
    Texture2D texture;
    public Material mat;
    public Color BackColor = Color.black;
    public Color ForeColor = Color.blue;

    public void CreateTexture(int width, int height)
    {
        texture = new Texture2D(width, height);
        mat.mainTexture = texture;
    }

    public void Clear(Color32 color)
    {
        Color32[] newColors = new Color32[texture.width * texture.height];
        for (int i = 0; i < newColors.Length; i++)
        {
            newColors[i]=color;
        }
        SetPixels32(newColors);
    }

    public void SetPixels32(Color32[] Colors)
    {
        texture.SetPixels32(Colors);
        texture.Apply();
    }

    public void SetPixels32(bool[] bColors)
    {
        Color32[] Colors = new Color32[bColors.Length];
        for (int i = 0; i < bColors.Length; i++)
        {
            if (bColors[i])
                Colors[i] = ForeColor;
            else
                Colors[i] = BackColor;
        }
        texture.SetPixels32(Colors);
        texture.Apply();
    }
}
