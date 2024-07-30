using UnityEngine;
using System.Collections;

public class CheeseMachine : Interactable
{
    [SerializeField] private ItemData milk;
    [SerializeField] private GameObject cheesePrefab;

    public override void Interact(Transform interactorTransform, KeyCode interactKey)
    {
        if (InventoryManager.Instance.HasItem(milk.Type, milk.count))
        {
            StartCoroutine(MakeCheese(1.5f));
        }
    }

    IEnumerator MakeCheese(float duration)
    {
        int expectedCheeseCount = InventoryManager.Instance.slots.Find(x => x.GetItem().ItemData.Type == milk.Type).count;
        if(expectedCheeseCount > 0)
        {
            Debug.Log(expectedCheeseCount);

            yield return new WaitForSeconds(duration * expectedCheeseCount);

            InventoryManager.Instance.RemoveItem(milk.Type, expectedCheeseCount);

            for (int i = 0; i < expectedCheeseCount; i++)
            {
                Instantiate(cheesePrefab, transform);
            }
        }
    }
}
