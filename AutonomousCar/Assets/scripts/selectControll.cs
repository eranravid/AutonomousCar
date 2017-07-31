using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class selectControll : EditorWindow { 


    [MenuItem("Custom/Select Lightmap Static")]
    static void Init()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Checkpoint");
        Selection.objects = gos;
    }
}
