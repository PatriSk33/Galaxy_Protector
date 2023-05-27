using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSelector : MonoBehaviour
{
    public InputField waveInput;

    public void Select()
    {
        if (StatController.Wave <= StatController.WaveCompleted && int.Parse(waveInput.text) > 0) {
            StatController.Wave = int.Parse(waveInput.text);
        }
    }
}
