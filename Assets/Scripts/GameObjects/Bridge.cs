using Cinemachine;
using System.Collections;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    ParticleSystem _particleSystem;
    public CinemachineVirtualCamera cam;
    private Vector3 requestedTransform;

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
        requestedTransform = new Vector3(42.43f, -1f, 3.94f);
    }

    private IEnumerator BuildBridge()
    {
        _particleSystem.Play();
        cam.Priority = 16;

        yield return new WaitForSeconds(3f);

        transform.position = requestedTransform;
        transform.rotation = new Quaternion(-90f, transform.rotation.y, transform.rotation.z, 1);
        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        cam.Priority = 1;
    }
}
