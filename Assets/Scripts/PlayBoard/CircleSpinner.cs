using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CircleSpinner : MonoBehaviour
    {
        [SerializeField] private RectTransform spinnerElementGroup;
        [SerializeField] private GameObject spinnerElementPrefab;

        /// <summary>
        /// Design data of a gameplay
        /// <param name="orderOfNumer">clockwise order of numbers</param>
        /// </summary>
        public void Initialize(int[] orderOfNumer)
        {
            int numberElement = orderOfNumer.Length;
            for(int i = 0; i < numberElement; ++i)
            {
                var element = Instantiate(spinnerElementPrefab, spinnerElementGroup);

                // Adjust angle of number element in spinner board
                var eImage = element.GetComponent<Image>();
                eImage.color = i % 2 == 0 ? Color.red : Color.black;
                eImage.fillAmount = 1f / numberElement;
                eImage.rectTransform.localRotation = Quaternion.Euler(0, 0, (i + 0.5f) * 360f / numberElement);
                eImage.rectTransform.sizeDelta = spinnerElementGroup.sizeDelta;

                // Adjust position of number text in each element
                var eText = element.GetComponentInChildren<Text>();
                eText.text = orderOfNumer[i].ToString();
                eText.rectTransform.localRotation = Quaternion.Euler(0, 0, -180f / numberElement);
            }
        }
    }
}
