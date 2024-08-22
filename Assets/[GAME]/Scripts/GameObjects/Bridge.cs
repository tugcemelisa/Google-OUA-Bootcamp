using Cinemachine;
using System.Collections;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    public CinemachineVirtualCamera cam;

    [SerializeField] Transform requestedTransform;

    [SerializeField]
    private GameObject newMeadowButton;
    [SerializeField]
    private GameObject buyMeadowButton;

    private void OnEnable()
    {
        PlayerSimulationController.OnPlayerBridgeBuy += () => StartCoroutine(BuildBridge());
    }
    private void OnDisable()
    {
        PlayerSimulationController.OnPlayerBridgeBuy -= () => StartCoroutine(BuildBridge());
    }

    private IEnumerator BuildBridge()
    {
        newMeadowButton.SetActive(true);
        buyMeadowButton.SetActive(false);
        _particleSystem.Play();
        cam.Priority = 16;

        yield return new WaitForSeconds(3f);

        requestedTransform.parent = null;
        transform.position = requestedTransform.position;
        transform.rotation = requestedTransform.rotation;
        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        cam.Priority = 1;
    }
}
