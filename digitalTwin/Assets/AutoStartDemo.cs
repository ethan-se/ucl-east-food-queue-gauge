using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class AutoStartDemo : MonoBehaviour
{
    public GameObject objectToSpawn;

    ARRaycastManager raycastManager;
    GameObject spawnedObject;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        InvokeRepeating("TrySpawnObject", 0.5f, 0.5f);
    }

    void TrySpawnObject()
    {
        // Cast from screen center
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        if (raycastManager.Raycast(screenCenter, hits, TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(objectToSpawn, hitPose.position, hitPose.rotation);
                Debug.Log("AUTO-SPAWN SUCCESS");
            }
        }
    }
}
