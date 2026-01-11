using System;

[Serializable]
public class GaugeMessage
{
    public string location;       // "pool_street", "marshgate_cafe", "marshgate_canteen"
    public int    weekday;        // 0..4 (Mon–Fri)
    public int    queue_time_min; // minutes
}
