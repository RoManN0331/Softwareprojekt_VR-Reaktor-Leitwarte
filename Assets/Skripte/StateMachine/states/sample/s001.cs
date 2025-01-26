using UnityEngine;

public class s001 : StateMachineBehaviour
{
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;
    private GazeGuidingPathPlayerSecondPath gazeGuidingPathPlayer2;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Diese vier Zeilen Code bei jedem State hinzufügen
        gazeGuidingPathPlayer = FindObjectOfType<GazeGuidingPathPlayer>();
        gazeGuidingPathPlayer2 = FindObjectOfType<GazeGuidingPathPlayerSecondPath>();
        
        gazeGuidingPathPlayer.ClearLine();
        gazeGuidingPathPlayer2.ClearLine();
        
        gazeGuidingPathPlayer.TriggerTargetNAME("CPRPM", GazeGuidingTarget.TargetType.Genau);
    }

}
