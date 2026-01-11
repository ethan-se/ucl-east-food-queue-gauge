using UnityEngine;
using UnityEngine.InputSystem;

public class GaugeButton3D : MonoBehaviour
{
    public mqttController controller;

    [Header("Button Meshes (3D)")]
    public MeshRenderer buttonPoolStreet;
    public MeshRenderer buttonMarshgateCafe;
    public MeshRenderer buttonMarshgateCanteen;

    [Header("Materials")]
    public Material defaultMaterial;
    public Material highlightMaterial;

    private Camera arCamera;

    void Start()
    {
        arCamera = Camera.main;
        ResetHighlights();
    }

    void Update()
    {
        if (Touchscreen.current == null) return;

        if (!Touchscreen.current.primaryTouch.press.wasPressedThisFrame) return;

        Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
        Ray ray = arCamera.ScreenPointToRay(touchPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == buttonPoolStreet.transform)
            {
                controller.SetActiveLocation(0);
                HighlightButton(buttonPoolStreet);
                Debug.Log("Selected POOL STREET");
            }
            else if (hit.transform == buttonMarshgateCafe.transform)
            {
                controller.SetActiveLocation(1);
                HighlightButton(buttonMarshgateCafe);
                Debug.Log("Selected MARSHGATE CAFE");
            }
            else if (hit.transform == buttonMarshgateCanteen.transform)
            {
                controller.SetActiveLocation(2);
                HighlightButton(buttonMarshgateCanteen);
                Debug.Log("Selected MARSHGATE CANTEEN");
            }
        }
    }

    void HighlightButton(MeshRenderer selected)
    {
        // Reset all buttons first
        ResetHighlights();

        // Apply highlight material to selected button
        selected.material = highlightMaterial;
    }

    void ResetHighlights()
    {
        if (buttonPoolStreet != null) buttonPoolStreet.material = defaultMaterial;
        if (buttonMarshgateCafe != null) buttonMarshgateCafe.material = defaultMaterial;
        if (buttonMarshgateCanteen != null) buttonMarshgateCanteen.material = defaultMaterial;
    }
}
