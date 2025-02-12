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
    // IN INSPECTOR ONLY SELECT ONE OF THE FOLLOWING OPTIONS
    
    /// <param name="DirectionCueEnabled">boolean tracking whether the direction cue feature is switched on</param>
    public bool DirectionCueEnabled = false;
    /// <param name="DirectionArrowEnabled">boolean tracking whether the direction arrow feature is switched on</param>
    public bool DirectionArrowEnabled = false;    
    /// <param name="DirectionArrowOnScreen">boolean tracking whether the direction arrow is shown on screen</param>
    public bool DirectionArrowOnScreen = false;
    /// <param name="Arrow3DEnabled">boolean tracking whether the continuous 3D arrow feature is switched on</param>
    public bool Arrow3DEnabled = false;
    /// <param name="Arrow3DBinearEnabled">boolean tracking whether the binary 3D arrow feature is switched on</param>
    public bool Arrow3DBinearEnabled = false;
    /// <param name="AnzeigenMarkierungEnabled">boolean tracking whether display annotations are switched on</param>
    public bool AnzeigenMarkierungEnabled = false;
    /// <param name="HUDEnabled">boolean tracking whether the HUD is enabled</param>
    public bool HUDEnabled = false;
    /// <param name="ifHUDEnabledShouldItBeOn">boolean tracking whether the HUD should be shown</param>
    public bool ifHUDEnabledShouldItBeOn = false;
    /// <param name="DetachEnabled">boolean tracking whether the detach feature is switched on</param>
    public bool DetachEnabled = false;
    /// <param name="distractionsEnabled">boolean tracking whether distractions are switched on (shown)</param>
    public bool distractionsEnabled = false;
    /// <param name="ifdistractionsEnabledShouldItBeOn">boolean tracking whether distractions should be shown</param>
    public bool ifdistractionsEnabledShouldItBeOn = false;
    /// <param name="BlurEnabled">boolean tracking whether the blur feature is switched on</param>
    public bool BlurEnabled = false;
    /// <param name="clipboardHighlight">boolean tracking whether the clipboard highlight feature is switched on</param>
    public bool clipboardHighlight;
    /// <param name="clipboardHighlightShouldItBeOn">boolean tracking whether clipboard highlights should be shown</param>
    public bool clipboardHighlightShouldItBeOn = false;
    /// <param name="flipped">boolean tracking whether a switch is flipped</param>
    private bool flipped = false;
    /// <param name="isCooldown">boolean tracking whether</param>
    private bool isCooldown = false;
    /// <param name="isHovering">boolean tracking whether</param>
    private bool isHovering = false;
    /// <param name="mat">Material for the switches based</param>
    public Material mat;
    /// <param name="oldmat">Material for the switches</param>
    public Material oldmat;
    /// <param name="trigger">InputActionReference referencing XRI Right Interaction/Select</param>
    public InputActionReference trigger;
    /// <param name="gazeGuidingButtons">Reference to a GazeGuidingButtons instance</param>
    GazeGuidingButtons gazeGuidingButtons;
    /// <param name="meshrenderer">Reference to an objetc's MeshRenderer</param>
    private MeshRenderer meshRenderer;




    /// <summary>
    /// This method sets up input actions and initialises the gaze-guiding features states based on the settings in the GazeGuidingPathPlayer instance.
    /// </summary>
    private void Start()
    {
        gazeGuidingButtons = FindAnyObjectByType<GazeGuidingButtons>();
        
         meshRenderer = GetComponent<MeshRenderer>();
         
         Material mater = meshRenderer.material;

         mater.DisableKeyword("_EMISSION");

        
        InputActionManager temp = FindObjectOfType<InputActionManager>();
        if (temp != null && temp.actionAssets.Count > 0)
        {
            foreach (var act in temp.actionAssets )
            {
                var action = act.FindAction("XRI Right Interaction/Select");
                trigger = InputActionReference.Create(action);
                break;
            }
        }
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
        
        if (clipboardHighlight && clipboardHighlightShouldItBeOn) StartCoroutine(Flip());

    }
        
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

    /// <summary>
    /// This method is called when the object is enabled and adds event listeners for the selectEntered and selectExited events.
    /// </summary>    
    private void OnEnable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    /// <summary>
    /// This method is called when the object is disabled and removes event listeners for the selectEntered and selectExited events.
    /// </summary>
    private void OnDisable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);
    }

    /// <summary>
    /// This method is called when the XR interactor looks at the object, i.e. points to / moves onto the object .
    /// </summary>
    /// <param name="args"> HoverEnterEventArgs to pass event specific arguments upon entering the interaction</param>
    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        isHovering = true;
    }

    /// <summary>
    /// This method is called when the XR interactor is no longer looking at the object, i.e. the interactor moves away.
    /// </summary>
    /// <param name="args"> HoverExitEventArgs to pass event specific arguments upon exiting the interaction</param>
    private void OnHoverExited(HoverExitEventArgs args)
    {
        isHovering = false;
    }

    /// <summary>
    /// This method toggles gaze-guiding features on or off when the player flips the appropriate switch on the gaze-guiding panel by calling the corresponding method in gazeGuidingButtons. 
    /// </summary>
    private IEnumerator Flip()
    {
        isCooldown = true;

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
            if (distractionsEnabled) FindAnyObjectByType<disableDistractions>().disableDistraction(true);
            if (clipboardHighlight)
            {
                GazeGuidingPathPlayer pathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
                pathPlayer.ClipBoardTextColor = "<color=#00FF00>";
                pathPlayer.removeHighlightFromClipboardForButton();
            }
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
        }
        
        yield return new WaitForSeconds(1f);

        isCooldown = false;
    }

    /// <summary>
    /// This method rotates a switch the the player has flipped on the gaze-guiding panel.
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
    /// This method is called to rotate the switch on the gaze-guiding panel.
    /// </summary>
    /// <param name="targetAngle"> float specifying the angle to rotate the switch to</param>
    /// <param name="duration"> float specifying the duration of the rotation</param>
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