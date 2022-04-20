using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Game.Helper;
using Game.Asset;

namespace Design
{
    public class BoardLayoutDesigner : MonoBehaviour
    {
        [SerializeField] private Transform test;
        [SerializeField] private GameObject numberBoxPrefab;

#if UNITY_EDITOR
        string jsonPath => Application.dataPath + "/Resources/boardData.json";

        [ContextMenu("Generate Json")]
        void GenerateJson()
        {
            Vector2 resolution = GetComponentInChildren<CanvasScaler>().referenceResolution;

            Camera camera = Camera.main;

            // Create GUI Object
            var guiObjects = GetComponentsInChildren<GUIObject>(true);
            var boxes = new WagerBox[guiObjects.Length];
            for(int i = 0; i < guiObjects.Length; i++)
            {
                var guiObject = guiObjects[i];
                var image = guiObject.GetComponent<Image>();
                var text = guiObject.GetComponentInChildren<Text>();

                // Convert GUIObjectType to WagerType if guiObject is a wager UI
                string defineName = guiObject.Type.ToString();
                switch(guiObject.Type)
                {
                    case GUIObjectType.SingleWager:
                        defineName = WagerType.Single.ToString();
                        break;
                    case GUIObjectType.RangeWager:
                        defineName = WagerType.Range.ToString();
                        break;
                    case GUIObjectType.OddWager:
                        defineName = WagerType.Odd.ToString();
                        break;
                    case GUIObjectType.EvenWager:
                        defineName = WagerType.Even.ToString();
                        break;
                    case GUIObjectType.ColorWager:
                        defineName = WagerType.Color.ToString();
                        break;
                }

                // express logic in string
                string logic = "";
                switch(guiObject.Type)
                {
                    case GUIObjectType.SingleWager:
                        logic = text.text;
                        break;
                    case GUIObjectType.RangeWager:
                        var range = guiObject as RangeWagerGUI;
                        logic = $"{range.From}-{range.To}";
                        break;
                    case GUIObjectType.ColorWager:
                        var color = guiObject as ColorWagerGUI;
                        logic = $"{Extensions.ColorToString(color.color)}";
                        break;
                }

                boxes[i] = new WagerBox
                {
                    Name = defineName,
                    Position = Extensions.ScreenToCustomUnit(guiObject.transform.position),
                    Size = (guiObject.transform as RectTransform).sizeDelta,
                    Color = Extensions.ColorToString(image.color),
                    VisualText = text.text,
                    Logic = logic
                };

                if(i == 0)
                    Debug.Log("Log Test: " + boxes[i].Position);
            }

            var boardData = new BoardData
            {
                DesignResolution = resolution,
                Boxes = boxes
            };

            // Write Json file
            string strValue = JsonUtility.ToJson(boardData);

            if(File.Exists(jsonPath))
                File.Delete(jsonPath);
            
            File.WriteAllText(jsonPath, strValue);
            Debug.Log("Generate Json OK");
        }

        [ContextMenu("Test Json")]
        void TestJson()
        {
            Clear();

            if(!File.Exists(jsonPath))
                return;
            
            var boardData = JsonUtility.FromJson<BoardData>(File.ReadAllText(jsonPath));
            for(int i = 0; i < boardData.Boxes.Length; i++)
            {
                var data = boardData.Boxes[i];

                var box = Instantiate(numberBoxPrefab, test);
                var transform = box.transform as RectTransform;
                
                transform.position = Extensions.CustomUnitToScreen(data.Position);
                transform.sizeDelta = data.Size;
                box.GetComponent<Image>().color = Extensions.StringToColor(data.Color);
                box.GetComponentInChildren<Text>().text = data.VisualText;
            }
        }

        [ContextMenu("Clear")]
        void Clear()
        {
            foreach(Transform e in test)
                DestroyImmediate(e.gameObject);
        }
#endif

    }
}