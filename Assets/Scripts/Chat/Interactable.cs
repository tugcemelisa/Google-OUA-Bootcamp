
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private Sprite interactIcon;
    [SerializeField]
    private string InteractableInformation;

    public Sprite InteractIcon { get => interactIcon; }
    public string InteractableInformation1 { get => InteractableInformation; }

    public abstract void Interact(Transform interactorTransform);
}