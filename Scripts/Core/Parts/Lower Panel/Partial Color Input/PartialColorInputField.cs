using UnityEngine;
using TMPro;
using System;
using System.Collections;

namespace Xenia.ColorPicker.Core.Parts.LowerPanel.PartialColorInput
{
    internal class PartialColorInputField : PartialColorInputTransmitter, IInputValidator<string>
    {
        private TMP_InputField inputField;

        private IEnumerator currentCoroutine;
        private bool textSetHere = false;

        protected override void Awake()
        {
            base.Awake();
            inputField = GetComponent<TMP_InputField>();
            inputField.onValueChanged.AddListener(Validate);
        }

        public void Validate(string input)
        {
            if (textSetHere)
            {
                textSetHere = false;
                return;
            }

            componentValue = 0;
            float test;

            if(Single.TryParse(input, out test))
                componentValue = test > componentUpperBoundary ? componentUpperBoundary : (test > 0f ? test : 0f);

            SetTextOnLoseFocus();
            UpdateColorPicker();
        }

        protected override void ColorModeChanged(object sender, ColorModeChangedEventArgs e)
        {
            base.ColorModeChanged(sender, e);
            inputField.contentType = wholeValues ? TMP_InputField.ContentType.IntegerNumber : TMP_InputField.ContentType.DecimalNumber;
            SetTextImmediate();
        }

        public override void ColorChanged(object sender, ColorChangedEventArgs e)
        {
            base.ColorChanged(sender, e);
            SetTextImmediate();
        }

        private void SetTextImmediate()
        {
            if (currentCoroutine is not null)
                return;

            SetText();
        }

        private void SetTextOnLoseFocus()
        {
            if (currentCoroutine is not null)
                StopCoroutine(currentCoroutine);

            StartCoroutine(currentCoroutine = SetTextCoroutine());

            IEnumerator SetTextCoroutine()
            {
                yield return new WaitWhile(() => inputField.isFocused);
                SetText();
                currentCoroutine = null;
            }
        }

        private void SetText()
        {
            textSetHere = true;
            inputField.text = wholeValues? Mathf.RoundToInt(componentValue).ToString() : componentValue.ToString();
        }
    }
}