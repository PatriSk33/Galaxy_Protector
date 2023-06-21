using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public GameObject loadingPanel;

    private void Start()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(2);
        loadingPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
