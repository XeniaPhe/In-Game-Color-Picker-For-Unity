using UnityEngine;
using Xenia.ColorPicker;

public class PreviewDemo : MonoBehaviour
{
    [SerializeField] ColorPicker previewColorPicker;

    private new MeshRenderer renderer;
    private bool editColor = false;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();

        previewColorPicker.Opening.AddListener(() => editColor = true);
        previewColorPicker.Closing.AddListener(() => editColor = false);
        previewColorPicker.ColorPreview.AddListener(ColorPreview);
        previewColorPicker.ColorChanged.AddListener((Color c) => renderer.material.color = c);
    }

    private void Update()
    {
        if (editColor)
        {
            float time = Time.time;
            transform.rotation = Quaternion.Euler(time * 30f, time * 20f, time * 10f);
        }
    }

    private void ColorPreview(Color color)
    {
        Debug.Log("Previewing " + color.ToString());
        renderer.material.color = color;
    }
}
