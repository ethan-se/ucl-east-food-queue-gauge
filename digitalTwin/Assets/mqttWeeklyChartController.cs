using System;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

public class mqttWeeklyChartController : MonoBehaviour
{
    [Serializable]
    public class LocationChart
    {
        [Header("ID (must match GaugeMessage.location exactly)")]
        public string locationId;

        [Header("Weekly Peak Busy Time LineChart")]
        public LineChart weeklyChart;

        [HideInInspector] public int[] peakBusyValue = new int[7];       // max queue per day
        [HideInInspector] public float[] peakBusyMinutes = new float[7]; // minute-of-day for peak (0..1440)
    }

    [Header("MQTT")]
    public string mqttManagerTag = "mqttManager";
    private mqttManager _mqtt;

    [Header("Charts")]
    public List<LocationChart> charts = new List<LocationChart>();

    [Header("X Axis Labels (Mon..Sun)")]
    public string[] weekLabels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

    [Header("Y Axis Meaning")]
    [Tooltip("If true, Y = minutes since midnight (0..1440). If false, Y = peak queue value (minutes waiting).")]
    public bool plotPeakTimeOfDay = true;

    private void Awake()
    {
        var managers = GameObject.FindGameObjectsWithTag(mqttManagerTag);
        if (managers.Length > 0)
        {
            _mqtt = managers[0].GetComponent<mqttManager>();
            if (_mqtt != null && !_mqtt.isConnected)
            {
                _mqtt.Connect();
            }
        }
        else
        {
            Debug.LogError($"{nameof(mqttWeeklyChartController)}: No mqttManager found with tag '{mqttManagerTag}'");
        }

        // Configure all charts once at start
        foreach (var c in charts)
        {
            SetupChartAxes(c);
            RedrawChart(c);
        }
    }

    private void OnEnable()
    {
        if (_mqtt != null)
            _mqtt.OnMessageArrived += OnMessageArrivedHandler;
    }

    private void OnDisable()
    {
        if (_mqtt != null)
            _mqtt.OnMessageArrived -= OnMessageArrivedHandler;
    }

    // ---------------- MQTT ----------------

    private void OnMessageArrivedHandler(mqttObj mqttObject)
    {
        GaugeMessage msg;
        try
        {
            msg = JsonUtility.FromJson<GaugeMessage>(mqttObject.msg);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"{nameof(mqttWeeklyChartController)}: JSON parse failed: {e.Message}");
            return;
        }

        if (msg == null || string.IsNullOrEmpty(msg.location)) return;

        var loc = charts.Find(c => c.locationId == msg.location);
        if (loc == null) return;

        // Your queue time value (peak "busy" magnitude)
        int q = Mathf.Max(0, msg.queue_time_min);

        DateTime now = DateTime.Now;
        int dayIndex = DayIndexMondayFirst(now);

        // Decide what we store as the “peak” Y-value:
        // - If plotPeakTimeOfDay: store minute-of-day only when q sets a new max
        // - Else: store the peak queue value itself
        if (q >= loc.peakBusyValue[dayIndex])
        {
            loc.peakBusyValue[dayIndex] = q;

            if (plotPeakTimeOfDay)
            {
                loc.peakBusyMinutes[dayIndex] = MinutesSinceMidnight(now);
            }

            // Update chart immediately
            RedrawChart(loc);
        }
    }

    // ---------------- Chart Drawing ----------------

    private void SetupChartAxes(LocationChart loc)
    {
        if (loc == null || loc.weeklyChart == null) return;

        // X Axis labels: Mon..Sun
        var xAxis = loc.weeklyChart.GetChartComponent<XAxis>();
        if (xAxis != null)
        {
            xAxis.type = Axis.AxisType.Category;
            xAxis.data.Clear();
            xAxis.data.AddRange(weekLabels);
        }

        // Ensure we have at least one line serie at index 0
        if (loc.weeklyChart.series.Count == 0)
        {
            loc.weeklyChart.AddSerie<Line>("Peak");
        }
    }

    private void RedrawChart(LocationChart loc)
    {
        if (loc == null || loc.weeklyChart == null) return;

        // Clear then add 7 points
        loc.weeklyChart.ClearData();

        for (int i = 0; i < 7; i++)
        {
            float y;

            if (plotPeakTimeOfDay)
            {
                // Peak time-of-day (minutes since midnight)
                y = loc.peakBusyMinutes[i];
            }
            else
            {
                // Peak magnitude (queue minutes waiting)
                y = loc.peakBusyValue[i];
            }

            loc.weeklyChart.AddData(0, y);
        }

        loc.weeklyChart.RefreshChart();
    }

    // ---------------- Helpers ----------------

    private static int DayIndexMondayFirst(DateTime dt)
    {
        // Sunday=0 in .NET, we want Monday=0 .. Sunday=6
        int dow = (int)dt.DayOfWeek;
        return (dow == 0) ? 6 : dow - 1;
    }

    private static float MinutesSinceMidnight(DateTime dt)
    {
        return dt.Hour * 60f + dt.Minute + dt.Second / 60f;
    }
}
