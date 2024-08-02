
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] InteractableUIElement[] interactableUIElements;

    public InteractableUIElement[] InteractableUIElements { get => interactableUIElements; }

    public abstract void Interact(Transform interactorTransform, KeyCode interactKey);

}

public enum InteractKeys
{
    Bargain = KeyCode.R,
    Talk = KeyCode.E,
    InteractAnimals = KeyCode.T,
    Accept = KeyCode.Y,
    Sit = KeyCode.E,
    NONE = KeyCode.None
}