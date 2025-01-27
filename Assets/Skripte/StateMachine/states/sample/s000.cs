using UnityEngine;

public class s000 : StateMachineBehaviour
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
        
        
        //ERSTER Pfeil
        gazeGuidingPathPlayer.TriggerTargetNAME("SV2", GazeGuidingTarget.TargetType.Binaer);
        gazeGuidingPathPlayer.DirectionCueEnabled = false; // Roten Rand Deaktivieren
        
        //ZWEITER Pfeil  (Besitzt keinen roten Rand, das wird nur vom ersten Pfeil gesteuert)
        gazeGuidingPathPlayer2.TriggerTargetNAME("WV1", GazeGuidingTarget.TargetType.Binaer);
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.DirectionCueEnabled = true; // Roten Rand für den nächsten State aktivieren
        
        
        // Der allerletzte State der StateMachine sollte diesen Code besitzen, damit die Pfeile verschwinden
        // nicht mitten in den States, da sonst Pfeile verschwinden (nicht synchronisiert)
        /*
        gazeGuidingPathPlayer.ClearLine();
        gazeGuidingPathPlayer2.ClearLine();
        */
        
    }
}
