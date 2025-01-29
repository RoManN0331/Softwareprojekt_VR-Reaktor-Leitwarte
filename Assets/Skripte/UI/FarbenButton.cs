using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;
using UnityEditor;

public class FarbenButton : MonoBehaviour
{
    public XRBaseInteractable interactable;
    public float moveDistance = 0.5f;
    public float animationDuration = 0.5f;
    public float cooldownDuration = 1f;
    public Material[] materialsEmissionChange; // Array of materials to adjust
    public Color[] newColorsEmission; // Array of new colors to apply to materials
    public float emissionIntensity = 1.0f; // Intensity of the emission color
    public TextMeshPro[] textMeshProObjects; // Array of TextMeshPro objects to adjust
    public Color newTextColors; // Array of new colors to apply to TextMeshPro objects

    private bool isAnimating = false;
    private Color[] originalColors;
    private Color[] originalEmissionColors;
    private Color[] originalTextColors;

    private void Start()
    {
        // Find all GameObjects named "AnzeigenMarker"
        GameObject[] anzeigenMarkers = Resources.FindObjectsOfTypeAll<GameObject>()
            .Where(go => go.name == "AnzeigenMarker" && !AssetDatabase.Contains(go))
            .ToArray();

        // Initialize textMeshProObjects with TextMeshPro components of children named "AusrufeZeichen"
        textMeshProObjects = anzeigenMarkers
            .SelectMany(marker => marker.GetComponentsInChildren<TextMeshPro>(true))
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
    
    
    private void OnEnable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);
    }

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

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!isAnimating)
        {
            StartCoroutine(AnimateTransform());
            ChangeMaterialsEmission();
            ChangeTextMeshProColors();
        }
    }

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
}