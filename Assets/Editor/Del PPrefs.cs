using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DelPPrefs : EditorWindow
{
    [MenuItem("Game/Delete PlayerPrefs (All)")]
    static void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}


