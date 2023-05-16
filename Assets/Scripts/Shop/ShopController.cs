using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public static ShopController instance;
    public Camera cam;
    int x;

    int Rockets;
    public bool[] buyed = new bool[4];
    public int[] price = new int[4];

    //Textovky
    public Text select, priceTag;
    public Button buyButton;
    public Button selectButton;

    //Audio
    public AudioSource notEnoughMoneySound;

    //When run get the instance and then go to main menu
    public static bool alreadyStarted;

    private void Awake()
    {
        if (!alreadyStarted) 
        { 
            alreadyStarted = true;
            SceneManager.LoadScene(0); 
            instance = this; 
        }
        Rockets = 0;
        priceTag.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);
        
        if (Rockets == StatController.selected) { select.text = "Selected"; }
    }

    private void Start()
    {
        GetIfBuyed();
    }

    public void GetIfBuyed()
    {
        if (PlayerPrefs.HasKey("Dual"))
        {
            buyed[1] = intToBool(PlayerPrefs.GetInt("Dual"));
            buyed[2] = intToBool(PlayerPrefs.GetInt("Triple"));
            buyed[3] = intToBool(PlayerPrefs.GetInt("Cannon"));
        }
    }
    public void SaveAsBuyed()
    {
        PlayerPrefs.SetInt("Dual", boolToInt(buyed[1]));
        PlayerPrefs.SetInt("Triple", boolToInt(buyed[2]));
        PlayerPrefs.SetInt("Cannnon", boolToInt(buyed[3]));
    }
    public void MoveLeft()
    {
        if (cam.transform.position.x == 0)
        {
            x = 45;
            Rockets = 3;
            if (buyed[Rockets])
            {
                buyButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);
                priceTag.gameObject.SetActive(false);
                if (Rockets == StatController.selected) { select.text = "Selected"; }
                else { select.text = "Select"; }
            }
            else 
            { 
                buyButton.gameObject.SetActive(true);
                selectButton.gameObject.SetActive(false);
                priceTag.gameObject.SetActive(true);
                priceTag.text = price[Rockets].ToString();
            }
        }
        else
        {
            x -= 15;
            Rockets--;
            if (buyed[Rockets])
            {
                buyButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);
                if (Rockets == StatController.selected) { select.text = "Selected"; }
                else { select.text = "Select"; }
                priceTag.gameObject.SetActive(false);
            }
            else 
            { 
                buyButton.gameObject.SetActive(true);
                selectButton.gameObject.SetActive(false);
                priceTag.gameObject.SetActive(true);
                priceTag.text = price[Rockets].ToString();
            }
        }
        cam.transform.position = new Vector3(x, 4, -11);
    }
    public void MoveRight()
    {
        if (cam.transform.position.x == 45)
        {
            x = 0;
            Rockets = 0;
            buyButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
            priceTag.gameObject.SetActive(false);
        }
        else
        {
            x += 15;
            Rockets++;
            if (buyed[Rockets])
            {
                buyButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);
                if (Rockets == StatController.selected) { select.text = "Selected"; }
                else { select.text = "Select"; }
                priceTag.gameObject.SetActive(false);
            }
            else 
            { 
                buyButton.gameObject.SetActive(true);
                selectButton.gameObject.SetActive(false);
                priceTag.gameObject.SetActive(true);
                priceTag.text = price[Rockets].ToString();
            }
        }
        cam.transform.position = new Vector3(x, 4, -11);
    }


    public void BuyNow()
    {
        if (StatController.Money >= price[Rockets])
        {
            switch (Rockets)
            {
                case 1:
                    buyed[Rockets] = true;
                    StatController.Money -= price[Rockets];
                    buyButton.gameObject.SetActive(false);
                    selectButton.gameObject.SetActive(true);
                    priceTag.gameObject.SetActive(false);
                    break;
                case 2:
                    if (buyed[Rockets - 1])
                    {
                        buyed[Rockets] = true;
                        StatController.Money -= price[Rockets];
                        buyButton.gameObject.SetActive(false);
                        selectButton.gameObject.SetActive(true);
                        priceTag.gameObject.SetActive(false);
                    }
                    break;
                case 3:
                    if (buyed[Rockets -1 ])
                    {
                        buyed[Rockets] = true;
                        StatController.Money -= price[Rockets];
                        buyButton.gameObject.SetActive(false);
                        selectButton.gameObject.SetActive(true);
                        priceTag.gameObject.SetActive(false);
                    }
                    break;
            }

            SaveAsBuyed();
            StatController.instance.Save();
            
        }
        else { Debug.Log("Not enough Money..."); notEnoughMoneySound.Play(); }
    }
    public void Select()
    {
        if (buyed[Rockets]) 
        { 
            StatController.selected = Rockets;
            select.text = "Selected";
            StatController.instance.Save();
        }
        else
        {
            Debug.Log("You don't own it");
        }
    }

    int boolToInt(bool val)
    {
        if (val)
            return 1;
        else
            return 0;
    }

    bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }
}
