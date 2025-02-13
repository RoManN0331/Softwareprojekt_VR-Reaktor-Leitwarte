using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// This class is used to change the color scheme in the scene to accommodate color blindness.
/// </summary>
public class FarbenButton : MonoBehaviour
{
    /// <param name="interactable"> is a reference to a XRSimpleInteractable</param>
    public XRBaseInteractable interactable;
    /// <param name="moveDistance"> specifies how far to move the button during the animation </param>
    public float moveDistance = 0.5f;            
    /// <param name="animationDuration"> specifies the duration of the animation</param>
    public float animationDuration = 0.5f;
    /// <param name="cooldownDuration"> specifies the duration of the animation cooldown</param>
    public float cooldownDuration = 1f;
    /// <param name="materialsEmissionChange"> is an array of materials to adjust</param>
    public Material[] materialsEmissionChange;  
    /// <param name="newColorsEmission"> is an array of new colors to apply to materials</param>
    public Color[] newColorsEmission;           
    /// <param name="emissionIntensity"> stores the Intensity of the emission color</param>
    public float emissionIntensity = 1.0f;      
    /// <param name="textMeshProObjects"> is an array of TextMeshPro objects to adjust</param>
    public TextMeshPro[] textMeshProObjects;    
    /// <param name="newTextColors"> is an array of new colors to apply to TextMeshPro objects</param>
    public Color newTextColors;                 
    /// <param name="isAnimating"> tracks whether the button is currently being animated</param>
    private bool isAnimating = false;
    /// <param name="originalColors"> is an array storing original colours</param>
    private Color[] originalColors;
    /// <param name="originalEmissionColors"> is an array storing original emission colours</param>
    private Color[] originalEmissionColors;
    /// <param name="originalTextColors"> is an array storing original text colours</param>
    private Color[] originalTextColors;

    /// <summary>
    /// This method finds, stores and deactivates all active display annotations. It also stores the original colour scheme of the scene.
    /// </summary>
    private void Start()
    {
        // Find all GameObjects named "AnzeigenMarker"
        // Find all GameObjects named "AnzeigenMarker"
        GameObject[] anzeigenMarkers = Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => go.name == "AnzeigenMarker" 
        #if UNITY_EDITOR
                         && !AssetDatabase.Contains(go)
        #endif
            )
            .ToArray();

        // Initialize textMeshProObjects with TextMeshPro components of children named "AusrufeZeichen"
        textMeshProObjects = Resources.FindObjectsOfTypeAll<TextMeshPro>()
            .Where(tmp => tmp.gameObject.name == "AusrufeZeichen")
            .ToArray();

        // Deactivate all "AnzeigenMarker" objects
        foreach (var marker in anzeigenMarkers)
        {
            marker.SetActive(false);
        }
        
        // Save original colors
        originalColors = new Color[materialsEmissionChange.Length];
        originalEmissionColors = new Color[materialsEmissionChange.Length];
        for (int i = 0; i < materialsEmissionChange.Length; i++)
        {
            if (materialsEmissionChange[i] != null)
            {
                originalColors[i] = materialsEmissionChange[i].color;
                if (materialsEmissionChange[i].IsKeywordEnabled("_EMISSION"))
                {
                    originalEmissionColors[i] = materialsEmissionChange[i].GetColor("_EmissionColor");
                }
            }
        }

