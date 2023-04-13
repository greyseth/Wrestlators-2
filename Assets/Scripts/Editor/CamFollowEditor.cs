using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CamFollow))]
public class CamFollowEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CamFollow trg = (CamFollow)target;

        if (GUILayout.Button("Match with transform"))
        {
            trg.offset = trg.transform.position;
        }
    }
}
