using UnityEngine;

public class ButtonMovement : MonoBehaviour
{
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Vector3 touchPosition = Input.GetTouch(0).position;
            if (touchPosition.x > Screen.width * 0.5f)
            {
                Player.move = new Vector2(1, 0);
            }
            else
            {
                Player.move = new Vector2(-1, 0);
            }
        }
        else
        {
            Player.move = new Vector2(0, 0);
        }
    }
}
