using UnityEngine;
using UnityEngine.UI;
using System;
using EasyUI.PickerWheelUI;
using TMPro;

public class SpinningWheel : MonoBehaviour
{
    [Header("Spin Wheel")]
    [SerializeField] private int maxDailySpins = 1;
    [SerializeField] private TMP_Text spinButtonText, spinRechargeText;
    [SerializeField] private Button spinButton;

    [SerializeField] private PickerWheel pickerWheel;

    private int spinsLeft;
    private float secondsLeftToRefresh;
    private bool isRefreshing;

    private const string SpinsLeftKey = "SpinsLeft";
    private const string SpinsRefreshTimeKey = "SpinsRefreshTime";

    private void Awake()
    {
        spinButton.onClick.AddListener(SpinTheWheel);
    }

    private void Start()
    {
        LoadSpinData();
        UpdateUI();
    }

    private void Update()
    {
        if (spinsLeft <= 0)
        {
            if (secondsLeftToRefresh > 0)
            {
                secondsLeftToRefresh -= Time.deltaTime;
                TimeSpan time = TimeSpan.FromSeconds(secondsLeftToRefresh);
                spinRechargeText.text = time.ToString("hh':'mm':'ss");
                spinButton.interactable = false;
            }
            else if (!isRefreshing)
            {
                RefreshSpins();
            }
        }
        else
        {
            spinRechargeText.text = "";
            spinButton.interactable = true;
        }
    }

    public void SpinTheWheel()
    {
        if (spinsLeft <= 0)
        {
            Debug.Log("No spins left!");
            return;
        }

        spinsLeft--;
        SaveSpinData();
        UpdateUI();

        spinButton.interactable = false;
        spinButtonText.text = "Spinning";

        pickerWheel.OnSpinStart(() => Debug.Log("Spin started..."));

        pickerWheel.OnSpinEnd(wheelPiece =>
        {
            Debug.Log("Spin ended:\nAmount: " + wheelPiece.Amount);

            // Add to player's money locally
            StatController.Money += wheelPiece.Amount;
            StatController.Instance.Save();
            StatController.Instance.UpdateText();

            spinButton.interactable = spinsLeft > 0;
            spinButtonText.text = "Spin";
        });

        pickerWheel.Spin();
    }

    private void RefreshSpins()
    {
        spinsLeft = maxDailySpins;
        secondsLeftToRefresh = 24 * 60 * 60; // 24 hours cooldown
        isRefreshing = false;

        SaveSpinData();
        UpdateUI();

        Debug.Log("Spins refreshed!");
    }

    private void SaveSpinData()
    {
        PlayerPrefs.SetInt(SpinsLeftKey, spinsLeft);

        // Save next refresh time as UNIX timestamp
        double nextRefreshTimestamp = DateTime.UtcNow.AddSeconds(secondsLeftToRefresh).ToOADate();
        PlayerPrefs.SetString(SpinsRefreshTimeKey, nextRefreshTimestamp.ToString());

        PlayerPrefs.Save();
    }

    private void LoadSpinData()
    {
        spinsLeft = PlayerPrefs.GetInt(SpinsLeftKey, maxDailySpins);

        if (PlayerPrefs.HasKey(SpinsRefreshTimeKey))
        {
            double refreshOADate;
            if (double.TryParse(PlayerPrefs.GetString(SpinsRefreshTimeKey), out refreshOADate))
            {
                DateTime nextRefreshTime = DateTime.FromOADate(refreshOADate);
                secondsLeftToRefresh = (float)(nextRefreshTime - DateTime.UtcNow).TotalSeconds;
                if (secondsLeftToRefresh < 0)
                    secondsLeftToRefresh = 0;
            }
            else
            {
                secondsLeftToRefresh = 0;
            }
        }
        else
        {
            secondsLeftToRefresh = 0;
        }

        isRefreshing = false;
    }

    private void UpdateUI()
    {
        spinButtonText.text = "Spin";
        spinButton.interactable = spinsLeft > 0;

        if (spinsLeft > 0)
        {
            spinRechargeText.text = "";
        }
        else if (secondsLeftToRefresh > 0)
        {
            TimeSpan time = TimeSpan.FromSeconds(secondsLeftToRefresh);
            spinRechargeText.text = time.ToString("hh':'mm':'ss");
        }
        else
        {
            spinRechargeText.text = "Refreshing...";
        }
    }
}
