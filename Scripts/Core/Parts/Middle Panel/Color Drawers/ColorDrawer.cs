using Xenia.ColorPicker.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Xenia.ColorPicker.Core.Parts.MiddlePanel
{
    [RequireComponent((typeof(Image)))]
    internal abstract class ColorDrawer : MonoBehaviour, IPointerClickHandler, IColorChangeSubscriber
    {
        [SerializeField] protected ColorPicker colorPicker;
        [DisableInInspector][SerializeField] protected int textureSize;

        [SerializeField] protected ColorSelector selector;

        protected RectTransform rectTransform;
        protected Image image;
        protected float size;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            size = rectTransform.rect.width;
            colorPicker.ColorChangedInternal += ColorChanged;
        }

        public abstract void ColorChanged(object sender, ColorChangedEventArgs e);

        protected abstract bool IsPositionValid(Vector2 positionRect);

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Vector2 clickPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, null, out clickPos);

            if (IsPositionValid(clickPos))
            {
                selector.SelectPosition(new Vector3(clickPos.x, clickPos.y, 0));
            }
        }

        internal Color GetColorAtPoint(Vector2 point)
        {
            point += new Vector2(size / 2f, size / 2f);

            int xCoordinateInTexture = Mathf.RoundToInt((point.x / size) * textureSize);
            int yCoordinateInTexture = Mathf.RoundToInt((point.y / size) * textureSize);

            return image.sprite.texture.GetPixel(xCoordinateInTexture, yCoordinateInTexture);
        }
    }
}