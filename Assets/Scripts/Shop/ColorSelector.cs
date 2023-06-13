using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelector : MonoBehaviour
{
    public Material detailsMaterial;
    public Color newColorDetails;
    public SVGImage lockIcon;
    public int unlockableWave;
    public Text onScreenText;

    public void Start()
    {
        if (unlockableWave <= StatController.WaveCompleted && lockIcon != null)
        {
            lockIcon.gameObject.SetActive(false);
        }
    }

    public void ChangeColor()
    {
        if (unlockableWave <= StatController.WaveCompleted)
        {
            detailsMaterial.color = newColorDetails;
        }
        else
        {
            StopCoroutine(ShowText());
            StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        onScreenText.text = "Color unlocked at wave: " + unlockableWave;
        yield return new WaitForSeconds(1);
        onScreenText.text = "";
    }
}