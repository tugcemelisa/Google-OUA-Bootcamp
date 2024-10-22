using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractableUIElement : MonoBehaviour
{
    [SerializeField] string OptionName;
    [SerializeField] string interactableText;
    [SerializeField] Sprite interactIcon;
    [SerializeField] InteractKeys interactKey;
    public InteractType interactType;
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

    public void EnableIt(float time)
    {
        Invoke("Enable", time);
    }

    void Enable()
    {
        this.enabled = true;
    }
}