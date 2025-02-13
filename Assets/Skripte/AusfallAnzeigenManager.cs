using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class implements logic to manage multiple displays signalling to the player whether specific components in the simulation have failed.
/// </summary>
public class AusfallAnzeigenManager : MonoBehaviour
{
    //Aus einem anderen Skript aufrufbar mit z.B.:
    //AusfallAnzeigenManager ausfallAnzeigenManager = FindObjectOfType<AusfallAnzeigenManager>();
    //ausfallAnzeigenManager.TurnOn("WP1");
    //dadurch wird dann die lampe WP1 eingeschaltet
    //die restlichen eingaben wären RKS,RKT,KNT,TBN,WP1,WP2,CP und AU
    
    /// <param name="ausfallAnzeigen"> is a list of displays signalling whether the assigned components have failed</param>
    public List<AusfallAnzeige> ausfallAnzeigen;
    /// <param name="toChange"> is a colour used for lamps</param>
    public Color toChange = Color.red;


    /// <summary>
    /// This method switches a display on if the corresponding component has failed.
    /// </summary>
    /// <param name="name">specifies a name of a component that has failed</param>
    public void TurnOn(string name)
    {
        foreach (var anzeige in ausfallAnzeigen)
        {
            if (anzeige.name == name)
            {
                anzeige.turnOn();
                break;
            }
        }
    }

    /// <summary>
    /// This method switches a display off.
    /// </summary>
    /// <param name="name"> specifies a name of a component with an assigned display</param>
    public void TurnOff(string name)
    {
        foreach (var anzeige in ausfallAnzeigen)
        {
            if (anzeige.name == name)
            {
                anzeige.turnOff();
                break;
            }
        }
    }

    /// <summary>
    /// This method switches all displays on that were added to ausfallAnzeigen.
    /// </summary>
    public void TurnAllOn()
    {
        foreach (var anzeige in ausfallAnzeigen)
        {
            anzeige.turnOn();
        }
        
        //Zusätzlich noch Lampen im Raum rot machen
        SetAllLampsToRed();
    }

    /// <summary>
    /// This method switches all displays off that were added to ausfallAnzeigen.
    /// </summary>
    public void TurnAllOff()
    {
        foreach (var anzeige in ausfallAnzeigen)
        {
            anzeige.turnOff();
        }
        SetAllLampsToWhite();
    }  

    /// <summary>
    /// This method sets the colour of all lamps to red. 
    /// </summary>
    public void SetAllLampsToRed()
    {
        Light[] lamps = FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light lamp in lamps)
        {
            lamp.color = toChange;
        }
    }
    
    /// <summary>
    /// This method sets the colour of all lamps to white.
    /// </summary>
    public void SetAllLampsToWhite()
    {
        Light[] lamps = FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light lamp in lamps)
        {
            lamp.color = Color.white;
        }
    }    
}