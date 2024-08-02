using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviourSingletonPersistent<MenuManager>
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject otherCanvas;

    [SerializeField] private Animator menuAnimator;
    [SerializeField] private Animator otherMenuAnimator;

    [SerializeField] CinemachineVirtualCamera menuCam;
    [SerializeField] CinemachineVirtualCamera normalCam;


    [SerializeField] Animator playerAnimator;

    bool isOpen = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOpen = !isOpen;
            SetMenuActive(isOpen);
        }
    }

    public void SetMenuActive(bool set)
    {
        Cursor.lockState = set ? CursorLockMode.None : CursorLockMode.Locked;
        menuAnimator.SetBool("isOpen", set);
        otherMenuAnimator.SetBool("isOpen", !set);

        menuCam.Priority = set ? 15 : 10;

        normalCam.Priority = !set ? 15 : 10;

        playerAnimator.SetTrigger(set ? "IdleToSit" : "SitToIdle");

        if (isOpen)
        {
            StarterAssets.InputController.Instance.DisableInputs();
        }

    }

    public void StartMenu()
    {
        HelperController.Instance.ShowHelper(HelpType.FindLiveStock);
        menuCanvas.SetActive(true);
        otherCanvas.SetActive(true);
        SetMenuActive(true);
    }

    public void StartGameMenu(bool set)
    {
        HelperController.Instance.ShowHelper(HelpType.FindLiveStock);
        menuCanvas.SetActive(true);
        otherCanvas.SetActive(true);
        SetMenuActive(true);

        menuAnimator.SetBool("isOpen", set);
        otherMenuAnimator.SetBool("isOpen", !set);

        menuCam.Priority = set ? 15 : 10;

        normalCam.Priority = !set ? 15 : 10;

        //playerAnimator.SetTrigger("SitToIdle");

        if (isOpen)
        {
            StarterAssets.InputController.Instance.DisableInputs();
        }
    }
}
