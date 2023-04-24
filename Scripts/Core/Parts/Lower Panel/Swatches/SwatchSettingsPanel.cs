using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.Swatches
{
    internal class SwatchSettingsPanel : MonoBehaviour, IDeselectHandler
    {
        [SerializeField] SwatchController controller;
        [SerializeField] RectTransform gridOption;
        [SerializeField] RectTransform listOption;
        [SerializeField] Image gridCheckmark;
        [SerializeField] Image listCheckmark;

        internal void Open()
        {
            gameObject.SetActive(true);
            bool grid = controller.CurrentLayout == SwatchLayout.GridLayout;
            gridCheckmark.gameObject.SetActive(grid);
            listCheckmark.gameObject.SetActive(!grid);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Vector2 mousePos = Input.mousePosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(gridOption, mousePos))
                controller.ChangeLayout(SwatchLayout.GridLayout);
            else if (RectTransformUtility.RectangleContainsScreenPoint(listOption, mousePos))
                controller.ChangeLayout(SwatchLayout.ListLayout);

            gameObject.SetActive(false);
        }
    }
}
