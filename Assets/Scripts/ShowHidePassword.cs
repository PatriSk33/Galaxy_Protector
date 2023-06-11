using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class ShowHidePassword : MonoBehaviour
{
    public InputField inputField;
    public SVGImage eyeIcon;
    public Sprite openEye, closeEye;

    private bool isPasswordVisible = false;
    private InputField.InputType previousInputType;

    public void OnButtonClicked()
    {
        isPasswordVisible = !isPasswordVisible;

        if (isPasswordVisible)
        {
            eyeIcon.sprite = openEye;
            previousInputType = inputField.inputType;
            inputField.inputType = InputField.InputType.Standard;
        }
        else
        {
            eyeIcon.sprite = closeEye;
            inputField.inputType = previousInputType;
        }

        inputField.ForceLabelUpdate();
    }
}