using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.Swatches
{
    internal class ColorSwatch : MonoBehaviour, IPointerClickHandler
    {
        private SwatchController controller;
        private Image top;
        private Image bottom;

        internal Color Color
        {
            get => top.color;
            set
            {
                top.color = value;
                value.a = 1f;
                bottom.color = value;
            }
        }

        private void OnEnable()
        {
            top = transform.GetChild(0).GetComponent<Image>();
            bottom = top.transform.GetChild(0).GetComponent<Image>();
        }

        internal void Initialize(SwatchController controller, Color color)
        {
            this.controller = controller;
            Color = color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.pointerId)
            {
                case -1:
                    controller.UpdateColorPicker(top.color);
                    break;
                case -2:
                    var rectT = GetComponent<RectTransform>();
                    Vector3 bottomPos = rectT.position - new Vector3(0, rectT.rect.height / 2f, 0);
                    controller.OperationsPanel.Open(this, bottomPos);
                    break;
            }
        }
    }
}
