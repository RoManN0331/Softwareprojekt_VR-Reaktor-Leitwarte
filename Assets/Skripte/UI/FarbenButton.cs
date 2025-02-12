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

    /// <param name="interactable">XRSimpleInteractable</param>
    public XRBaseInteractable interactable;
    /// <param name="moveDistance">float specifying how far to move the switch in the animation </param>
    public float moveDistance = 0.5f;            
    /// <param name="animationDuration">float specifying the duration of the animation</param>
    public float animationDuration = 0.5f;
    /// <param name="cooldownDuration">float specifying the duration of the animation cooldown</param>
    public float cooldownDuration = 1f;
    /// <param name="materialsEmissionChange">Array of materials to adjust</param>
    public Material[] materialsEmissionChange;  
    /// <param name="newColorsEmission">Array of new colors to apply to materials</param>
    public Color[] newColorsEmission;           
    /// <param name="emissionIntensity">Intensity of the emission color</param>
    public float emissionIntensity = 1.0f;      
    /// <param name="textMeshProObjects">Array of TextMeshPro objects to adjust</param>
    public TextMeshPro[] textMeshProObjects;    
    /// <param name="newTextColors">Array of new colors to apply to TextMeshPro objects</param>
    public Color newTextColors;                 
    /// <param name="isAnimating">boolean tracking whether the button is currently being animated</param>
    private bool isAnimating = false;
    /// <param name="originalColors">Array storing original colours</param>
    private Color[] originalColors;
    /// <param name="originalEmissionColors">Array storing original emission colours</param>
    private Color[] originalEmissionColors;
    /// <param name="originalTextColors">Array storing original text colours</param>
    private Color[] originalTextColors;
    /// <param name="colorBlindModeActivated">boolean tracking whether the color blind mode is currently activated</param>
    private bool colorBlindModeActivated = false;


    /// <summary>
    /// This method finds, stores and deactivates all active display annotations, and it also stores the original colour scheme of the scene.
    /// </summary>
    private void Start()
    {
        // Find all GameObjects named "AnzeigenMarker"
        GameObject[] anzeigenMarkers = Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => go.name == "AnzeigenMarker" && !AssetDatabase.Contains(go))
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

    /// <summary>
    /// This method is called when an interactor enters the object and toggles colour blind mode.
    /// </summary>
    /// <param name="args">SelectEnterEventArgs to pass event specific arguments upon entering the interaction</param>
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
    /// This method animates the button by moving it back and forth.
    /// </summary>
    private IEnumerator AnimateTransform()
    {
        // animates the button

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
    /// This method is called in AnimateTransform() to move the button to a target position.
    /// </summary>
    /// <param name="start">Vector3 specifying the origin of the vector</param>
    /// <param name="end">Vector3 specifying the end position of the vector</param>
    /// <param name="duration">float specifying the duration of the animation</param>
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
    /// This method applies a new colour scheme to all the TextMeshPro objects in the scene.
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
    /// This method reverts the colour scheme of all the TextMeshPro objects in the scene to the original colour scheme.
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
    /// This method reverts the colour scheme of the DirectionCue objects back to the original color scheme of the scene.
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
    /// This method applies a new colour scheme to the lamps that signal the failure of specific components in the scene.
    /// </summary>
    public void ChangeMeltdownColorForLamps()
    {
        AusfallAnzeigenManager ausfallAnzeigenManager = FindAnyObjectByType<AusfallAnzeigenManager>();
        ausfallAnzeigenManager.toChange = Color.magenta;
    }

    /// <summary>
    /// This method reverts the colour scheme of the lamps that signal the failure of specific components in the scene to the original colour scheme.
    /// </summary>
    public void revertChangeMeltdownColorForLamps()
    {
        AusfallAnzeigenManager ausfallAnzeigenManager = FindAnyObjectByType<AusfallAnzeigenManager>();
        ausfallAnzeigenManager.toChange = Color.white;
    }
}