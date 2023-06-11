using System.Collections;
using UnityEngine;

public class TutorialEnabler : MonoBehaviour
{
    public GameObject tutorialPanel;

    private void Awake()
    {
        StartCoroutine(OpenAndClose());
    }

    IEnumerator OpenAndClose()
    {
        tutorialPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        tutorialPanel.SetActive(false);
    }
}
