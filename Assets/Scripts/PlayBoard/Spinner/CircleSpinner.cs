using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Game
{
    /// <summary>
    /// Circle spinner
    /// Spin Logic reference: https://stackoverflow.com/questions/57973167/how-to-choose-when-fortune-wheel-stop-in-unity
    /// </summary>
    public class CircleSpinner : MonoBehaviour, ISpinner
    {
        [Header("UI creation")]
        [SerializeField] private RectTransform spinnerItemGroup;
        [SerializeField] private GameObject spinnerItemPrefab;

        [Header("Spin properties")]
        [SerializeField][Tooltip("Minimum full rotations")] private int minRotationTime = 5;
        [SerializeField][Tooltip("Maximum full rotations")] private int maxRotationTime = 7;
        [SerializeField][Tooltip("Minimum spin duration")] private float minSpinDuration  = 5f;
        [SerializeField][Tooltip("Maximum spin duration")] private float maxSpinDuration  = 7f;

        private int[] orderOfNumber;
        private int randomFullRotationTime => UnityEngine.Random.Range(minRotationTime, maxRotationTime + 1);
        private float randomSpinDuration => UnityEngine.Random.Range(minSpinDuration, maxSpinDuration);
        private int numberItem;
        private float anglePerItem;
        private float angle
        {
            get => transform.eulerAngles.z;
            set => transform.eulerAngles = new Vector3(0f, 0f, value);
        }


        public GameObject GameObject => gameObject;
        public bool IsSpinning {get; private set;}

        /// <summary>
        /// Design data of a gameplay
        /// <param name="orderOfNumber">clockwise order of numbers</param>
        /// </summary>
        public void Initialize(int[] orderOfNumber)
        {
            StopAllCoroutines();
            numberItem = orderOfNumber.Length;
            IsSpinning = false;
            anglePerItem = 360f / numberItem;
            angle = 0;
            float defaultWeight = 100f / numberItem;

            for(int i = 0; i < numberItem; i++)
            {
                var element = Instantiate(spinnerItemPrefab, spinnerItemGroup);

                // Adjust anchor
                var rectTran = element.transform as RectTransform;
                rectTran.anchorMin = new Vector2(0, 0);
                rectTran.anchorMax = new Vector2(1, 1);

                // Adjust rotation
                var eImage = element.GetComponent<Image>();
                eImage.color = i % 2 == 0 ? Color.red : Color.black;
                eImage.fillAmount = 1f / numberItem;
                eImage.rectTransform.localRotation = Quaternion.Euler(0, 0, (-i + 0.5f) * anglePerItem);
                eImage.rectTransform.sizeDelta = spinnerItemGroup.sizeDelta;

                // Adjust position of number text in each element
                var eText = element.GetComponentInChildren<Text>();
                eText.text = orderOfNumber[i].ToString();
                eText.rectTransform.localRotation = Quaternion.Euler(0, 0, -180f / numberItem);
            }
            
            this.orderOfNumber = orderOfNumber;
        }

        /// <summary>
        /// Start spinning
        /// <param name="expectResult">the result of spin</param>
        /// </summary>
        public void Spin(int expectResult)
        {
            // Get Index of expect result
            int indexOfExpectResult = Array.FindIndex(orderOfNumber, e => e == expectResult);

            // Start Spin here
            StartCoroutine(HandleSpin(indexOfExpectResult));
        }

        private IEnumerator HandleSpin(int toIndex)
        {
            float itemNumberAngle = 360f - toIndex * anglePerItem;
            
            // Get current angle in [0-360]
            float currentAngle = angle;
            while (currentAngle >= 360) currentAngle -= 360;
            while (currentAngle <= -360) currentAngle += 360;

            int fullRotation = randomFullRotationTime;

            // Get angle when spin clockwise
            float targetAngle = -(itemNumberAngle + 360f * fullRotation);

            // Start handling spin
            IsSpinning = true;
            float passedTime = 0f;
            float spinDuration = randomSpinDuration;

            while(passedTime < spinDuration)
            {
                float lerpFactor = Mathf.SmoothStep(0, 1, (Mathf.SmoothStep(0, 1, passedTime / spinDuration)));
                angle = Mathf.Lerp(currentAngle, targetAngle, lerpFactor);
                passedTime += Time.deltaTime;

                yield return null;
            }

            angle = targetAngle;
            IsSpinning = false;
        }

        private void OnEnable()
        {
            angle = 0;
        }
    }
}
