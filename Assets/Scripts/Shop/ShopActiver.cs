using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopActiver : MonoBehaviour
{
    public AdsInitializer AdsInitializer;
    public void GoToShop()
    {
        SceneManager.LoadScene("Shop");
        AdsInitializer.InitializeAds();
    }
    public void GoToHome()
    {
        SceneManager.LoadScene(0);
    }
}
