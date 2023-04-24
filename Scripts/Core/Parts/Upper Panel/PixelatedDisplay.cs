using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Xenia.ColorPicker.Core.Parts.UpperPanel
{
    internal class PixelatedDisplay : MonoBehaviour
    {
        private Image[,] pixels;
        private Transform parent;

        private void Awake()
        {
            parent = transform.parent;
            List<Image> images = GetComponentsInChildren<Image>().OrderBy(i => i.transform.GetSiblingIndex()).ToList();
            images.Remove(GetComponent<Image>());

            pixels = new Image[21, 21];

            for (int i = 0; i < 21; i++)
                for (int j = 0; j < 21; j++)
                    pixels[j, 20 - i] = images[21 * i + j];

            Disable(false);
        }

        internal void Enable()
        {
            enabled = true;

            parent.gameObject.SetActive(true);
            parent.SetSiblingIndex(parent.GetSiblingIndex() + 1);

            foreach (var image in pixels)
                image.enabled = true;
        }

        internal void Disable(bool changeIndex = true)
        {
            enabled = false;
            parent.gameObject.SetActive(false);
            if(changeIndex)
                parent.SetSiblingIndex(parent.GetSiblingIndex() - 1);

            foreach (var image in pixels)
                image.enabled = false;
        }

        internal void DisplayTexture(Texture2D texture)
        {
            for (int i = 0; i < 21; i++)
                for (int j = 0; j < 21; j++)
                    pixels[i, j].color = texture.GetPixel(i, j);
        }
    }
}
