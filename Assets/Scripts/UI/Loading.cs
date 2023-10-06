using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
