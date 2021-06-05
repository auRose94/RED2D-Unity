using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class LevelLoader : MonoBehaviour
{
    public TextAsset LevelFile;
    public Dictionary<string, GameObject> PrefabMap; 
    private bool loaded = false;

    void LoadLevel() {
        if (!loaded) {
            loaded = true;
            byte[] byteText = LevelFile.bytes;

        }
    }

    void Update() {
        LoadLevel();
    }
}