using UnityEngine;

/// <summary>
/// This class is used to define objects within the scene as targets for GazeGuidingPathPlayer and GazeGuidingPathPlayerSecondPath.
/// </summary>
public class GazeGuidingTarget : MonoBehaviour
{



    /// <param name="isTypeOf"> Enum defining the type of the object the script is attached to </param>
    public TargetType isTypeOf;
    
    public enum TargetType
    {
        Genau,
        Binaer,
        Anzeige,
        Ausfallanzeige
    }
    
    
    
    
    
}
