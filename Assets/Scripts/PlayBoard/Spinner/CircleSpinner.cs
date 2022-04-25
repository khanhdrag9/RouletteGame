using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Game.Data;
using Game.Helper;

namespace Game
{
    /// <summary>
    /// Circle spinner
    /// Spin Logic reference: https://stackoverflow.com/questions/57973167/how-to-choose-when-fortune-wheel-stop-in-unity
    /// </summary>
    public class CircleSpinner : MonoBehaviour, ISpinner
    {
        public enum Direction
        {
            Clockwise, Anticlockwise
        } 


        [Header("UI creation")]
        [SerializeField] private RectTransform spinnerItemGroup;
        [SerializeField] private GameObject spinnerItemPrefab;

        [Header("Spin properties")]
        [SerializeField][Tooltip("Minimum full rotations")] private int minRotationTime = 5;
        [SerializeField][Tooltip("Maximum full rotations")] private int maxRotationTime = 7;
        [SerializeField][Tooltip("Minimum spin duration")] private float minSpinDuration  = 5f;
        [SerializeField][Tooltip("Maximum spin duration")] private float maxSpinDuration  = 7f;
        [SerializeField][Tooltip("Defailt spin speed")] private Direction spinDirection;
    

        private SpinnerConfig config;
        private int randomFullRotationTime
        {
            get
            {
                currentFullRotationTime = UnityEngine.Random.Range(minRotationTime, maxRotationTime + 1);
                return currentFullRotationTime;
            }
        }
        private float randomSpinDuration
        {
            get
            {
                currentSpinDuration = UnityEngine.Random.Range(minSpinDuration, maxSpinDuration);
                return currentSpinDuration;
            }
        }
        private int numberItem;
        private float anglePerItem;
        private float angle
        {
            get => spinnerItemGroup.eulerAngles.z;
            set => spinnerItemGroup.eulerAngles = new Vector3(0f, 0f, value);
        }
        private float passedTime;
        private Coroutine spinCoroutine;
        private int currentFullRotationTime;
        private float currentSpinDuration;
        private int spinDirectionInt => spinDirection == Direction.Clockwise ? -1 : 1;

        public GameObject GameObject => gameObject;
        public bool IsSpinning {get; private set;}

        /// <summary>
        /// Design data of a gameplay
        /// <param name="orderOfNumber">clockwise order of numbers</param>
        /// </summary>
        public void Initialize(SpinnerConfig config)
        {
            StopAllCoroutines();
            numberItem = config.Items.Length;
            IsSpinning = false;
            anglePerItem = 360f / numberItem;
            angle = 0;

            for(int i = 0; i < numberItem; i++)
            {
                var item = Instantiate(spinnerItemPrefab, spinnerItemGroup);
                var itemData = config.Items[i];
                var spinnerGui = item.GetComponent<SpinnerItemGUI>();

                // Adjust anchor - item auto fit with size of spinner
                var rectTran = item.transform as RectTransform;
                rectTran.anchorMin = new Vector2(0, 0);
                rectTran.anchorMax = new Vector2(1, 1);

                // Adjust background, use circle fillAmount to make element on a circle wheel
                var eImage = spinnerGui.Background;
                eImage.color = Extensions.StringToColor(itemData.Color);
                eImage.fillAmount = 1f / numberItem;
                eImage.rectTransform.localRotation = Quaternion.Euler(0, 0, (-i + 0.5f) * anglePerItem);
                eImage.rectTransform.sizeDelta = spinnerItemGroup.sizeDelta;

                // Adjust position of number text in each item
                var eText = spinnerGui.Text;
                eText.text = itemData.Number.ToString();
                eText.rectTransform.localRotation = Quaternion.Euler(0, 0, -180f / numberItem);

                // Adjust position of icon in each item
                var icon = spinnerGui.Icon;
                icon.sprite = ServiceLocator.GetService<AssetService>().GetSprite(itemData.Sprite);
                icon.transform.parent.localRotation = Quaternion.Euler(0, 0, -180f / numberItem);
                icon.gameObject.SetActive(icon.sprite != null);
            }
            
            this.config = config;
        }

        public void Spin()
        {
            passedTime = 0;

            // Spin without a specify target, calculate the speed to spin
            // Should mul by 1.5f to make transition between spin without target and with a target is more smooth.
            float anglePerSecond = (randomFullRotationTime * 360f / randomSpinDuration) * 1.5f;
            if(spinCoroutine != null) StopCoroutine(spinCoroutine);
            spinCoroutine = StartCoroutine(SpinWithSpeed(anglePerSecond));
        }

        /// <summary>
        /// Start spinning
        /// <param name="expectResult"></param>
        /// </summary>
        public void Spin(int expectResult)
        {
            // Get Index of expect result
            int indexOfExpectResult = Array.FindIndex(config.Items, e => e.Number == expectResult);

            // Start Spin here
            if(spinCoroutine != null) StopCoroutine(spinCoroutine);
            spinCoroutine = StartCoroutine(HandleSpin(indexOfExpectResult));
        }

        private IEnumerator HandleSpin(int toIndex)
        {
            float itemNumberAngle = 360f - toIndex * anglePerItem;
            
            // Get current angle in [0-360]
            float currentAngle = ValidateAngle(angle);

            Debug.Log("Spin to index " + toIndex);

            int fullRotation = currentFullRotationTime;

            // Get angle when spin clockwise, anglePerItem / 20f is padding amount
            float half = anglePerItem / 2f - anglePerItem / 20f;
            float targetAngle = spinDirectionInt * (itemNumberAngle + 360f * fullRotation) + UnityEngine.Random.Range(-half, half);

            // Start handling spin
            IsSpinning = true;
            // float passedTime = 0f;
            float spinDuration = currentSpinDuration + passedTime;

            yield return StartSpin(spinDuration, currentAngle, targetAngle);

            angle = targetAngle;
            IsSpinning = false;
        }

        private IEnumerator StartSpin(float duration, float from, float to)
        {
            while(passedTime < duration)
            {
                // Smooth Step make slow to fast to slow
                float lerpFactor = Mathf.SmoothStep(0, 1, (Mathf.SmoothStep(0, 1f, passedTime / duration)));
                float newAngle = Mathf.Lerp(from, to, lerpFactor);
                
                angle = newAngle;
                passedTime += Time.deltaTime;

                yield return null;
            }
        }

        private IEnumerator SpinWithSpeed(float maxSpeed, float timeToMaxSpeed = 2f)   // maxSpeed is angle per second
        {
            while(true)
            {
                float speed = Mathf.Lerp(0, maxSpeed, Mathf.Clamp01(passedTime / timeToMaxSpeed));
                angle += spinDirectionInt * speed * Time.deltaTime;

                passedTime += Time.deltaTime;
                yield return null;
            }
        }

        private float ValidateAngle(float value)
        {
            while (value >= 360) value -= 360;
            while (value <= -360) value += 360;
            return value;
        }

        private void OnEnable()
        {
            if(spinCoroutine != null) StopCoroutine(spinCoroutine); 
            angle = 0;
            passedTime = 0;
        }

        
    }
}
