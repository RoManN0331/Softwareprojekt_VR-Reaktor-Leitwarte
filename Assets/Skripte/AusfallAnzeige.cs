using System;
using UnityEngine;

/// <summary>
/// This class implements logic for a single display to signal the failure of a component in the simulation to the player.
/// </summary>
public class AusfallAnzeige : MonoBehaviour
{
    /// <param name="isAusgefallen"> tracks whether a component has failed (is blown)</param>
    public bool isAusgefallen = false;
    /// <param name="transparentOn"> is a Material that is rendered when the display is switched on</param>
    public Material transparentOn;
    /// <param name="transparentOff"> is a Material that is rendered when the display is switched off</param>
    public Material transparentOff;
    /// <param name="meshRenderer"> is a reference to an object's MeshRenderer</param>
    public MeshRenderer meshRenderer;
    /// <param name="previousState"> tracks whether a component was broken in the previous frame</param>
    private bool previousState;

    public string name = "WP1";         // deprecated

    /// <summary>
    /// This method initialises the display for the component this script is attached to.
    /// </summary>
    private void Start()
    {
        previousState = isAusgefallen;
        UpdateMaterial();
    }

    /// <summary>
    /// This method updates the display by checking whether the component the script is attached to has failed.
    /// </summary>
    private void Update()
    {
        if (isAusgefallen != previousState)
        {
            UpdateMaterial();
            previousState = isAusgefallen;
        }
    }

    /// <summary>
    /// This method updates the material of the display based on whether the component the script is attached to has failed
    /// </summary>
    private void UpdateMaterial()
    {
        Material[] materials = meshRenderer.materials;
        if (isAusgefallen)
        {
            materials[1] = transparentOn;
        }
        else
        {
            materials[1] = transparentOff;
        }
        meshRenderer.materials = materials;
    }

    /// <summary>
    /// This method is called when the component to which the script is attached to fails, triggering the display to switch on.
    /// </summary>
    public void turnOn() // wenn die Komponente ausfällt soll die Methode turnOn() aufgerufen werden
    {
        isAusgefallen = true;
    }
    
    /// <summary>
    /// This method triggers the display to switch off.
    /// </summary>
    public void turnOff()
    {
        isAusgefallen = false;
    }
}
