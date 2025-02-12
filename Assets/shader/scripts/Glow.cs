using UnityEngine;

/// <summary>
/// This class implements logic to apply a glow effect to an object.
/// </summary>
public class Glow : MonoBehaviour
{
    /// <param name="glowStrength_on"> float setting the strength of the glow effect when turned on</param>
    [SerializeField] private float glowStrength_on = 1.5f;
    /// <param name="glowStrength_off"> float setting the strength of the glow effect when turned off</param>
    private float glowStrength_off = 0.0f;
    /// <param name="isGlowing"> boolean to check if the glow effect is currently active active on the object </param>
    private bool isGlowing = false;
    /// <param name="objectRenderer"> Renderer object to render the glow effect </param>
    private Renderer objectRenderer;
    /// <param name="mats"> Material the renderer is applied to </param>
    private Material mats;

    /// <summary>
    /// This method initialises the objectRenderer to render the glow effect and sets the strength of the effect
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