using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Flipper : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    
    GazeGuidingButtons gazeGuidingButtons;
    
    public Material mat;

    public Material oldmat;
    
    
    // IN INSPECTOR ONLY SELECT ONE OF THE FOLLOWING OPTIONS
    public bool DirectionCueEnabled = false;
    
    public bool DirectionArrowEnabled = false;    
    
    public bool Arrow3DEnabled = false;
    
    public bool Arrow3DBinearEnabled = false;

    public bool DirectionArrowOnScreen = false;
    
    public bool AnzeigenMarkierungEnabled = false;
    
    public bool HUDEnabled = false;
    
    public bool ifHUDEnabledShouldItBeOn = false;

    public bool DetachEnabled = false;
    public bool distractionsEnabled = false;
    
    public bool BlurEnabled = false;
     
    public bool ifdistractionsEnabledShouldItBeOn = false;

    public bool clipboardHighlight;
    
    public bool clipboardHighlightShouldItBeOn = false;
    
    public bool AnzeigenHighlight = false;
        
    public bool AnzeigenHighlightShouldItBeOn = false;
        
    private bool flipped = false;
    
    
    
    private void Start()
    {
        gazeGuidingButtons = FindAnyObjectByType<GazeGuidingButtons>();
        
         meshRenderer = GetComponent<MeshRenderer>();
         
         Material mater = meshRenderer.material;

         mater.DisableKeyword("_EMISSION");
         
        GazeGuidingPathPlayer pathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        
        //INITALISIERE STATE
        if (DirectionCueEnabled && DirectionCueEnabled == pathPlayer.DirectionCueEnabled) StartCoroutine(FlipWithoutCall()); 
        if (DirectionArrowEnabled && DirectionArrowEnabled == pathPlayer.DirectionArrowEnabled) StartCoroutine(FlipWithoutCall());  
        if (Arrow3DEnabled && Arrow3DEnabled == pathPlayer.Arrow3DEnabled) StartCoroutine(FlipWithoutCall()); 
        if (Arrow3DBinearEnabled && Arrow3DBinearEnabled == pathPlayer.Arrow3DBinearEnabled) StartCoroutine(FlipWithoutCall()); 
        if (DirectionArrowOnScreen && DirectionArrowOnScreen == pathPlayer.DirectionArrowOnScreen) StartCoroutine(FlipWithoutCall()); 
        if (AnzeigenMarkierungEnabled && AnzeigenMarkierungEnabled == pathPlayer.AnzeigenMarkierungEnabled) StartCoroutine(FlipWithoutCall());
        if (HUDEnabled && ifHUDEnabledShouldItBeOn)StartCoroutine(Flip());
        if (DetachEnabled && DetachEnabled == pathPlayer.detached) StartCoroutine(FlipWithoutCall());
        if (BlurEnabled && BlurEnabled == pathPlayer.blur) StartCoroutine(FlipWithoutCall());
        if (distractionsEnabled && ifdistractionsEnabledShouldItBeOn) StartCoroutine(Flip());
        
        if(AnzeigenHighlight && AnzeigenHighlightShouldItBeOn) StartCoroutine(Flip());
        
        if (clipboardHighlight && clipboardHighlightShouldItBeOn) StartCoroutine(Flip());

    }
    
    private void OnEnable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.RemoveListener(OnSelectEntered);
        interactable.selectExited.RemoveListener(OnSelectExited);
    }
    

    private bool alreadyClicked = false;
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!alreadyClicked)
        {
            StartCoroutine(Flip());
            alreadyClicked = true;
        }
    }
    private bool CoroutineRunning = false;
    private void OnSelectExited(SelectExitEventArgs args)
    {
        if(!CoroutineRunning) StartCoroutine(ResetAlreadyClicked());
    }
    
    private IEnumerator ResetAlreadyClicked()
    {
        CoroutineRunning = true;
        yield return new WaitForSeconds(1.5f);
        alreadyClicked = false;
        CoroutineRunning = false;
    }
    
    private IEnumerator Flip()
    {
        Material mater = meshRenderer.material;

        if(!flipped)
        {
            mater.EnableKeyword("_EMISSION");
            // Smoothly rotate to 40 degrees on the X axis over 0.5 seconds
            yield return RotateToAngle(22, 0.15f);
            flipped = true;
            
            // Turn ON/OFF gazeguiding

            if (DirectionCueEnabled) gazeGuidingButtons.DirectionCue(true);
            if (DirectionArrowEnabled) gazeGuidingButtons.DirectionArrow(true);
            if (Arrow3DEnabled) gazeGuidingButtons.Arrow3D(true);
            if (Arrow3DBinearEnabled) gazeGuidingButtons.Arrow3DBinear(true);
            if (DirectionArrowOnScreen) gazeGuidingButtons.DirectionArrowOnScreen(true);
            if (AnzeigenMarkierungEnabled) gazeGuidingButtons.AnzeigenMarkierung(true);
            if (HUDEnabled) gazeGuidingButtons.HUD(true);
            if (DetachEnabled) gazeGuidingButtons.TriggerDetach(true);
            if (BlurEnabled) gazeGuidingButtons.TriggerBlur(true);
            if (distractionsEnabled) gazeGuidingButtons.Distractions(true);
            if (clipboardHighlight) gazeGuidingButtons.ClipboardHighlight(true);
            if (AnzeigenHighlight) gazeGuidingButtons.AnzeigenHighlight(true);

        }
        else
        {
            mater.DisableKeyword("_EMISSION");
            // Smoothly rotate to 40 degrees on the X axis over 0.5 seconds
            yield return RotateToAngle(0, 0.15f);
            flipped = false;
            
            // Turn ON/OFF gazeguiding
            
            if (DirectionCueEnabled) gazeGuidingButtons.DirectionCue(false);
            if (DirectionArrowEnabled) gazeGuidingButtons.DirectionArrow(false);
            if (Arrow3DEnabled) gazeGuidingButtons.Arrow3D(false);
            if (Arrow3DBinearEnabled) gazeGuidingButtons.Arrow3DBinear(false);
            if (DirectionArrowOnScreen) gazeGuidingButtons.DirectionArrowOnScreen(false);
            if (AnzeigenMarkierungEnabled) gazeGuidingButtons.AnzeigenMarkierung(false);
            if (HUDEnabled) gazeGuidingButtons.HUD(false);
            if (DetachEnabled) gazeGuidingButtons.TriggerDetach(false);
            if (BlurEnabled) gazeGuidingButtons.TriggerBlur(false);
            if (distractionsEnabled) gazeGuidingButtons.Distractions(false);
            if (clipboardHighlight) gazeGuidingButtons.ClipboardHighlight(false);
            if (AnzeigenHighlight) gazeGuidingButtons.AnzeigenHighlight(false);
        }
        
    }

    private IEnumerator FlipWithoutCall()
    {
        Material mater = meshRenderer.material;

        if(!flipped)
        {
            mater.EnableKeyword("_EMISSION");
            // Smoothly rotate to 40 degrees on the X axis over 0.5 seconds
            yield return RotateToAngle(22, 0.15f);
            flipped = true;
        }
        else
        {
            mater.DisableKeyword("_EMISSION");
            // Smoothly rotate to 40 degrees on the X axis over 0.5 seconds
            yield return RotateToAngle(0, 0.15f);
            flipped = false;
        }
    }
    
    private IEnumerator RotateToAngle(float targetAngle, float duration)
    {
        float startAngle = transform.localRotation.eulerAngles.x;
        float time = 0;

        while (time < duration)
        {
            float angle = Mathf.Lerp(startAngle, targetAngle, time / duration);
            transform.localRotation = Quaternion.Euler(angle, 0, 0);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(targetAngle, 0, 0);
    }

    public void updateMaterials()
    {
        oldmat = meshRenderer.material;
        meshRenderer.material = mat;
        Material mater = meshRenderer.material;
        if (flipped)
        {
            mater.EnableKeyword("_EMISSION");
            
        }else if(!flipped)
        {
            mater.DisableKeyword("_EMISSION");
        }
    }
    
    public void revertMaterials()
    {
        meshRenderer.material = oldmat;
        Material mater = meshRenderer.material;
        if (flipped)
        {
            mater.EnableKeyword("_EMISSION");
            
        }else if(!flipped)
        {
            mater.DisableKeyword("_EMISSION");
        }
    }
}