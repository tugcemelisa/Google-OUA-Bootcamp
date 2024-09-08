using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractUIController : MonoBehaviourSingletonPersistent<InteractUIController>
{
    [SerializeField] List<InteractableUIElement> data;

    private void OnEnable()
    {
        //foreach (var item in data)
        //{
        //    item.enabled = false;
        //}
    }

    public void ManageInteractUI(InteractType typeToShow, InteractType typeToHide)
    {
        ShowInteractUI(typeToShow);
        HideInteractUI(typeToHide);
    }

    public void ShowInteractUI(InteractType type)
    {
        foreach (var item in data)
        {
            if (item.interactType == type)
            {
                item.enabled = true;
                break;
            }
        }
    }

    public void HideInteractUI(InteractType type)
    {
        foreach (var item in data)
        {
            if (item.interactType == type)
            {
                item.enabled = false;
                break;
            }
        }
    }
}

public enum InteractType
{
    E_ToTalk,
    MakeDeal,
    AfterDeal,
    E_ForOffer,  
    AcceptOffer,
    AlreadyAccepted,

    Accelerate,
    CollectMeat,  
    Sheare,
    AlreadySheared,
    Milk,
    AlreadyMilked,

    T_ToPet
}
