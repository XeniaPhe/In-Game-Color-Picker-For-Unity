using TMPro;
using UnityEngine;
using Xenia.ColorPicker;

public class DemoScript1 : MonoBehaviour
{
    [SerializeField] ColorPicker colorPicker;
    [SerializeField] TMP_Text text;

    private new MeshRenderer renderer;
    private bool editColor = false;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();

        colorPicker.Opening.AddListener(() => editColor = true);
        colorPicker.Closing.AddListener(() => editColor = false);
        colorPicker.ColorPreview.AddListener(ColorPreview);
        colorPicker.ColorChanged.AddListener((Color c) => renderer.material.color = c);
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

    public void ChangeColor(Color c)
    {
        Debug.Log(c);
        renderer.material.color = c;
    }

    public void ChangeText(Color c)
    {
        text.color = c;
        text.text = c.ToString();
    }
}
