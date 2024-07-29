using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviourSingletonPersistent<MapManager>
{
    [SerializeField] GameObject map;

    [SerializeField] GameObject miniMap;

    [SerializeField] Camera miniMapCamera;

    Vector3 minimapCameraOffset;

    [SerializeField] PlayerSimulationController player;

    bool isOpen = false;

    private void Start()
    {
        OpenMap(false);

        minimapCameraOffset = miniMapCamera.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isOpen = !isOpen;
            OpenMap(isOpen);
        }
    }

    public void OpenMap(bool set)
    {
        StartCoroutine(UpdateCamera());


        map.SetActive(set);
        miniMap.SetActive(!set);

        //If map open, then show the cursor
        Cursor.lockState = set ? CursorLockMode.None : CursorLockMode.Locked;
    }

    IEnumerator UpdateCamera()
    {
        miniMapCamera.transform.position = minimapCameraOffset + player.transform.position;

        miniMapCamera.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        miniMapCamera.gameObject.SetActive(false);
    }

}
