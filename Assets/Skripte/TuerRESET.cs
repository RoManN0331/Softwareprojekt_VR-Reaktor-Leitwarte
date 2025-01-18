using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TuerRESET : MonoBehaviour
{
    private XRSimpleInteractable interactable;
    private bool isCooldown = false;

    private void OnEnable()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!isCooldown)
        {
            StartCoroutine(HandleDoorInteraction());
        }
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