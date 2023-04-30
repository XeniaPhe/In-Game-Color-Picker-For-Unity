using UnityEngine;
using UnityEngine.UI;
using Xenia.ColorPicker;

public class DemoScript2 : MonoBehaviour
{
    [SerializeField] ColorPicker colorPicker;

    private Image image;
    private Button button;

    private static DemoScript2 currentSelected;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        button.onClick.AddListener(Select);
    }

    private void Select()
    {
        currentSelected?.Deselect();
        currentSelected = this;
        transform.localScale *= 1.5f;

        colorPicker.ColorChanged.AddListener(ChangeColor);
        colorPicker.Closing.AddListener(Deselect);

        RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

        colorPicker.OpenAtScreenPoint(transform.position - new Vector3(300, 50, 0));
    }

    private void ChangeColor(Color c)
    {
        image.color = c;
    }

    private void Deselect()
    {
        currentSelected = null;
        transform.localScale /= 1.5f;
        colorPicker.Closing.RemoveListener(Deselect);
        colorPicker.ColorChanged.RemoveListener(ChangeColor);
    }
}
