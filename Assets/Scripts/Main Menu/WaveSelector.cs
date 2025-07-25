using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;   

public class WaveSelector : MonoBehaviour
{
    [SerializeField] private TMP_InputField waveInput;
    [SerializeField] private Button selectButton;

    private void Awake()
    {
        selectButton.onClick.AddListener(Select);
    }

    public void Select()
    {
        if (waveInput.text.Length > 0)
        {
            if (int.Parse(waveInput.text) <= StatController.WaveCompleted && int.Parse(waveInput.text) > 0)
            {
                StatController.Wave = int.Parse(waveInput.text);
                StatController.Instance.UpdateText();
            }
            else
            {
                StatController.Wave = StatController.WaveCompleted;
                StatController.Instance.UpdateText();
            }
        }
        else
        {
            StatController.Wave = StatController.WaveCompleted;
            StatController.Instance.UpdateText();
        }

        gameObject.SetActive(false);
    }
}
