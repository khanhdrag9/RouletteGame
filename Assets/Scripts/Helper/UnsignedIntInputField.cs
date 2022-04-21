using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Helper
{
    [RequireComponent(typeof(InputField))]
    public class UnsignedIntInputField : MonoBehaviour
    {
        private InputField inputField;
        private InputField.ContentType cacheContentType;

        private void Validate(string value)
        {
            int intValue = int.Parse(value);
            if(intValue < 0) intValue = 0;
            inputField.text = intValue.ToString();
        }

        private void Awake()
        {
            inputField = GetComponent<InputField>();
            inputField.contentType = InputField.ContentType.IntegerNumber;
            inputField.onEndEdit.AddListener(Validate);
        }

        private void OnEnable()
        {
            cacheContentType = inputField.contentType;
            inputField.contentType = InputField.ContentType.IntegerNumber;
            inputField.onEndEdit.AddListener(Validate);
        }

        private void OnDisable()
        {
            if(inputField)
            {
                inputField.contentType = cacheContentType;
                inputField.onEndEdit.RemoveListener(Validate);
            }
        }

    }
}
