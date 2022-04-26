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
    public class CircleSpinnerV2 : MonoBehaviour, ISpinner
    {
        public enum Direction
        {
            Clockwise, Anticlockwise
        } 

        private enum SpinPharse
        {
            Start, End
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
        private int currentFullRotationTime;
        private float randomSpinDuration
        {
            get
            {
                currentSpinDuration = UnityEngine.Random.Range(minSpinDuration, maxSpinDuration);
                return currentSpinDuration;
            }
        }
        private float currentSpinDuration;
        private float anglePerItem;
        private float angle
        {
            get => spinnerItemGroup.eulerAngles.z;
            set => spinnerItemGroup.eulerAngles = new Vector3(0f, 0f, value);
        }
        private float spinCountTime;
        private int spinDirectionInt => spinDirection == Direction.Clockwise ? -1 : 1;
        private SpinPharse spinPharse;
        private float durationPerFullRot;
        private float startAngle;
        private float endAngle;
        private float spinDurationToEnd;
        private float cacheSpeedFactor = 0;
        private int indexOfExpectResult = -1;

        public GameObject GameObject => gameObject;
        public bool IsSpinning {get; private set;}

        /// <summary>
        /// Design data of a gameplay
        /// <param name="orderOfNumber">clockwise order of numbers</param>
        /// </summary>
        public void Initialize(SpinnerConfig config)
        {
            StopAllCoroutines();
            int numberItem = config.Items.Length;
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
            IsSpinning = true;
            spinCountTime = 0;
            durationPerFullRot = randomSpinDuration / randomFullRotationTime;
        }

        /// <summary>
        /// Start spinning
        /// <param name="expectResult"></param>
        /// </summary>
        public void Spin(int expectResult)
        {
            Debug.Log("Rotate to number " + expectResult);
            // Get Index of expect result
            indexOfExpectResult = Array.FindIndex(config.Items, e => e.Number == expectResult);
        }

        private float ValidateAngle(float value)
        {
            while (value >= 360) value -= 360;
            while (value <= -360) value += 360;
            return value;
        }

        private void OnEnable()
        {
            angle = 0;
            spinPharse = SpinPharse.Start;
            spinCountTime = 0f;
            indexOfExpectResult = -1;
        }

        private void Update()
        {
            float halfSpinDuration = currentSpinDuration / 2f;
            float speedScale = 1f;

            if(spinPharse == SpinPharse.Start)
            {
                float speedFactor = Extensions.EaseInCubic(spinCountTime, 0f, 1f, halfSpinDuration) * speedScale;
                float newAngle = Mathf.LerpUnclamped(0, -360, speedFactor * spinCountTime / durationPerFullRot);
                angle = newAngle;

                cacheSpeedFactor = speedFactor;
                spinCountTime += Time.deltaTime;

                // We should spin enough halfSpinDuration seconds and wait for result
                if(spinCountTime >= halfSpinDuration && indexOfExpectResult >= 0)
                {
                    // get angle of target item in 1 circle
                    float itemNumberAngle = 360f - indexOfExpectResult * anglePerItem;

                    startAngle = ValidateAngle(angle);

                    // Get angle when spin, anglePerItem / 20f is padding amount
                    float half = anglePerItem / 2f - anglePerItem / 20f;
                    endAngle = spinDirectionInt * (itemNumberAngle + 360f * currentFullRotationTime) + UnityEngine.Random.Range(-half, half);

                    spinDurationToEnd = spinCountTime;
                    Debug.Log("Spincount: " + spinCountTime);

                    spinPharse = SpinPharse.End;
                }
            }

            if(spinPharse == SpinPharse.End)
            {
                float speedFactor = Mathf.Clamp(Extensions.EaseOutCubic(spinCountTime, 0f, cacheSpeedFactor, spinDurationToEnd), 0, cacheSpeedFactor) * speedScale;
                float factor = speedFactor * spinCountTime / spinDurationToEnd;
                angle = factor <= 0 ? endAngle : Mathf.LerpUnclamped(endAngle, startAngle, factor);

                spinCountTime -= Time.deltaTime;
            }
        }
    }
}
