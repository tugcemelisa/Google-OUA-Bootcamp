using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractUIOptionPlaceHolder : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactableText;
    [SerializeField] Image interactIcon;

    private InteractableUIElement interactableUIElement;

    public void SetOption(InteractableUIElement interactableUIElement)
    {
        this.interactableUIElement = interactableUIElement;
        interactableText.text = interactableUIElement.InteractableText;
        interactIcon.sprite = interactableUIElement.InteractIcon;
    }
}



