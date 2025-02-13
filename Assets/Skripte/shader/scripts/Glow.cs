using System.Collections;
using UnityEngine;

/// <summary>
/// This class implements logic to apply a glow effect to an object.
/// </summary>
public class Glow : MonoBehaviour{


    /// <param name="glowStrength_on"> specifies the strength of the glow effect when turned on</param>
    [SerializeField] private float glowStrength_on = 1.5f;
    /// <param name="glowStrength_off"> specifies the strength of the glow effect when turned off</param>
    private float glowStrength_off = 0.0f;
    /// <param name="isGlowing"> checks if the glow effect is currently active active on the object </param>
    private bool isGlowing = false;
    /// <param name="objectRenderer"> is a reference to a renderer object to render the glow effect </param>
    private Renderer objectRenderer;
    /// <param name="mats"> is a Material being rendered by the renderer </param>
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
        if (!isGlowing)
        {
            StartCoroutine(AnimateGlow(glowStrength_off, glowStrength_on, 1.0f)); // 1 second duration
            isGlowing = true;
        }
        else
        {
            StartCoroutine(AnimateGlow(glowStrength_on, glowStrength_off, 1.0f)); // 1 second duration
            isGlowing = false;
        }
    }

    /// <summary>
    /// This method interpolates the glow strength.
    /// </summary>
    /// <param name="startValue"> lower bound for the interpolation</param>
    /// <param name="endValue"> upper bound for the interpolation</param>
    /// <param name="duration"> duration of the interpolation</param>
    private IEnumerator AnimateGlow(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float currentGlow = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            mats.SetFloat("_GlowStrength", currentGlow);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mats.SetFloat("_GlowStrength", endValue);
    }
}