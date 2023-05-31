using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class ResetPlayerData : MonoBehaviour
{
    public GameObject SettingsPanel;
    public GameObject firstRegisterPanel;
    public GameObject confirmationPanel;

    public void OnConfirmButtonClick()
    {
        confirmationPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        firstRegisterPanel.SetActive(true);

        // Reset PlayerPrefs
        ResetAllPlayerPrefs();

        // Delete all player data in PlayFab
        ClearPlayerDataVariables();
    }

    public void OnCancelButtonClick()
    {
        confirmationPanel.SetActive(false);
        Debug.Log("Player data deletion canceled.");
    }

    public void ShowConfirmationPopup()
    {
        confirmationPanel.SetActive(true);
    }

    private void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();

        // Save the changes
        PlayerPrefs.Save();

        Debug.Log("All PlayerPrefs have been reset.");
    }

    private void ClearPlayerDataVariables()
    {
        // List of keys to remove
        List<string> keysToRemove = new List<string>()
        {
            "Guns",
            "Money",
            "WaveCompleted"
        };

        // Update the user data to remove specific variables
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>(),
            KeysToRemove = keysToRemove
        },
        result =>
        {
            Debug.Log("Player data variables cleared successfully.");
        },
        error =>
        {
            Debug.LogError("Failed to clear player data variables: " + error.GenerateErrorReport());
        });
    }
}
