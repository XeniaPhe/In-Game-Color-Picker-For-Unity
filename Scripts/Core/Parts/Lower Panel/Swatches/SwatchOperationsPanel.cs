using UnityEngine;
using UnityEngine.EventSystems;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.Swatches
{
    internal class SwatchOperationsPanel : MonoBehaviour, IDeselectHandler
    {
        [SerializeField] SwatchController controller;
        [SerializeField] RectTransform replace;
        [SerializeField] RectTransform delete;
        [SerializeField] RectTransform moveToFirst;

        private ColorSwatch swatch;

        internal void Open(ColorSwatch swatch, Vector3 topPosition)
        {
            gameObject.SetActive(true);
            RectTransform rectT = GetComponent<RectTransform>();
            rectT.position = topPosition - new Vector3(0, (rectT.rect.height / 2f) + 7f, 0);

            this.swatch = swatch;

            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Vector2 mousePos = Input.mousePosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(replace, mousePos))
                controller.UpdateSwatch(swatch);
            else if (RectTransformUtility.RectangleContainsScreenPoint(delete, mousePos))
                controller.RemoveSwatch(swatch);
            else if (RectTransformUtility.RectangleContainsScreenPoint(moveToFirst, mousePos))
                controller.MoveSwatchToFirst(swatch);

            gameObject.SetActive(false);
        }
    }
}