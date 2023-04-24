using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Xenia.ColorPicker.Core.Parts.UpperPanel
{
    internal class ScreenColorPicker : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] ColorPicker colorPicker;
        [SerializeField] MouseEventDetector detector;
        [SerializeField] PixelatedDisplay display;

        private Texture2D screenTexture;
        private Vector2Int currentPixel;

        public void OnPointerClick(PointerEventData eventData)
        {
            detector.Enable();
            display.Enable();
            StartCoroutine(CaptureScreenContinually());
        }

        private IEnumerator CaptureScreenContinually()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                screenTexture = ScreenCapture.CaptureScreenshotAsTexture();
                ZoomInOnMousePosition(currentPixel);
                yield return new WaitForSeconds(3f);
            }
        }

        internal void PickColor()
        {
            StopAllCoroutines();
            detector.Disable();
            display.Disable();
            colorPicker.CurrentColorHSV = screenTexture.GetPixel(currentPixel.x, currentPixel.y);
        }

        internal void ZoomInOnMousePosition(Vector2 position)
        {
            int width = Screen.width - 1;
            int height = Screen.height - 1;

            int x = Mathf.RoundToInt(position.x);
            int y = Mathf.RoundToInt(position.y);

            CheckCoordinates();

            currentPixel = new Vector2Int(x, y);

            x -= 10;
            y -= 10;

            CheckCoordinates();

            Texture2D textureToDisplay = new Texture2D(21, 21);

            for (int i = 0; i < 21; i++)
                for (int j = 0; j < 21; j++)
                    textureToDisplay.SetPixel(i, j, GetPixel(x + i, y + j));

            display.DisplayTexture(textureToDisplay);

            Color GetPixel(int x, int y)
            {
                if (x > width || y > height)
                    return Color.black;

                return screenTexture.GetPixel(x, y);
            }

            void CheckCoordinates()
            {
                x = x > width ? width : x;
                y = y > height ? height : y;
            }
        }
    }
}
