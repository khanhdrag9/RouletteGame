using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Game.Helper;

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

            // Create GUI Object
            var guiObjects = GetComponentsInChildren<GUIObject>(true);
            var boxes = new WagerBox[guiObjects.Length];
            for(int i = 0; i < guiObjects.Length; i++)
            {
                var guiObject = guiObjects[i];
                var transform = guiObject.transform as RectTransform;
                var image = guiObject.GetComponent<Image>();
                var text = guiObject.GetComponentInChildren<Text>();

                // express logic in string
                string logic = "";
                switch(guiObject.Type)
                {
                    case GUIObjectType.RangeWager:
                        var range = guiObject as RangeWagerGUI;
                        logic = $"{range.From}-{range.To}";
                        break;
                    case GUIObjectType.ColorWager:
                        var color = guiObject as ColorWagerGUI;
                        logic = $"{color.color.ToString()}";
                        break;
                }

                boxes[i] = new WagerBox
                {
                    Name = guiObject.Type.ToString(),
                    Position = transform.position,
                    Size = transform.sizeDelta,
                    Color = Extensions.ColorToString(image.color),
                    VisualText = text.text,
                    Logic = logic
                };
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
                
                transform.position = data.Position;
                transform.sizeDelta = data.Size;
                box.GetComponent<Image>().color = Extensions.StringToColor(data.Color);
                box.GetComponentInChildren<Text>().text = data.VisualText;

                var guiObject = box.GetComponent<GUIObject>();
                guiObject.Type = Extensions.StringToEnum<GUIObjectType>(data.Name);
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