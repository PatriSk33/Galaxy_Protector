using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public static ShopController instance;

    public Camera cam;
    int x;

    int Rockets;
    public static bool[] buyed = { true, false, false, false};
    public int[] price = new int[4];

    //Textovky
    public Text selectButtonText, priceTag;
    public Button buyButton;
    public Button selectButton;

    //Audio
    public AudioSource notEnoughMoneySound;

    //Lock icon
    public GameObject lockIcon;

    private void Awake()
    {
        instance = this;

        Rockets = 0;
        x = 0;
        priceTag.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);
        lockIcon.gameObject.SetActive(false);

        selectButtonText.text = (Rockets == StatController.selected) ? "Selected" : "Select";
    }

    private void Update()
    {
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(x, 4, -11), Time.deltaTime * 100);
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
                selectButtonText.text = (Rockets == StatController.selected) ? "Selected" : "Select";
            }
            else 
            { 
                buyButton.gameObject.SetActive(true);
                selectButton.gameObject.SetActive(false);
                priceTag.gameObject.SetActive(true);
                priceTag.text = price[Rockets].ToString();
            }

            if ((Rockets * 25) > StatController.WaveCompleted)
            {
                priceTag.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(false);
                lockIcon.gameObject.SetActive(true);
            }
            else
            {
                lockIcon.gameObject.SetActive(false);
            }
        }
        else if (Rockets != 0)
        {
            x -= 15;
            Rockets--;
            if (buyed[Rockets])
            {
                buyButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);
                selectButtonText.text = (Rockets == StatController.selected) ? "Selected" : "Select";
                priceTag.gameObject.SetActive(false);
            }
            else 
            { 
                buyButton.gameObject.SetActive(true);
                selectButton.gameObject.SetActive(false);
                priceTag.gameObject.SetActive(true);
                priceTag.text = price[Rockets].ToString();
            }

            if ((Rockets * 25) > StatController.WaveCompleted && Rockets != 0)
            {
                priceTag.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(false);
                lockIcon.gameObject.SetActive(true);
            }
            else
            {
                lockIcon.gameObject.SetActive(false);
            }
        }
    }

    public void MoveRight()
    {
        if (cam.transform.position.x == 45)
        {
            x = 0;
            Rockets = 0;
            buyButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
            selectButtonText.text = (Rockets == StatController.selected) ? "Selected" : "Select";
            priceTag.gameObject.SetActive(false);
            lockIcon.gameObject.SetActive(false);
        }
        else if(Rockets != 3)
        {
            x += 15;
            Rockets++;
            if (buyed[Rockets])
            {
                buyButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);
                selectButtonText.text = (Rockets == StatController.selected) ? "Selected" : "Select";
                priceTag.gameObject.SetActive(false);
            }
            else
            { 
                buyButton.gameObject.SetActive(true);
                selectButton.gameObject.SetActive(false);
                priceTag.gameObject.SetActive(true);
                priceTag.text = price[Rockets].ToString();
            }

            if ((Rockets * 25) > StatController.WaveCompleted)
            { 
                priceTag.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(false);
                lockIcon.gameObject.SetActive(true);
            }
            else
            {
                lockIcon.gameObject.SetActive(false);
            }
        }
    }


    public void BuyNow()
    {
        if (PlayfabManager.clientConnected)
        {
            if (StatController.Money < price[Rockets])
            {
                Debug.Log("Not enough money...");
                notEnoughMoneySound.Play();
                return;
            }

            if (buyed[Rockets - 1] && (Rockets * 25) >= StatController.WaveCompleted )
            {
                buyed[Rockets] = true;
                StatController.Money -= price[Rockets];

                buyButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);
                priceTag.gameObject.SetActive(false);
                selectButtonText.text = "Select";

                StatController.instance.Save();
                PlayfabManager.Instance.SaveGuns();

                LevelPlayAds.Instance.ShowFullSizeAd();
            }
        }
    }

    public void Select()
    {
        if (buyed[Rockets]) 
        { 
            StatController.selected = Rockets;
            selectButtonText.text = "Selected";
            StatController.instance.Save();
            StatController.instance.UpdateStats();
        }
        else
        {
            Debug.Log("You don't own it");
        }
    }
}
