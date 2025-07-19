using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ResetPlayerData : MonoBehaviour
{
    public GameObject SettingsPanel;
    public GameObject firstRegisterPanel;
    public GameObject confirmationPanel;

    private string dataPath => Path.Combine(Application.persistentDataPath, "playerData.json");

    public void OnConfirmButtonClick()
    {
        confirmationPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        firstRegisterPanel.SetActive(true);

        ResetPlayerProgress();
        ResetAllPlayerPrefs();

        Debug.Log("Player progress reset, personal info retained.");
    }

    public void OnCancelButtonClick()
    {
        confirmationPanel.SetActive(false);
        Debug.Log("Player data reset canceled.");
    }

    public void ShowConfirmationPopup()
    {
        confirmationPanel.SetActive(true);
    }

    private void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    private void ResetPlayerProgress()
    {
        if (!File.Exists(dataPath))
        {
            Debug.LogWarning("No player data file found.");
            return;
        }

        string json = File.ReadAllText(dataPath);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);

        // Clear progress-related fields but keep email, password, displayName
        playerData.stats = new PlayerStats(); // resets ints to 0
        playerData.guns = new List<Gun>();    // empty list

        // Save updated data
        string updatedJson = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(dataPath, updatedJson);

        Debug.Log("Player progress cleared but personal info retained.");
    }
}
