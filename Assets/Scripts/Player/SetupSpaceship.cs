using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupSpaceship : MonoBehaviour
{
    public GameObject[] spaceshipPrefabs;

    private void Start()
    {
        ActivateSpaceship();
    }
    public void ActivateSpaceship()
    {
        for (int i = 0; i < spaceshipPrefabs.Length; i++)
        {
            if (i == StatController.selected)
            {
                spaceshipPrefabs[i].SetActive(true);
            }
            else
            {
                spaceshipPrefabs[i].SetActive(false);
            }
        }
    }
}
