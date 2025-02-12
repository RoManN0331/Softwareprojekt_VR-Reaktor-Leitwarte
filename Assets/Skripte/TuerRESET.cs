using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// This class implements logic to reset the simulation by using the door handle in the scene of the VR application.
/// </summary>
public class TuerRESET : MonoBehaviour
{



    /// <param name="isCooldown">boolean checking if the handle has been used recently </param>
    private bool isCooldown = false;
    /// <param name="isHovering">boolean checking if the XR interactor is hovering over the handle </param>    
    private bool isHovering = false;
    /// <param name="trigger">Reference to an input action</param>
    public InputActionReference trigger;
    /// <param name="nppClient">Reference to the NPPClient instance in the scene</param>
    NPPClient nppClient;   


    /// <summary>
    /// This method intiates the InputActionReference trigger.
    /// </summary>
    private void Start()
    {
        nppClient = FindObjectOfType<NPPClient>();
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
    }

    /// <summary>
    /// This method checks if the door is being hovered over and if the trigger is being pressed for each frame.
    /// </summary>
    private void Update()
    {
        if (isHovering && !isCooldown && trigger.action.triggered)
        {
            StartCoroutine(HandleDoorInteraction());
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
    /// This method is called when the player interacts with the door handle via the XR interactor to reset the simulation.
    /// </summary>
    private IEnumerator HandleDoorInteraction()
    {
        isCooldown = true;
        // Reset the simulation
        StartCoroutine(nppClient.ResetSimulation());

        FindAnyObjectByType<AusfallAnzeigenManager>().SetAllLampsToWhite();
        
        // Smoothly rotate to 40 degrees on the Z axis over 0.5 seconds
        yield return RotateToAngle(40, 0.35f);

        // Smoothly rotate back to 0 degrees on the Z axis over 0.5 seconds
        yield return RotateToAngle(0, 0.35f);

        // Wait for 1 second before allowing another interaction
        yield return new WaitForSeconds(1f);

        isCooldown = false;

        // RESET
    }

    /// <summary>
    /// This method is called in HandleDoorInteraction() to rotate the door handle.
    /// </summary>
    /// <param name="targetAngle"> float specifying the angle to rotate the door handle to</param>
    /// <param name="duration"> float specifying the duration of the rotation</param>
    private IEnumerator RotateToAngle(float targetAngle, float duration)
    {
        float startAngle = transform.localRotation.eulerAngles.z;
        float time = 0;

        while (time < duration)
        {
            float angle = Mathf.Lerp(startAngle, targetAngle, time / duration);
            transform.localRotation = Quaternion.Euler(0, 0, angle);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(0, 0, targetAngle);
    }
}