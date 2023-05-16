using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Starter : MonoBehaviour
{
    public static int wave;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
