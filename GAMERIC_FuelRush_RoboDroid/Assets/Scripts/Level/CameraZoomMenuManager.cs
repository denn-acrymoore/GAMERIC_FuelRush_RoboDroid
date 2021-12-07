using UnityEngine;
using UnityEngine.UI;

public class CameraZoomMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject cameraZoomOutButton;
    [SerializeField] private GameObject cameraZoomInButton;
    [SerializeField] private float zoomRadius = 3f;
    [SerializeField] private float zoomSpeed = 12f;

    private Camera mainCamera;
    private bool isZoomIn = false;
    private Vector3 zoomInPoint;
    private Vector3 zoomOutPoint;

    private void Start()
    {
        mainCamera = Camera.main;
        zoomOutPoint = mainCamera.transform.position;
        zoomInPoint = zoomOutPoint + Vector3.forward * zoomRadius + Vector3.down * zoomRadius;
    }

    private void Update()
    {
        if (isZoomIn)
        {
            if (Vector3.Distance(mainCamera.transform.position, zoomInPoint) > 0.05)
            {
                Vector3 dir = (zoomInPoint - mainCamera.transform.position).normalized;
                mainCamera.transform.Translate(dir * Time.deltaTime * zoomSpeed, Space.World);
            }
            else
            {
                mainCamera.transform.position = zoomInPoint;
            }
        }
        else
        {
            if (Vector3.Distance(mainCamera.transform.position, zoomOutPoint) > 0.05)
            {
                Vector3 dir = (zoomOutPoint - mainCamera.transform.position).normalized;
                mainCamera.transform.Translate(dir * Time.deltaTime * zoomSpeed, Space.World);
            }
            else
            {
                mainCamera.transform.position = zoomOutPoint;
            }
        }
    }

    public void ZoomIn()
    {
        isZoomIn = true;
        cameraZoomInButton.SetActive(false);
        cameraZoomOutButton.SetActive(true);
    }

    public void ZoomOut()
    {
        isZoomIn = false;
        cameraZoomInButton.SetActive(true);
        cameraZoomOutButton.SetActive(false);
    }
}
