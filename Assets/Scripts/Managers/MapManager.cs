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

    [SerializeField] GameObject playerMapIcon;

    bool isOpen = false;

    private void Start()
    {
        minimapCameraOffset = miniMapCamera.transform.position;
        OpenMap(false);

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
        isOpen = set;

        StartCoroutine(UpdateCamera(!set));


        map.SetActive(set);
        miniMap.SetActive(!set);

        //If map open, then show the cursor
        Cursor.lockState = set ? CursorLockMode.None : CursorLockMode.Locked;
    }

    IEnumerator UpdateCamera(bool isCenter)
    {
        miniMapCamera.transform.position = isCenter ? minimapCameraOffset + player.transform.position : minimapCameraOffset;
        playerMapIcon.transform.position = player.transform.position + Vector3.up;

        miniMapCamera.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        miniMapCamera.gameObject.SetActive(false);
    }

}
