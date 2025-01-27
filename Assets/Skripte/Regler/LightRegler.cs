using UnityEngine;

public class LightRegler : MonoBehaviour
{
    public Material redMaterial;
    public Material greenMaterial;

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

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