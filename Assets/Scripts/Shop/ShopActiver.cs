using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopActiver : MonoBehaviour
{
    public void GoToShop()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Shop");
    }
    public void GoToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
