using UnityEngine;
using UnityEngine.EventSystems;

namespace Xenia.ColorPicker.Core.Parts
{
    internal class DragPanel : MonoBehaviour, IDragHandler
    {
        [SerializeField] ColorPicker colorPicker;
        private RectTransform self;
        private RectTransform body;

        private void Awake()
        {
            self = GetComponent<RectTransform>();
            body = self.parent.GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(colorPicker.Draggable)
                body.position += new Vector3(eventData.delta.x, eventData.delta.y, 0);
        }
    }
}
