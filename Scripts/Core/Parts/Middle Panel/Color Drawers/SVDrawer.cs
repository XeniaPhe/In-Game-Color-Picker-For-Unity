using UnityEngine;

namespace Xenia.ColorPicker.Core.Parts.MiddlePanel
{
    internal class SVDrawer : ColorDrawer
    {
        [SerializeField] HueSelector hueSelector;

        internal void Initialize(int textureSize)
        {
            this.textureSize = textureSize;
        }

        protected override bool IsPositionValid(Vector2 position)
        {
            return true;
        }

        public override void ColorChanged(object sender, ColorChangedEventArgs e)
        {
            float delta = 1f / textureSize;

            HSVColor pixelColor = new HSVColor(e.Color.H, 0f, 0f, 1f);

            Texture2D texture = new Texture2D(textureSize, textureSize);
            texture.anisoLevel = 16;
            texture.filterMode = FilterMode.Trilinear;
            texture.wrapMode = TextureWrapMode.Clamp;

            for (int i = 0; i < textureSize; i++)
            {
                pixelColor.V = 0f;

                for (int j = 0; j < textureSize; j++)
                {
                    texture.SetPixel(i, j, pixelColor.ToRGB());
                    pixelColor.V += delta;
                }

                pixelColor.S += delta;
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            sprite.name = typeof(SVDrawer).Name;
            image.sprite = sprite;
        }
    }
}