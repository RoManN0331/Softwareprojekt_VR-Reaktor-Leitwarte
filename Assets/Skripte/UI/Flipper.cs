using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// This class implements logic to enable or disable gaze-guiding features via corresponding switches on a gaze-guiding panel in the VR application.
/// </summary>
public class Flipper : MonoBehaviour
{
    /// <param name="meshrenderer">is a reference to an objetc's MeshRenderer</param>
    private MeshRenderer meshRenderer;
    
    /// <param name="gazeGuidingButtons">is a reference to a GazeGuidingButtons instance</param>
    GazeGuidingButtons gazeGuidingButtons;
    
    /// <param name="mat">is a Material currently assigned to a switch</param>
    public Material mat;
    /// <param name="oldmat"> is a Material previously assigned to a switch</param>
    public Material oldmat;
    
    
    // IN INSPECTOR ONLY SELECT ONE OF THE FOLLOWING OPTIONS

    /// <param name="DirectionCueEnabled"> tracks whether the direction cue feature is switched on</param>
    public bool DirectionCueEnabled = false;
    /// <param name="DirectionArrowEnabled"> tracks whether the direction arrow feature is switched on</param>
    public bool DirectionArrowEnabled = false;    
    /// <param name="DirectionArrowOnScreen"> tracks whether the direction arrow is shown on screen</param>
    public bool DirectionArrowOnScreen = false;
    /// <param name="Arrow3DEnabled"> tracks whether the continuous 3D arrow feature is switched on</param>
    public bool Arrow3DEnabled = false;
    /// <param name="Arrow3DBinearEnabled"> tracks whether the binary 3D arrow feature is switched on</param>
    public bool Arrow3DBinearEnabled = false;
    /// <param name="AnzeigenMarkierungEnabled"> tracks whether display annotations are switched on</param>
    public bool AnzeigenMarkierungEnabled = false;
    /// <param name="HUDEnabled">tracks whether the HUD is enabled</param>
    public bool HUDEnabled = false;
    /// <param name="ifHUDEnabledShouldItBeOn"> tracks whether the HUD should be on</param>
    public bool ifHUDEnabledShouldItBeOn = false;
    /// <param name="DetachEnabled"> tracks whether the detach feature is switched on</param>
    public bool DetachEnabled = false;
    /// <param name="distractionsEnabled"> tracks whether distractions are switched on </param>
    public bool distractionsEnabled = false;
    /// <param name="ifdistractionsEnabledShouldItBeOn"> tracks whether distractions should be on</param>
    public bool ifdistractionsEnabledShouldItBeOn = false;
    /// <param name="BlurEnabled"> tracks whether the blur feature is switched on</param>
    public bool BlurEnabled = false;
    /// <param name="clipboardHighlight"> tracks whether the clipboard highlight feature is switched on</param>
    public bool clipboardHighlight;
    /// <param name="clipboardHighlightShouldItBeOn"> tracks whether clipboard highlights should be on</param>
    public bool clipboardHighlightShouldItBeOn = false;
    /// <param name="AnzeigenHighlight"=> tracks whether the display highlight feature is switched on</param>
    public bool AnzeigenHighlight = false;
    /// <param name="AnzeigenHighlightShouldItBeOn"=> tracks whether the display highlight feature should be on </param>    
    public bool AnzeigenHighlightShouldItBeOn = false;

    /// <param name="flipped"> tracks whether a switch is flipped</param>    
    private bool flipped = false;
    
    
    /// <summary>
    /// This method sets up input actions and initialises the gaze-guiding features' states based on the settings in the GazeGuidingPathPlayer instance.
    /// </summary>
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

    /// <param name="trigger"> is an InputActionReference referencing a XRI Right Interaction/Select</param>
    public InputActionReference trigger;
    
    /// <summary>
    /// This method updates the state of the gaze-guiding features based on the player's interaction with the switches on the gaze-guiding panel.
    /// </summary>
    private void Update()
    {
        if (isHovering && !isCooldown && trigger.action.triggered)
        {
            StartCoroutine(Flip());
        }
    }
    
    
    /// <param name="isCooldown"> tracks whether a switch is on cooldown</param>
    private bool isCooldown = false;
    /// <param name="isHovering">tracks whether the player is hovering over a switch with the controller</param>
    private bool isHovering = false;

    /// <summary>
    /// This method is called when the object is enabled and adds event listeners for the selectEntered and selectExited events.
    /// </summary>   
    private void OnEnable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    /// <summary>
    /// This method is called when the object is disabled and removes event listeners for the selectEntered and selectExited events.
    /// </summary>
    private void OnDisable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.RemoveListener(OnSelectEntered);
        interactable.selectExited.RemoveListener(OnSelectExited);
    }

    /// <summary>
    /// This method is called when the XR interactor looks at the object, i.e. points to or moves onto the object .
    /// </summary>
    /// <param name="args"> passes event specific arguments upon entering the interaction</param>
    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        isHovering = true;
    }

    /// <summary>
    /// This method is called when the XR interactor is no longer looking at the object, i.e. the interactor moves away.
    /// </summary>
    /// <param name="args"> passes event specific arguments upon exiting the interaction</param>
    private void OnHoverExited(HoverExitEventArgs args)
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
    
    /// <summary>
    /// This method toggles gaze-guiding features on or off when the player flips the appropriate switch on the gaze-guiding panel by calling the corresponding method in gazeGuidingButtons (live switch). 
    /// </summary>
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
            if (distractionsEnabled) FindAnyObjectByType<disableDistractions>().disableDistraction(false);
            if (clipboardHighlight)
            {
                GazeGuidingPathPlayer pathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
                pathPlayer.ClipBoardTextColor = "<color=#000000>";
                pathPlayer.removeHighlightFromClipboardForButton();
            }

            if (AnzeigenHighlight)
            {
                GazeGuidingPathPlayer pathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
                pathPlayer.DisplayHighlightEnabled = false;
                pathPlayer.unsetDisplayHighlight();
            }
        }
        
        yield return new WaitForSeconds(1f);

        isCooldown = false;
    }

    /// <summary>
    /// This method rotates a switch the player has flipped on the gaze-guiding panel without making a call to a method in gazeGuidingButtons (dummy switch).
    /// </summary>
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

    /// <summary>
    /// This method is called to rotate a switch on the gaze-guiding panel.
    /// </summary>
    /// <param name="targetAngle"> specifies the angle to rotate a switch to</param>
    /// <param name="duration"> specifies the duration of the rotation of a switch</param>
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

    /// <summary>
    /// This method updates the materials of a switch that the player has flipped (red when on, grey when off).
    /// </summary>
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

    /// <summary>
    /// This method reverts the materials of a switch that the player has flipped to their original state.
    /// </summary>    
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