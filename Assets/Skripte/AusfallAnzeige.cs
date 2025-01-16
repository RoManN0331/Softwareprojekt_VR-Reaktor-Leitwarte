using System;
using UnityEngine;

public class AusfallAnzeige : MonoBehaviour
{
    public string name = "WP1";
    public bool isAusgefallen = false;
    public Material transparentOn;
    public Material transparentOff;
    
    public MeshRenderer meshRenderer;
    private bool previousState;
    private void Start()
    {
        previousState = isAusgefallen;
        UpdateMaterial();
    }

    private void Update()
    {
        if (isAusgefallen != previousState)
        {
            UpdateMaterial();
            previousState = isAusgefallen;
        }
    }

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


    public void turnOn() // wenn die Komponente ausfällt soll die Methode turnOn() aufgerufen werden
    {
        isAusgefallen = true;
    }
    
    public void turnOff()
    {
        isAusgefallen = false;
    }
}
