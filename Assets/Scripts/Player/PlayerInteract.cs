using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviourSingletonPersistent<PlayerInteract>
{
    [SerializeField] float interactRange = 1.5f;
    [SerializeField] Transform interactor;//For Head

    void Update()
    {
        KeyCode keyCode = SelectKey();
        if (keyCode == KeyCode.None)
        {
            return;
        }

        var interactable = GetInteractable();
        if (interactable) { interactable.Interact(interactor, keyCode); }

    }

    private KeyCode SelectKey()
    {
        if (Input.GetKeyDown(KeyCode.E)) { return KeyCode.E; }
        if (Input.GetKeyDown(KeyCode.R)) { return KeyCode.R; }
        if (Input.GetKeyDown(KeyCode.T)) { return KeyCode.T; }
        if (Input.GetKeyDown(KeyCode.Y)) { return KeyCode.Y; }
        return KeyCode.None;
    }

    public Interactable GetInteractable()
    {
        List<Interactable> interactables = new List<Interactable>();

        var cols = Physics.OverlapSphere(transform.position, interactRange);
        foreach (var item in cols)
        {
            var interactable = item.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactables.Add(interactable);
            }
        }

        if (interactables.Count < 1)
            return null;
            

        Interactable closestInteractable = interactables[0];
        float closestDistance = Vector3.Distance(transform.position, closestInteractable.transform.position);
        foreach (var item in interactables)
        {
            var distance = Vector3.Distance(item.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestInteractable = item;
                closestDistance = distance;
            }
        }
        return closestInteractable;
    }
}
