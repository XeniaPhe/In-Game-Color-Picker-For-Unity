using UnityEngine;
using UnityEngine.EventSystems;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.Swatches
{
    internal class SwatchesButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] SwatchController controller;
        [SerializeField] RectTransform swatchesArrow;
        [SerializeField] RectTransform settingsButton;

        private bool state;

        private void Start()
        {
            state = controller.IsEnabled();
            settingsButton.gameObject.SetActive(state);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            state = !state;
            swatchesArrow.rotation = Quaternion.Euler(0, 0, state ? 0f : 90f);
            controller.Toggle(state);
            settingsButton.gameObject.SetActive(state);
        }
    }
}
