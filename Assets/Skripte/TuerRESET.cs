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

public class TuerRESET : MonoBehaviour
{
    public InputActionReference trigger;

    private void Start()
    {
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

    private void Update()
    {
        if (isHovering && !isCooldown && trigger.action.triggered)
        {
            StartCoroutine(HandleDoorInteraction());
        }
    }
    
    
    private bool isCooldown = false;
    private bool isHovering = false;

    private void OnEnable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    private void OnDisable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        isHovering = true;
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        isHovering = false;
    }
    

    private IEnumerator HandleDoorInteraction()
    {
        isCooldown = true;

        // Smoothly rotate to 40 degrees on the Z axis over 0.5 seconds
        yield return RotateToAngle(40, 0.35f);

        // Smoothly rotate back to 0 degrees on the Z axis over 0.5 seconds
        yield return RotateToAngle(0, 0.35f);

        // Wait for 1 second before allowing another interaction
        yield return new WaitForSeconds(1f);

        isCooldown = false;

        // RESET
    }

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