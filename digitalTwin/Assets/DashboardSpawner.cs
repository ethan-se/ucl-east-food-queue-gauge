using UnityEngine;

public class DashboardSpawner : MonoBehaviour
{
    public GameObject dashboardPrefab;

    private GameObject spawnedDashboard;
    private bool canShow = false;

    public void EnableDashboard()
    {
        canShow = true;
        Debug.Log("Dashboard enabled (AR object placed).");
    }

    public void ToggleDashboard()
    {
        if (!canShow)
        {
            Debug.Log("Dashboard not enabled yet. Place the AR object first.");
            return;
        }

        if (spawnedDashboard == null)
        {
            spawnedDashboard = Instantiate(dashboardPrefab);
            spawnedDashboard.SetActive(true);
        }
        else
        {
            spawnedDashboard.SetActive(!spawnedDashboard.activeSelf);
        }
    }
}
