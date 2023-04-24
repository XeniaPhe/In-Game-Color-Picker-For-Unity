using UnityEngine;
using UnityEngine.UI;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.Swatches
{
    internal class ScrollbarArrow : MonoBehaviour
    {
        [SerializeField] Scrollbar scrollbar;
        [SerializeField] bool up;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(UpdateScrollbar);
        }

        private void UpdateScrollbar()
        {
            float value = scrollbar.value;
            value += (up ? 1f : -1f) * (0.173125f - value / 640f);

            if (value < 0f)
                value = 0f;
            else if(value > 1f)
                value = 1f;

            scrollbar.value = value;
        }
    }
}