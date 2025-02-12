using UnityEngine;

/// <summary>
/// This class implements a small lamp that is attachable to rotary switches.
/// </summary>
public class LightRegler : MonoBehaviour
{

    /// <param name="redMaterial"> Material for the red light </param>
    public Material redMaterial;
    /// <param name="greenMaterial"> Material for the green light </param>
    public Material greenMaterial;
    /// <param name="objectRenderer"> Reference to the Renderer component of the object </param>
    private Renderer objectRenderer;

    /// <summary>
    /// This method initializes the renderer component.
    /// </summary>
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// This method sets the material for the object, i.e. lamp, changing the color of the light.
    /// </summary>
    public void SetLight(bool isOn)
    {
        if (isOn)
        {
            objectRenderer.material = greenMaterial;
        }
        else
        {
            objectRenderer.material = redMaterial;
        }
    }
}