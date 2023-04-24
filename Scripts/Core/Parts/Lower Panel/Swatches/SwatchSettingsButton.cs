using UnityEngine;
using UnityEngine.UI;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.Swatches
{
    internal class SwatchSettingsButton : MonoBehaviour
    {
        [SerializeField] SwatchSettingsPanel panel;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(panel.Open);
        }
    }
}
