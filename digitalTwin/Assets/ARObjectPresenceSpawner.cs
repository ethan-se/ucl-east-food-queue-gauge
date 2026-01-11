using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ARObjectPresenceSpawner : MonoBehaviour
{
    [Header("Prefab Reference")]
    public GameObject arObjectPrefab;   // Drag your "ar object" prefab here

    private ARTrackedImageManager imageManager;
    private GameObject arObjectInstance;

    private void Awake()
    {
        imageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        //  AR Foundation 6: use AddListener, not +=
        imageManager.trackablesChanged.AddListener(OnTrackablesChanged);
    }

    private void OnDisable()
    {
        //  Remove the listener when disabled
        imageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
    }

    // This is called whenever tracked images are added/updated/removed
    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        UpdateObjectState();
    }

    private void UpdateObjectState()
    {
        bool physicalGaugeVisible = false;

        // Check all currently tracked images
        foreach (var trackedImage in imageManager.trackables)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                physicalGaugeVisible = true;
                break;
            }
        }

        if (physicalGaugeVisible)
        {
            // Physical gauge present → remove AR object if it exists
            if (arObjectInstance != null)
            {
                Destroy(arObjectInstance);
                arObjectInstance = null;
            }
        }
        else
        {
            // Physical gauge NOT present → spawn AR object if it's missing
            if (arObjectInstance == null && arObjectPrefab != null)
            {
                arObjectInstance = Instantiate(arObjectPrefab);

                // Parent under XR Origin (this object)
                arObjectInstance.transform.SetParent(transform, false);

                // Place 1m in front of the origin (you can tweak this)
                arObjectInstance.transform.localPosition = new Vector3(0f, 0f, 1f);
                arObjectInstance.transform.localRotation = Quaternion.identity;
            }
        }
    }
}

