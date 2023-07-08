using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;
    public string curScene;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
