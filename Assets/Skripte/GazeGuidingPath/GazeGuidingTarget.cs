using UnityEngine;

public class GazeGuidingTarget : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TargetType isTypeOf;
    
    public enum TargetType
    {
        Genau,
        Binaer,
        Anzeige,
        Ausfallanzeige
    }
}
