using UnityEngine;

public class Glow : MonoBehaviour{


    [SerializeField] private float glowStrength_on = 1.5f;
    private float glowStrength_off = 0.0f;
    private bool isGlowing = false;
    
    private Renderer objectRenderer;
    private Material mats;

    void Start()
    {
        if (objectRenderer == null)
        {
            objectRenderer = this.GetComponentInChildren<MeshRenderer>();
            Debug.Log("objectRenderer: " + objectRenderer);
        }

        mats = objectRenderer.material;
        mats.SetFloat("_GlowStrength", glowStrength_off);
    }

    public void setGlowing()
    {
        /*turns the glow on*/

        if (!isGlowing)
        {
            Debug.Log(mats.GetFloat("_GlowStrength"));
            mats.SetFloat("_GlowStrength", glowStrength_on);
            isGlowing = true;

        }
        else
        {
            Debug.Log(mats.GetFloat("_GlowStrength"));
            mats.SetFloat("_GlowStrength", glowStrength_off);
            isGlowing = false;
        }
    }
}