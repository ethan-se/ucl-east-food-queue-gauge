using UnityEngine;

public class OpenDashboards3D : MonoBehaviour
{
    public GameObject dashboardsCanvas;   // assign DashboardsCanvas in Inspector
    private Camera arCamera;

    private void Start()
    {
        arCamera = Camera.main;

        if (dashboardsCanvas != null)
        {
            dashboardsCanvas.SetActive(false);    // start hidden
        }
    }

    private void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;

        Ray ray = arCamera.ScreenPointToRay(touch.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform && dashboardsCanvas != null)
            {
                dashboardsCanvas.SetActive(true);
            }
        }
    }
}
