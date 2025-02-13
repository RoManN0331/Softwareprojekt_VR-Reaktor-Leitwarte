using UnityEngine;

/// <summary>
/// This class implements a small lamp that is attachable to rotary switches.
/// </summary>
public class LightRegler : MonoBehaviour
{
    /// <param name="redMaterial"> is a Material to make the lamp shine red </param>
    public Material redMaterial;
    /// <param name="greenMaterial"> is a Material to make the lamp shine green </param>
    public Material greenMaterial;
    /// <param name="objectRenderer"> is a Reference to the Renderer component of the object </param>
    private Renderer objectRenderer;

    /// <summary>
    /// This method initialises the renderer component.
    /// </summary>
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// This method changes the colour of the lamp by changing its material.
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