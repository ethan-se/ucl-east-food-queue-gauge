using UnityEngine;

public class DashboardManager : MonoBehaviour
{
    [Header("Dashboard Panels (3 locations)")]
    public GameObject dashboardPool;
    public GameObject dashboardCafe;
    public GameObject dashboardCanteen;

    private int index = 0;

    private void Start()
    {
        ShowDashboard(0);   // show Pool Street first
    }

    public void ShowNextDashboard()
    {
        index = (index + 1) % 3;
        ShowDashboard(index);
    }

    public void ShowPreviousDashboard()
    {
        index = (index - 1 + 3) % 3;
        ShowDashboard(index);
    }

    private void ShowDashboard(int i)
    {
        dashboardPool.SetActive(i == 0);
        dashboardCafe.SetActive(i == 1);
        dashboardCanteen.SetActive(i == 2);
    }
}

