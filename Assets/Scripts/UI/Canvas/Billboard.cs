using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Canvas _canvas;
    private Transform _mainCameraTransform;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = Camera.main;

        _mainCameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + _mainCameraTransform.forward);
    }
}
