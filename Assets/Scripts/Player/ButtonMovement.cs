using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonMovement : MonoBehaviour
{
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Vector3 touchPosition = Input.GetTouch(0).position;
            if (touchPosition.x > Screen.width * 0.5f)
            {
                PlayerMovement.move = new Vector2(1, 0);
            }
            else
            {
                PlayerMovement.move = new Vector2(-1, 0);
            }
        }
        else
        {
            PlayerMovement.move = new Vector2(0, 0);
        }
    }
}
