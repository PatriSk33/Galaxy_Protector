using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClosePanel : MonoBehaviour
{
    public void OpenPanel(GameObject panelGameobject)
    {
        panelGameobject.SetActive(true);
    }

    public void ClosePanel(GameObject panelGameobject)
    {
        panelGameobject.SetActive(false);
    }
}
