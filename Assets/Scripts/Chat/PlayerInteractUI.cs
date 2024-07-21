using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractableUI : MonoBehaviour
{
    [SerializeField] PlayerInteract playerInteract;
    [SerializeField] GameObject interactUI;
    [SerializeField] TextMeshProUGUI interactableText;
    [SerializeField] Image interactIcon;

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
        interactableText.text = interactable.InteractableInformation1;
        interactIcon.sprite = interactable.InteractIcon;
        interactUI.SetActive(true);
    }
    void Hide()
    {
        interactUI.SetActive(false);
    }
}