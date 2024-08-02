using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPCHeadLookAt : MonoBehaviour
{
    [SerializeField] private Rig rig;
    [SerializeField] Transform headLookAtTransform;
    Transform lookAt;

    private bool isLookAtPosition = false;

    [SerializeField] float lookingTime = 5f;

    private void Start()
    {
        lookAt = headLookAtTransform;
    }
    private void Update()
    {
        float targetWeight = isLookAtPosition ? 1.0f : 0.0f;
        float lerpSpeed = 2f;

        rig.weight = Mathf.Lerp(rig.weight, targetWeight, lerpSpeed * Time.deltaTime);

        headLookAtTransform.position = lookAt.position;
    }

    public void LookAt(Transform LookAt)
    {
        isLookAtPosition = true;
        this.lookAt = LookAt;
        headLookAtTransform.transform.position = LookAt.position;
        Invoke("CloseLookingUp", lookingTime);
    }

    void CloseLookingUp()
    {
        isLookAtPosition = false;
    }
}
