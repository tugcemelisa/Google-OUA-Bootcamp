using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractableUI : MonoBehaviour
{
    [SerializeField] PlayerInteract playerInteract;
    [SerializeField] GameObject interactableUI;
    [SerializeField] InteractUIOptionPlaceHolder[] interactableUIPlaceHolders;

    Interactable interactable;
    Interactable previousInteractable;
    private void Start()
    {
        playerInteract = PlayerInteract.Instance;
        print(playerInteract.gameObject);
    }
    private void Update()
    {
        var interactable = playerInteract.GetInteractable();
        if (interactable == null)
        {
            this.previousInteractable = null;
            Hide();
        }
        else
        {
            this.interactable = interactable;
            Show();
        }
    }

    void Show()
    {
        if (interactable == previousInteractable)
        {
            return;
        }
        previousInteractable = interactable;

        SetInteractableUIElements(previousInteractable);

        interactableUI.SetActive(true);
    }
    void Hide()
    {
        interactableUI.SetActive(false);
    }

    void SetInteractableUIElements(Interactable interactable)
    {
        foreach (var element in interactableUIPlaceHolders)
        {
            element.gameObject.SetActive(false);
        }

        var UIElements = interactable.InteractableUIElements;
        int i = 0;
        foreach (var element in UIElements)
        {
            var placeHolder = interactableUIPlaceHolders[i++];
            placeHolder.SetOption(element);
            placeHolder.gameObject.SetActive(true);
        }
    }
}