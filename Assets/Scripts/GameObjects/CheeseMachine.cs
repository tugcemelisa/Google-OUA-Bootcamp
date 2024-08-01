using UnityEngine;
using System.Collections;

public class CheeseMachine : Interactable
{

    [SerializeField] private ItemData milk;
    [SerializeField] private GameObject cheesePrefab;
    [SerializeField] private float workingDuration=2.5f;

    public override void Interact(Transform interactorTransform, KeyCode interactKey)
    {
        if (InventoryManager.Instance.HasItem(milk.Type, milk.count))
        {
            StartCoroutine(MakeCheese(workingDuration));
        }
    }

    IEnumerator MakeCheese(float duration)
    {
        SoundManager.Instance.PlaySound(VoiceType.MachineIndustrial, transform, transform.position);

        int expectedCheeseCount = InventoryManager.Instance.slots.Find(x => x.GetItem().ItemData.Type == milk.Type).count;
        if (expectedCheeseCount > 0)
        {
            Debug.Log(expectedCheeseCount);
            InventoryManager.Instance.RemoveItem(milk.Type, expectedCheeseCount);

            yield return new WaitForSeconds(duration * expectedCheeseCount);


            for (int i = 0; i < expectedCheeseCount; i++)
            {
                Instantiate(cheesePrefab, transform);
            }
        }
    }
}