        // Save original text colors
        originalTextColors = new Color[textMeshProObjects.Length];
        for (int i = 0; i < textMeshProObjects.Length; i++)
        {
            if (textMeshProObjects[i] != null)
            {
                originalTextColors[i] = textMeshProObjects[i].color;
            }
        }
    }
    

    /// <summary>
    /// This method is called when the object is enabled and adds event listeners for the selectEntered and selectExited events.
    /// </summary>    
    private void OnEnable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);
    }

    /// <summary>
    /// This method is called when the object is disabled and removes event listeners for the selectEntered and selectExited events. Additionally the original colour scheme is restored.
    /// </summary>
    private void OnDisable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.RemoveListener(OnSelectEntered);

        // Restore original colors
        for (int i = 0; i < materialsEmissionChange.Length; i++)
        {
            if (materialsEmissionChange[i] != null && i < originalColors.Length && i < originalEmissionColors.Length)
            {
                materialsEmissionChange[i].color = originalColors[i];
                if (materialsEmissionChange[i].IsKeywordEnabled("_EMISSION"))
                {
                    materialsEmissionChange[i].SetColor("_EmissionColor", originalEmissionColors[i]);
                }
            }
        }

        // Restore original text colors
        for (int i = 0; i < textMeshProObjects.Length; i++)
        {
            if (textMeshProObjects[i] != null && i < originalTextColors.Length)
            {
                textMeshProObjects[i].color = originalTextColors[i];
            }
        }
    }

    /// <param name="colorBlindModeActivated"> tracks whether the color blind mode is currently active</param>
    private bool colorBlindModeActivated = false;

    /// <summary>
    /// This method is called when an interactor enters the object and toggles colour blind mode.
    /// </summary>
    /// <param name="args"> passes event specific arguments upon entering the interaction</param>    
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!isAnimating && !colorBlindModeActivated)
        {
            StartCoroutine(AnimateTransform());
            ChangeMaterialsEmission();
            ChangeTextMeshProColors();
            ChangeCueColor();
            ChangeColorOfGazeGuidingButtons();
            ChangeMeltdownColorForLamps();
            colorBlindModeActivated = true;
        }else if (!isAnimating && colorBlindModeActivated)
        {
            // Restore original colors
            
            StartCoroutine(AnimateTransform());
            revertChangeMaterialsEmission();
            revertChangeTextMeshProColors();
            revertChangeCueColor();
            revertChangeColorOfGazeGuidingButtons();
            revertChangeMeltdownColorForLamps();
            colorBlindModeActivated = false;
        }
    }

    /// <summary>
    /// This method animates the button by moving it back and forth calling MoveToPosition().
    /// </summary>
    private IEnumerator AnimateTransform()
    {
        isAnimating = true;
        Vector3 initialPosition = transform.localPosition;
        Vector3 targetPosition = initialPosition + new Vector3(moveDistance, 0, 0);

        // Move to target position
        yield return MoveToPosition(initialPosition, targetPosition, animationDuration);

        // Move back to initial position
        yield return MoveToPosition(targetPosition, initialPosition, animationDuration);

        // Cooldown
        yield return new WaitForSeconds(cooldownDuration);

        isAnimating = false;
    }

    /// <summary>
    /// This method is called in AnimateTransform() to move the button.
    /// </summary>
    /// <param name="start"> is a Vector3 specifying the origin of the movement</param>
    /// <param name="end"> is a Vector3 specifying the end position of the movement</param>
    /// <param name="duration"> specifies the duration of the animation</param>
    private IEnumerator MoveToPosition(Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = end;
    }

    /// <summary>
    /// This method applies a new emission colour scheme to all the materials in the scene.
    /// </summary>
    private void ChangeMaterialsEmission()
    {
        for (int i = 0; i < materialsEmissionChange.Length; i++)
        {
            if (materialsEmissionChange[i] != null && i < newColorsEmission.Length)
            {
                materialsEmissionChange[i].color = newColorsEmission[i];
                if (materialsEmissionChange[i].IsKeywordEnabled("_EMISSION"))
                {
                    materialsEmissionChange[i].SetColor("_EmissionColor", newColorsEmission[i] * emissionIntensity);
                }
            }
        }
    }

    /// <summary>
    /// This method reverts the emission colour scheme of all the materials in the scene to the original colour scheme.
    /// </summary>    
    private void revertChangeMaterialsEmission()
    {
        for (int i = 0; i < materialsEmissionChange.Length; i++)
        {
            if (materialsEmissionChange[i] != null && i < originalColors.Length && i < originalEmissionColors.Length)
            {
                materialsEmissionChange[i].color = originalColors[i];
                if (materialsEmissionChange[i].IsKeywordEnabled("_EMISSION"))
                {
                    materialsEmissionChange[i].SetColor("_EmissionColor", originalEmissionColors[i]);
                }
            }
        }
    }

    /// <summary>
    /// This method applies a new colour scheme to all TextMeshPro objects in the scene.
    /// </summary>
    private void ChangeTextMeshProColors()
    {
        for (int i = 0; i < textMeshProObjects.Length; i++)
        {
            if (textMeshProObjects[i] != null)
            {
                textMeshProObjects[i].color = newTextColors;
            }
        }
    }

    /// <summary>
    /// This method reverts the colour scheme of all TextMeshPro objects in the scene to the original colour scheme.
    /// </summary>    
    private void revertChangeTextMeshProColors()
    {
        for (int i = 0; i < textMeshProObjects.Length; i++)
        {
            if (textMeshProObjects[i] != null)
            {
                textMeshProObjects[i].color = Color.red;
            }
        }
    }
    
    /// <summary>
    /// This method applies a new colour scheme to the DirectionCue objects in the scene.
    /// </summary>
    private void ChangeCueColor()
    {
        // Load the new sprite from the Resources folder
        Sprite newSprite = Resources.Load<Sprite>("DirectionCueMagenta");

        if (newSprite == null)
        {
            Debug.LogError("DirectionCueMagenta.png not found in Resources folder");
            return;
        }

        // Find all objects with Image components named DirectionCue and DirectionCue2
        Image[] directionCues = FindObjectsByType<Image>(FindObjectsSortMode.None)
            .Where(img => img.gameObject.name == "DirectionCue" || img.gameObject.name == "DirectionCue2")
            .ToArray();

        // Swap the image with the new sprite
        foreach (var img in directionCues)
        {
            img.sprite = newSprite;
        }
    }

    /// <summary>
    /// This method reverts the colour scheme of the DirectionCue objects in the scene back to the original color scheme.
    /// <summary>
    private void revertChangeCueColor()
    {
        Sprite newSprite = Resources.Load<Sprite>("DirectionCue");

        if (newSprite == null)
        {
            Debug.LogError("DirectionCue.png not found in Resources folder");
            return;
        }

        // Find all objects with Image components named DirectionCue and DirectionCue2
        Image[] directionCues = FindObjectsByType<Image>(FindObjectsSortMode.None)
            .Where(img => img.gameObject.name == "DirectionCue" || img.gameObject.name == "DirectionCue2")
            .ToArray();

        // Swap the image with the new sprite
        foreach (var img in directionCues)
        {
            img.sprite = newSprite;
        }
    }

    /// <summary>
    /// This method applies a new colour scheme to the GazeGuidingButtons in the scene.
    /// </summary>
    public void ChangeColorOfGazeGuidingButtons()
    {
        Flipper[] flippers = FindObjectsByType<Flipper>(FindObjectsSortMode.None);
        foreach (var flipper in flippers)
        {
            flipper.updateMaterials();
        }
    }

    /// <summary>
    /// This method reverts the colour scheme of the GazeGuidingButtons in the scene to the original colour scheme.
    /// </summary>    
    public void revertChangeColorOfGazeGuidingButtons()
    {
        Flipper[] flippers = FindObjectsByType<Flipper>(FindObjectsSortMode.None);
        foreach (var flipper in flippers)
        {
            flipper.revertMaterials();
        }
    }

    /// <summary>
    /// This method applies a new colour scheme to the lamps signalling the failure of specific components.
    /// </summary>
    public void ChangeMeltdownColorForLamps()
    {
        AusfallAnzeigenManager ausfallAnzeigenManager = FindAnyObjectByType<AusfallAnzeigenManager>();
        ausfallAnzeigenManager.toChange = Color.magenta;
    }

    /// <summary>
    /// This method reverts the colour scheme of the lamps signalling the failure of specific components to the original colour scheme.
    /// </summary>    
    public void revertChangeMeltdownColorForLamps()
    {
        AusfallAnzeigenManager ausfallAnzeigenManager = FindAnyObjectByType<AusfallAnzeigenManager>();
        ausfallAnzeigenManager.toChange = Color.white;
    }
}