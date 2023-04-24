using UnityEngine;
using UnityEngine.UI;

namespace Xenia.ColorPicker.Core.Parts.TitlePanel
{
    internal class CloseButton : MonoBehaviour
    {
        [SerializeField] ColorPicker colorPicker;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(colorPicker.Close);
        }
    }
}
