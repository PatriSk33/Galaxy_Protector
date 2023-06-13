using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rozhodovac : MonoBehaviour
{
    public GameObject[] spaceshipPrefab;

    private void Start()
    {
        ActivateSpaceship();
    }
    public void ActivateSpaceship()
    {
        for (int i = 0; i < spaceshipPrefab.Length; i++)
        {
            if (i == StatController.selected)
            {
                spaceshipPrefab[i].SetActive(true);
            }
            else
            {
                spaceshipPrefab[i].SetActive(false);
            }
        }
    }
}
