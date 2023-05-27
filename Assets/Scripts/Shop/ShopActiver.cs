using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopActiver : MonoBehaviour
{
    public void GoToShop()
    {
        SceneManager.LoadScene("Shop");
    }
    public void GoToHome()
    {
        SceneManager.LoadScene(0);
    }
}
