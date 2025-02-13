using UnityEngine;

/// <summary>
/// This class is used to define objects within the scene as targets for GazeGuidingPathPlayer and GazeGuidingPathPlayerSecondPath.
/// </summary>
public class GazeGuidingTarget : MonoBehaviour
{
    /// <param name="isTypeOf"> is an Enum defining the type of a object the script is attached to </param>
    public TargetType isTypeOf;
    
    /// <summary>
    /// This Enum specifies the type of an object GazeGuidingTarget may be applied to as an exact rotary switch (Genau), a binary rotary switch (Binaer), a display (Anzeige) or a display indicating the failure of a component (AusfallAnzeige). 
    /// </summary>
    public enum TargetType
    {
        Genau,
        Binaer,
        Anzeige,
        Ausfallanzeige
    }
    
    
    
    
    
}
