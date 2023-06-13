using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using System;
using EasyUI.PickerWheelUI;

public class SpinningWheel : MonoBehaviour
{
    [Header("Spin Wheel")]
    [HideInInspector]public int spins;
    public Text spinButtonText, spinRechargeText;
    public Button spinButton;

    public static float secondsLeftToRefresh;
    [SerializeField] private PickerWheel pickerWheel;

    private bool isRefreshing;

    private void Update()
    {
        if (spins == 0)
        {
            secondsLeftToRefresh -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(secondsLeftToRefresh);
            spinRechargeText.text = time.ToString("hh':'mm':'ss");
            if (secondsLeftToRefresh < 0 && !isRefreshing)
            {
                isRefreshing = true;
                GetVirtualCurrencies();
            }
            spinButton.interactable = false;
        }
        else
        {
            spinButton.interactable = true;
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error occured while trying to spin. Error: " + error);
        isRefreshing = false;
    }

    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }

    public void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        spins = result.VirtualCurrency["SP"];
        secondsLeftToRefresh = result.VirtualCurrencyRechargeTimes["SP"].SecondsToRecharge;
        isRefreshing = false;
    }

    public void SpinTheWheel()
    {
        if (spins == 0)
        {
            Debug.Log("You can't spin!");
            return;
        }

        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "SP",
            Amount = 1
        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnSubtractSpinsSuccess, OnError);
    }

    void OnSubtractSpinsSuccess(ModifyUserVirtualCurrencyResult result)
    {
        spins--;
        spinButton.interactable = false;
        spinButtonText.text = "Spinning";

        pickerWheel.OnSpinStart(() =>  Debug.Log("Spin started..."));
        
        pickerWheel.OnSpinEnd(wheelPiece =>
        {
            Debug.Log("Spin ended:\nAmount: " + wheelPiece.Amount);
            spinButton.interactable = true;
            spinButtonText.text = "Spin";

            StatController.Money += wheelPiece.Amount;
            StatController.instance.Save();
            StatController.instance.UpdateText();
        });
        pickerWheel.Spin();
    }
}
