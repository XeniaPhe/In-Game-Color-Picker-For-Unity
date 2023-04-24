using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel
{
    internal class HexadecimalInputField : MonoBehaviour, IColorChangeSubscriber, IInputValidator<string>
    {
        [SerializeField] ColorPicker colorPicker;

        private TMP_InputField inputField;
        private IEnumerator currentCoroutine = null;
        private bool textSetHere = false;

        private void Awake()
        {
            inputField = GetComponent<TMP_InputField>();

            inputField.contentType = TMPro.TMP_InputField.ContentType.Alphanumeric;
            inputField.characterLimit = 6;
            inputField.onValueChanged.AddListener(Validate);
            colorPicker.ColorChangedInternal += ColorChanged;
        }

        public void Validate(string input)
        {
            if (textSetHere)
            {
                textSetHere = false;
                return;
            }

            if (input.Equals(string.Empty))
            {
                SetTextOnLoseFocus();
                return;
            }

            StringBuilder builder = new StringBuilder(6);

            foreach (var ch in input)
            {
                char temp = ch;

                if (char.IsLower(ch))
                    temp = char.ToUpper(ch);

                if ((temp >= '0' && temp <= '9') || (temp >= 'A' && temp <= 'F'))
                    builder.Append(temp);

                if (builder.Length == 6)
                    break;
            }

            SetTextImmediate(builder.ToString());

            builder.Clear().Append("#");

            string text = inputField.text;

            if (text.Length == 3 || text.Length == 4)
                builder.Append(text.Substring(0, 3));
            else if (input.Length == 6)
                builder.Append(text);
            else
                return;

            Color color;

            if (ColorUtility.TryParseHtmlString(builder.ToString(), out color))
                colorPicker.CurrentColorHSV = color;


            SetTextOnLoseFocus();
        }

        private void SetTextImmediate(string text)
        {
            CancelCoroutine();
            textSetHere = true;
            inputField.text = text;
        }

        private void SetTextOnLoseFocus()
        {
            CancelCoroutine();
            StartCoroutine(currentCoroutine = SetText());

            IEnumerator SetText()
            {
                while (inputField.isFocused)
                    yield return null;

                textSetHere = true;
                inputField.text = ColorUtility.ToHtmlStringRGB(colorPicker.CurrentColor);
            }
        }

        private void CancelCoroutine()
        {
            if (currentCoroutine is not null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
        }


        public void ColorChanged(object sender, ColorChangedEventArgs e)
        {
            SetTextOnLoseFocus();
        }
    }
}