using Cinemachine;
using System.Collections;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    ParticleSystem _particleSystem;
    public CinemachineVirtualCamera cam;

    [SerializeField] Transform requestedTransform;

    private void OnEnable()
    {
        PlayerSimulationController.OnPlayerBridgeBuy += () => StartCoroutine(BuildBridge());
    }
    private void OnDisable()
    {
        PlayerSimulationController.OnPlayerBridgeBuy -= () => StartCoroutine(BuildBridge());
    }

    private void Start()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private IEnumerator BuildBridge()
    {
        _particleSystem.Play();
        cam.Priority = 16;

        yield return new WaitForSeconds(3f);

        transform.position = requestedTransform.position;
        transform.rotation = requestedTransform.rotation;
        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        cam.Priority = 1;
    }
}
