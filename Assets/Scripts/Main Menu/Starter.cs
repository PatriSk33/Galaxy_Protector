using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Starter : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1f;
        Loader.Load(Loader.Scene.GameScene);
    }
}
