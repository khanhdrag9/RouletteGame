using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Design
{
    [CustomEditor(typeof(BoardLayoutDesigner))]
    public class BoardLayoutDesignerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if(GUILayout.Button("Save json"))
            {
                (target as BoardLayoutDesigner).GenerateJson();
            }
        }
    }
}