using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private Loader.Scene sceneToLoad = Loader.Scene.MainMenuScene;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if(SceneLoader.i != null)
            {
                SceneLoader.i.Load(sceneToLoad);  
            }
            else
            {
                Loader.Load(sceneToLoad);
            }
        });
    }
}
