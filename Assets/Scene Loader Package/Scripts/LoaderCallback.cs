/* 
    ------------------- Made by PatriSk33 -------------------
 */

using System.Collections;
using TMPro;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingText;

    private void Start()
    {
        StartCoroutine(StartLoaderCallback());
    }

    IEnumerator StartLoaderCallback()
    {
        loadingText.text = "Loading";

        yield return new WaitForSeconds(0.5f);
        loadingText.text = "Loading.";

        yield return new WaitForSeconds(0.5f);
        loadingText.text = "Loading..";

        yield return new WaitForSeconds(0.5f);
        loadingText.text = "Loading...";

        Loader.LoaderCallback();
    }
}
