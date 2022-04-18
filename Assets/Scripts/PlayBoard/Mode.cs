using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.PlayBoard
{
    public class Mode : MonoBehaviour
    {
        [SerializeField] private RectTransform spinnerElementGroup;
        [SerializeField] private GameObject spinnerElementPrefab;

        public void InitalizeBoard(ModeData modeData)
        {
            int numberElement = modeData.numbers.Length;
            for(int i = 0; i < numberElement; ++i)
            {
                var element = SpawnElementGroup();

                // Set angle in spinner board
                Image eImage = element.GetComponent<Image>();
                eImage.color = i % 2 == 0 ? Color.red : Color.black;
                eImage.fillAmount = 1f / numberElement;
                eImage.rectTransform.localRotation = Quaternion.Euler(0, 0, (i + 0.5f) * 360f / numberElement);
                eImage.rectTransform.sizeDelta = spinnerElementGroup.sizeDelta;

                // Set rotation of number in each element
                Text eText = element.GetComponentInChildren<Text>();
                eText.text = modeData.numbers[i].ToString();
                eText.rectTransform.localRotation = Quaternion.Euler(0, 0, -180f / numberElement);
            }
        }

        private GameObject SpawnElementGroup()
        {
            var element = Instantiate(spinnerElementPrefab, spinnerElementGroup);
            return element;
        }
    }
}
