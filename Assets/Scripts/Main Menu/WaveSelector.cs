using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSelector : MonoBehaviour
{
    public InputField waveInput;

    public void Select()
    {
        if (waveInput.text.Length > 0)
        {
            if (int.Parse(waveInput.text) <= StatController.WaveCompleted && int.Parse(waveInput.text) > 0)
            {
                StatController.Wave = int.Parse(waveInput.text);
                StatController.instance.UpdateText();
            }
            else
            {
                StatController.Wave = StatController.WaveCompleted;
                StatController.instance.UpdateText();
            }
        }
        else
        {
            StatController.Wave = StatController.WaveCompleted;
            StatController.instance.UpdateText();
        }
    }
}
