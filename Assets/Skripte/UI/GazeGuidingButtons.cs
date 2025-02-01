using UnityEngine;

public class GazeGuidingButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private GazeGuidingPathPlayer pathPlayer;
    private GazeGuidingPathPlayerSecondPath pathPlayer2;
    void Start()
    {
        pathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        pathPlayer2 = FindAnyObjectByType<GazeGuidingPathPlayerSecondPath>();
    }

    public void DirectionCue(bool TurnOn)
    {
        pathPlayer.DirectionCueEnabled = TurnOn;
    }


    public void DirectionArrow(bool TurnOn)
    {
        pathPlayer.DirectionArrowEnabled = TurnOn;
        pathPlayer2.DirectionArrowEnabled = TurnOn;
    }

    public void Arrow3D(bool TurnOn)
    {
        pathPlayer.Arrow3DEnabled = TurnOn;
        pathPlayer2.Arrow3DEnabled = TurnOn;
    }

    public void Arrow3DBinear(bool TurnOn)
    {
        pathPlayer.Arrow3DBinearEnabled = TurnOn;
        pathPlayer2.Arrow3DBinearEnabled = TurnOn;
    }

    public void DirectionArrowOnScreen(bool TurnOn)
    {
        pathPlayer.DirectionArrowOnScreen = TurnOn;
    }

    public void AnzeigenMarkierung(bool TurnOn)
    {
        pathPlayer.AnzeigenMarkierungEnabled = TurnOn;
    }
}
