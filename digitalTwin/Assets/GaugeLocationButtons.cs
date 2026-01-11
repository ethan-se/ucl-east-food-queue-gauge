using UnityEngine;

public class GaugeLocationButtons : MonoBehaviour
{
    public mqttController controller;  // drag ARObject (with mqttController) here

    // These will be assigned to your 3 UI buttons
    public void SelectPoolStreet()
    {
        controller.SetActiveLocation(0); // topicSubscribe[0]
    }

    public void SelectMarshgateCafe()
    {
        controller.SetActiveLocation(1); // topicSubscribe[1]
    }

    public void SelectMarshgateCanteen()
    {
        controller.SetActiveLocation(2); // topicSubscribe[2]
    }
}
