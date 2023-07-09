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
    public static string curScene;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        curScene = SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string scene)
    {
        curScene = scene;
        SceneManager.LoadScene(scene);
    }
}
