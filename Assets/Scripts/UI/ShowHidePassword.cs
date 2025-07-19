using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowHidePassword : MonoBehaviour
{
    public TMP_InputField inputField;
    public SVGImage eyeIcon;
    public Sprite openEye, closeEye;

    private bool isPasswordVisible = false;
    private TMP_InputField.InputType previousInputType;

    private void Awake()
    {
        OnButtonClicked();
    }

    public void OnButtonClicked()
    {
        isPasswordVisible = !isPasswordVisible;

        if (isPasswordVisible)
        {
            eyeIcon.sprite = openEye;
            previousInputType = inputField.inputType;
            inputField.inputType = TMP_InputField.InputType.Standard;
        }
        else
        {
            eyeIcon.sprite = closeEye;
            inputField.inputType = previousInputType;
        }

        inputField.ForceLabelUpdate();
    }
}