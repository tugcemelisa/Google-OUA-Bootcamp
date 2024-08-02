using UnityEngine;
using System.Collections;

public class CheeseMachine : Interactable
{
    [SerializeField] private float conveyorSpeed = 3f;
    [SerializeField]
    private Material conveyorMat;

    [SerializeField] private ItemData milk;
    [SerializeField] private GameObject milkObj;
    [SerializeField] private GameObject cheesePrefab;
    [SerializeField] private float workingDuration = 2.5f;

    [SerializeField] private Transform outputSpawnPos;
    [SerializeField] private Vector3 throwingDir = Vector3.up;
    [SerializeField] private float throwingPower = 30f;

    bool isMachineBusy = false;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;

    [SerializeField]
    private ParticleSystem particle;

    public override void Interact(Transform interactorTransform, KeyCode interactKey)
    {
        if (isMachineBusy)
        {
            print("Machine is busy");
            return;
        }
        if (InventoryManager.Instance.HasItem(milk.Type, milk.count))
        {
            isMachineBusy = true;
            StartCoroutine(MakeCheese(workingDuration));
        }
    }

    IEnumerator MakeCheese(float duration)
    {

        InventoryManager.Instance.RemoveItem(milk.Type, 1);
        InventoryManager.Instance.ShowInventory();

        SoundManager.Instance.PlaySound(VoiceType.MachineIndustrial, transform, transform.position);
        particle.Play();

        milkObj.SetActive(true);
        milkObj.transform.position = startPosition.position;
        Vector3 dir = endPosition.position - startPosition.position;
        dir /= duration;
        float timer = 0f;
        while (timer <= duration)
        {
            yield return new WaitForFixedUpdate();
            milkObj.transform.position += dir * Time.deltaTime;
            timer += Time.deltaTime;
            conveyorMat.mainTextureOffset += new Vector2(0, conveyorSpeed * Time.deltaTime);
        }

        milkObj.SetActive(false);
        particle.Stop();

        GameObject obj = Instantiate(cheesePrefab, outputSpawnPos.position, Quaternion.identity);

        obj.GetComponent<Rigidbody>().AddForce(throwingDir * throwingPower, ForceMode.Impulse);

        isMachineBusy = false;
    }

}
