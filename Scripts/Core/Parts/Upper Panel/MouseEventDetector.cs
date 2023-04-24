using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Xenia.ColorPicker.Core.Parts.UpperPanel
{
    internal class MouseEventDetector : MonoBehaviour, IPointerMoveHandler, IPointerClickHandler
    {
        [SerializeField] ScreenColorPicker colorPicker;

        private Image detector;

        private void Awake()
        {
            detector = GetComponent<Image>();
            Disable();
        }

        internal void Enable()
        {
            detector.enabled = true;
            enabled = true;
        }

        internal void Disable()
        {
            detector.enabled = false;
            enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            colorPicker.PickColor();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            colorPicker.ZoomInOnMousePosition(eventData.position);
        }
    }
}
