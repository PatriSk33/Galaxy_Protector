using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexSetter : MonoBehaviour
{
    public static HexSetter Instance;

    public Material detailsMaterial;
    private Color newColorDetails;
    public InputField hexInput;
    public Text onScreenText;
    private int hexSetterBought = 0;  //0 = false      1 = true
    [HideInInspector] public string hexValue;
    public int cost;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        hexSetterBought = PlayerPrefs.GetInt("hexSetterBought", 0);
        
        if (PlayerPrefs.HasKey("HexValue"))
        {
            hexValue = PlayerPrefs.GetString("HexValue");
            SetHexColor(hexValue);
        }
    }

    private void ChangeColor()
    {
        detailsMaterial.color = newColorDetails;
    }

    public void SetHexColor(string hexvalue = null)
    {
        // Check if "#" is missing and add it if needed
        if(hexInput.text.Length > 0)
        {
            if (hexInput.text.Length < 7 && hexInput.text[0] != '#' && hexvalue == null)
            {
                hexInput.text = "#" + hexInput.text;
            }
        }

        if (hexvalue == null)
        {
            ColorUtility.TryParseHtmlString(hexInput.text, out newColorDetails);
            PlayerPrefs.SetString("HexValue", hexInput.text);
        }
        else
        {
            ColorUtility.TryParseHtmlString(hexvalue, out newColorDetails);
        }

        ChangeColor();
    }

    public void BuyHexSetter()
    {
        // If the color has already been bought, do nothing
        if (hexSetterBought == 1)
        {
            SetHexColor();
            return;
        }

        // If the player has enough currency, deduct 100 and mark the color as bought
        if (StatController.Money >= cost)
        {
            StatController.Money -= cost;
            hexSetterBought = 1;
            PlayerPrefs.SetInt("hexSetterBought", hexSetterBought);


            // Get the current colors of the input field
            ColorBlock colors = hexInput.colors;

            // Set the new normal color
            colors.normalColor = Color.white;

            // Apply the new colors to the input field
            hexInput.colors = colors;
        }
        else
        {
            StopCoroutine(ShowText());
            StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        onScreenText.text = "Custom color setter costs: " + cost;
        yield return new WaitForSeconds(1);
        onScreenText.text = "";
    }

}
