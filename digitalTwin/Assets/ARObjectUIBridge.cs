using UnityEngine;

public class ARObjectUIBridge : MonoBehaviour
{
    [Header("Dashboard Prefab")]
    public GameObject dashboardPrefab;

    private static GameObject dashboardInstance;

    public void OnOpenDashboardPressed()
    {
        if (dashboardInstance == null)
        {
            dashboardInstance = Instantiate(dashboardPrefab);
        }

        var nav = dashboardInstance.GetComponentInChildren<DashboardNavigation>(true);
        if (nav == null)
        {
            Debug.LogError("DashboardNavigation not found in dashboard prefab.");
            return;
        }

        nav.RegisterPlacedARObject(gameObject);
        nav.OpenDashboard();
    }
}
