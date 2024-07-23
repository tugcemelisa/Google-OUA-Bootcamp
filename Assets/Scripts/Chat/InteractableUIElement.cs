using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableUIElement : MonoBehaviour
{
    [SerializeField] string OptionName;
    [SerializeField] string interactableText;
    [SerializeField] Sprite interactIcon;
    [SerializeField] InteractKeys interactKey;
    //[SerializeField] IconType interactIconType;

    [SerializeField] InteractableUIElement activateWhenDisable;

    public void Disable(bool shouldActivateOtherOne)
    {
        if (activateWhenDisable)
            activateWhenDisable.enabled = shouldActivateOtherOne;
        this.enabled = false;
    }

    public string InteractableText { get => interactableText; set => interactableText = value; }
    public Sprite InteractIcon { get => interactIcon; set => interactIcon = value; }
    //public IconType InteractIconType { get => interactIconType; set => interactIconType = value; }

    public InteractKeys InteractKey { get => interactKey; }
}