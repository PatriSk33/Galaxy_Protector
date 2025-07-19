/* 
    ------------------- Made by PatriSk33 -------------------
 */

using System.Collections;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader i { private set; get; }

    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime;

    private void Awake()
    {
        i = this;
    }

    public void Load(Loader.Scene targetScene)
    {
        StartCoroutine(LoadLevelCoroutine(targetScene));
    }

    IEnumerator LoadLevelCoroutine(Loader.Scene targetScene)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        Loader.Load(targetScene);
    }
}
