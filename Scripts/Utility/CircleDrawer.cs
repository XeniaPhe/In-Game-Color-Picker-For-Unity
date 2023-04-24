using System;
using UnityEngine;
using UnityEngine.UI;

namespace Xenia.ColorPicker.Utility
{
    internal class CircleDrawer
    {
        internal void DrawCircle(Image image, int size, int radius, int innerRadius, Color color, Color inside, Color outside, string name)
        {
            Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, true);
            texture.anisoLevel = 16;
            texture.filterMode = FilterMode.Trilinear;
            texture.wrapMode = TextureWrapMode.Clamp;

            Vector2 point = new Vector2(0, 0);
            float distance;
            float center = size / 2f;

            for (int x = 0; x < size; ++x)
            {
                point.x = x + 0.5f;

                for (int y = 0; y < size; ++y)
                {
                    point.y = y + 0.5f;
                    distance = Mathf.Sqrt(Mathf.Pow(center - point.x, 2) + Mathf.Pow(center - point.y, 2));

                    if (distance <= innerRadius)
                    {
                        texture.SetPixel(x, y, inside);
                    }
                    else if (distance >= radius)
                    {
                        texture.SetPixel(x, y, outside);
                    }
                    else
                    {
                        texture.SetPixel(x, y, color);
                    }
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            sprite.name = name;
            image.sprite = sprite;
        }

        internal void DrawCircle(Image image, int size, int radius, int innerRadius, Func<Vector2, Color> colorFunc, Color inside, Color outside, string name)
        {
            Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, true);
            texture.anisoLevel = 16;
            texture.filterMode = FilterMode.Trilinear;
            texture.wrapMode = TextureWrapMode.Clamp;


            Vector2 point = new Vector2Int(0, 0);
            float distance;
            float center = size / 2f;

            for (int x = 0; x < size; ++x)
            {
                point.x = x + 0.5f;

                for (int y = 0; y < size; ++y)
                {
                    point.y = y + 0.5f;
                    distance = Mathf.Sqrt(Mathf.Pow(center - point.x, 2) + Mathf.Pow(center - point.y, 2));

                    if (distance <= innerRadius)
                    {
                        texture.SetPixel(x, y, inside);
                    }
                    else if (distance >= radius)
                    {
                        texture.SetPixel(x, y, outside);
                    }
                    else
                    {
                        texture.SetPixel(x, y, colorFunc(point));
                    }
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            sprite.name = name;
            image.sprite = sprite;
        }
    }
}
