using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.name == "LineVisual" && other.gameObject.GetComponent<LineRenderer>() != null)
        {
            Debug.Log("LineVisual detected");
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                Color color = material.color;
                color.a = 0.2f; // Set alpha to 10%
                material.color = color;

                material.DisableKeyword("_EMISSION");
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.name == "LineVisual" && other.gameObject.GetComponent<LineRenderer>() != null)
        {
            Debug.Log("LineVisual detected");
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                Color color = material.color;
                color.a = 1f; // Set alpha to 10%
                material.color = color;

                material.EnableKeyword("_EMISSION");
            }
        }
    }
}