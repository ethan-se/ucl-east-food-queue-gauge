using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class mqttController : MonoBehaviour
{
    [Tooltip("Optional name for the controller")]
    public string nameController = "Controller 1";

    [Tooltip("Tag of the GameObject that has mqttManager attached")]
    public string tag_mqttManager = "mqttManager";

    [Header("   Case Sensitive!!")]
    [Tooltip("List of full MQTT topics for the 3 locations, in this order: 0=Pool, 1=Cafe, 2=Canteen")]
    public List<string> topicSubscribe = new List<string>(); 

    // Which location is currently selected by the user (0, 1 or 2)
    [Header("Location selection")]
    [Tooltip("Index of the currently selected topic/location: 0=Pool, 1=Cafe, 2=Canteen")]
    public int activeTopicIndex = 0;

    // Gauge numeric value (queue time in minutes)
    private float pointerValue = 0f;

    [Space]
    [Header("Gauge Pointer (3D)")]
    public GameObject objectToControl;      // pointer GameObject
    public enum State { X, Y, Z };
    public State rotationAxis = State.Z;
    public bool ClockWise = true;
    private int rotationDirection = 1;

    [Tooltip("Minimum queue value the dial can show")]
    public float startValue = 0f;
    [Tooltip("Maximum queue value the dial can show")]
    public float endValue = 60f;
    [Tooltip("Full sweep of the needle in degrees")]
    public float fullAngle = 270f;
    [Tooltip("Offset angle to align the mesh")]
    public float adjustedStart = -45f;

    [Space]
    [Header("Optional UI")]
    public TextMeshProUGUI specialText;   // e.g. "Pastry&Capp £3.8"
    public TextMeshProUGUI queueText;     // e.g. "Wait: 7 min"

    [Header("Daily Specials (0 = Mon ... 4 = Fri)")]
    [SerializeField] private string[] poolSpecials = {
        "Pastry&Capp £3.8",
        "Iced Latte £2.2",
        "Vegan Wrap £4.8",
        "Cake+Tea £4.0",
        "StudentDeal £4.5"
    };

    [SerializeField] private string[] mgCafeSpecials = {
        "MozzaPanini £5.0",
        "Smoothie £2.5",
        "Wrap+Water £4.8",
        "CroissLatte £4.2",
        "Bagel £4.9"
    };

    [SerializeField] private string[] mgCanteenSpecials = {
        "Plant Bowl £5.2",
        "ChicknKatsu £6.0",
        "Mac+Chicken £5.8",
        "Veg Lasagne £5.8",
        "Fish&Chips £6.5"
    };

    [Space]
    public mqttManager _eventSender;

    // ---------- Called by your buttons on the gauge ----------
    public void SetActiveLocation(int index)
    {
        if (topicSubscribe == null || topicSubscribe.Count == 0) return;

        activeTopicIndex = Mathf.Clamp(index, 0, topicSubscribe.Count - 1);
        Debug.Log($"[{nameController}] Active location set to index {activeTopicIndex}: {topicSubscribe[activeTopicIndex]}");
    }

    // ---------- Unity lifecycle ----------

    private void Awake()
    {
        var managers = GameObject.FindGameObjectsWithTag(tag_mqttManager);
        if (managers.Length > 0)
        {
            _eventSender = managers[0].GetComponent<mqttManager>();
            _eventSender.Connect();
        }
        else
        {
            Debug.LogError("mqttController: No GameObject found with Tag '" + tag_mqttManager +
                           "' containing mqttManager component.");
        }
    }

    private void OnEnable()
    {
        if (_eventSender != null)
            _eventSender.OnMessageArrived += OnMessageArrivedHandler;
    }

    private void OnDisable()
    {
        if (_eventSender != null)
            _eventSender.OnMessageArrived -= OnMessageArrivedHandler;
    }

    // ---------- Handle incoming MQTT messages ----------

    private void OnMessageArrivedHandler(mqttObj mqttObject)
    {
        // 1) Check we have topics and a valid active index
        if (topicSubscribe == null || topicSubscribe.Count == 0) return;
        if (activeTopicIndex < 0 || activeTopicIndex >= topicSubscribe.Count) return;

        string selectedTopic = topicSubscribe[activeTopicIndex];

        // 2) Only react to the topic for the currently selected location
        if (!string.Equals(mqttObject.topic, selectedTopic, StringComparison.Ordinal))
        {
            return; // message is for another location
        }

        // 3) Parse JSON
        GaugeMessage msg = null;
        try
        {
            msg = JsonUtility.FromJson<GaugeMessage>(mqttObject.msg);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[{nameController}] Failed to parse JSON: {e.Message}\nRaw: {mqttObject.msg}");
            return;
        }

        if (msg == null)
        {
            Debug.LogWarning($"[{nameController}] JSON parsed to null. Raw: {mqttObject.msg}");
            return;
        }

        // 4) Use queue_time_min as gauge value
        int q = Mathf.Max(0, msg.queue_time_min);
        pointerValue = Mathf.Clamp(q, startValue, endValue);

        // 5) Update UI for THIS location
        if (specialText != null)
            specialText.text = GetSpecialLabel(msg.location, msg.weekday);

        if (queueText != null)
            queueText.text = $"Wait: {q} min";

        Debug.Log($"[{nameController}] topic={mqttObject.topic} loc={msg.location} weekday={msg.weekday} queue={q}");
    }

    // ---------- Rotate gauge each frame ----------

    private void Update()
    {
        if (objectToControl == null) return;

        float step = 1.5f * Time.deltaTime;
        rotationDirection = ClockWise ? -1 : 1;

        if (pointerValue >= startValue)
        {
            float normalized = (pointerValue - startValue) / Mathf.Max(0.0001f, (endValue - startValue));
            float angle = rotationDirection * (normalized * fullAngle) - adjustedStart;

            Vector3 rotationVector;

            if (rotationAxis == State.X)
            {
                rotationVector = new Vector3(
                    angle,
                    objectToControl.transform.localEulerAngles.y,
                    objectToControl.transform.localEulerAngles.z);
            }
            else if (rotationAxis == State.Y)
            {
                rotationVector = new Vector3(
                    objectToControl.transform.localEulerAngles.x,
                    angle,
                    objectToControl.transform.localEulerAngles.z);
            }
            else // Z
            {
                rotationVector = new Vector3(
                    objectToControl.transform.localEulerAngles.x,
                    objectToControl.transform.localEulerAngles.y,
                    angle);
            }

            objectToControl.transform.localRotation = Quaternion.Lerp(
                objectToControl.transform.localRotation,
                Quaternion.Euler(rotationVector),
                step);
        }
    }

    // ---------- Specials helper ----------

    private string GetSpecialLabel(string location, int weekday)
    {
        weekday = Mathf.Clamp(weekday, 0, 4);

        switch (location)
        {
            case "pool_street":
                return poolSpecials.Length > weekday ? poolSpecials[weekday] : "No special";
            case "marshgate_cafe":
                return mgCafeSpecials.Length > weekday ? mgCafeSpecials[weekday] : "No special";
            case "marshgate_canteen":
                return mgCanteenSpecials.Length > weekday ? mgCanteenSpecials[weekday] : "No special";
            default:
                return "No special";
        }
    }
}


