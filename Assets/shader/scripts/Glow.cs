using UnityEngine;

public class Glow : MonoBehaviour{

/// <summary>
/// This class is used to create a glow effect on an object.
/// </summary>


/// <param name="glowStrength_on"> float setting the strength of the glow effect when turned on</param>
/// <param name="glowStrength_off"> float setting the strength of the glow effect when turned off</param>
/// <param name="isGlowing"> boolean to check if the glow effect is currently active active on the object </param>
/// <param name="objectRenderer"> Renderer object to render the glow effect </param>
/// <param name="mats"> Material the renderer is applied to </param>

    [SerializeField] private float glowStrength_on = 1.5f;
    private float glowStrength_off = 0.0f;
    private bool isGlowing = false;
    
    private Renderer objectRenderer;
    private Material mats;

/// <summary>
/// This method initialises the objectRenderer rendering the glow effect as well as the strength of the effect
/// </summary>

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

/// <summary>
/// This method toggles the glow effect on and off.
/// </summary>

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