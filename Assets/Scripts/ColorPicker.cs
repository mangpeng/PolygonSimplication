using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public Texture2D tex;
    public Slider slider;
    
    public Color32 curColor = Color.white;
    
    private Image palette;

    private void Awake()
    {
        palette = GetComponent<Image>();
    }

    private void Start()
    {
        int width = tex.width;
        int height = tex.height;

        int curX = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //curX += x;
                float ratio = x / (float)width;
                Debug.Log($"{x} {width}");

                if (ratio < 0.17f)
                {
                    ratio = (float) (ratio - 0.17 + 0.17f);
                    Color32 temp = Color32.Lerp(new Color32(255,0, 0, 255), new Color32(255,255, 0, 255), ratio / 0.17f);
                    tex.SetPixel(x, y, temp);
                }
                else if (ratio < 0.33)
                {
                    ratio = (float) (ratio - 0.33 + 0.17f);
                    Color32 temp = Color32.Lerp(new Color32(255,255, 0, 255), new Color32(0,255, 0, 255), ratio / 0.17f);
                    tex.SetPixel(x, y, temp);
                }
                else if (ratio < 0.50)
                {
                    ratio = (float) (ratio - 0.50 + 0.17f);
                    Color32 temp = Color32.Lerp(new Color32(0,255, 0, 255), new Color32(0,255, 255, 255), ratio / 0.17f);
                    tex.SetPixel(x, y, temp);
                }
                else if (ratio < 0.67)
                {
                    ratio = (float) (ratio - 0.67 + 0.17f);
                    Color32 temp = Color32.Lerp(new Color32(0,255, 255, 255), new Color32(255,0, 255, 255), ratio / 0.17f);
                    tex.SetPixel(x, y, temp);
                }
                else if (ratio < 0.83)
                {
                    ratio = (float) (ratio - 0.83 + 0.17f);
                    Color32 temp = Color32.Lerp(new Color32(255,0, 255, 255), new Color32(255,0, 0, 255), ratio / 0.17f);
                    tex.SetPixel(x, y, temp);
                }
                else
                {
                    ratio = (float) (ratio - 1 + 0.17f);
                    Color32 temp = Color32.Lerp(new Color32(255,0, 0, 255), new Color32(255,0, 255, 255), ratio / 0.17f);
                    tex.SetPixel(x, y, temp);
                }
                
            }

            curX = 0;
        }
        tex.Apply(); 
        
        palette.material.mainTexture = tex;
    }

    private void Update()
    {
        int ratio = Convert.ToInt32(slider.value * tex.width);
        curColor = tex.GetPixel(ratio, 0);
    }
}
