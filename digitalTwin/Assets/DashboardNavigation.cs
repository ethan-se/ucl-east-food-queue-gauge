using UnityEngine;
using UnityEngine.Events;

public class DashboardNavigation : MonoBehaviour
{
    [Header("Main UI Roots")]
    public GameObject dashboardUIRoot;   // whole dashboard UI (Screen Space)
    public GameObject arUIRootOptional;  // optional: other AR UI you want hidden when dashboard shows

    [Header("Dashboard Pages (Panels)")]
    public GameObject[] dashboards;      // size 3: Dashboard_0, Dashboard_1, Dashboard_2

    [Header("AR Object Handling")]
    public bool hideARObjectWhenDashboardOpen = true;
    private GameObject currentPlacedARObject; // set by AR object when spawned

    private int index = 0;

    private void Start()
    {
        // Start in AR mode
        if (dashboardUIRoot != null) dashboardUIRoot.SetActive(false);

        ShowDashboardPage(0);
    }

    // Called by the AR object when it spawns (tap-to-place)
    public void RegisterPlacedARObject(GameObject arObject)
    {
        currentPlacedARObject = arObject;
    }

    // ---------- Open/Close Dashboard ----------
    public void OpenDashboard()
    {
        if (hideARObjectWhenDashboardOpen) SetARObjectVisible(false);

        if (arUIRootOptional != null) arUIRootOptional.SetActive(false);
        if (dashboardUIRoot != null) dashboardUIRoot.SetActive(true);

        ShowDashboardPage(index);
    }

    public void CloseDashboard()
    {
        if (dashboardUIRoot != null) dashboardUIRoot.SetActive(false);

        if (arUIRootOptional != null) arUIRootOptional.SetActive(true);
        if (hideARObjectWhenDashboardOpen) SetARObjectVisible(true);
    }

    // ---------- Page Switching ----------
    public void NextDashboard()
    {
        if (dashboards == null || dashboards.Length == 0) return;

        index = (index + 1) % dashboards.Length;
        ShowDashboardPage(index);
    }

    public void PreviousDashboard()
    {
        if (dashboards == null || dashboards.Length == 0) return;

        index = (index - 1 + dashboards.Length) % dashboards.Length;
        ShowDashboardPage(index);
    }

    public void ShowDashboardPage(int i)
    {
        if (dashboards == null || dashboards.Length == 0) return;

        index = Mathf.Clamp(i, 0, dashboards.Length - 1);

        for (int d = 0; d < dashboards.Length; d++)
        {
            if (dashboards[d] != null)
                dashboards[d].SetActive(d == index);
        }
    }

    // ---------- Utility ----------
    private void SetARObjectVisible(bool visible)
    {
        if (currentPlacedARObject == null) return;

        // Hide/show all renderers (keeps object in place)
        var renderers = currentPlacedARObject.GetComponentsInChildren<Renderer>(true);
        foreach (var r in renderers) r.enabled = visible;

        // Optional: hide world-space UI too
        var canvases = currentPlacedARObject.GetComponentsInChildren<Canvas>(true);
        foreach (var c in canvases) c.enabled = visible;
    }
}

