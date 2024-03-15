using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(R_Int_ReadOnly))]
public class ED_R_Int_Readonly : Editor
{
    R_Int_ReadOnly data;
    public override void OnInspectorGUI()
    {
        data = (R_Int_ReadOnly)target;
        if (data != null)
        {
            base.OnInspectorGUI();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Value: " + data.value);
            EditorGUILayout.EndHorizontal();
        }
    }
}
