using UnityEngine;

public class s1006 : StateMachineBehaviour
{

    /*  Skript: hochfahren
        close SV2 valve      */
    
    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();        
        gazeGuidingPathPlayer.DirectionCueEnabled = false; // Roten Rand Deaktivieren        


        // state specific

        gazeGuidingPathPlayer.HighlightClipboard(7);
        target = GameObject.Find("SV2").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("SV2", target.GetComponent<GazeGuidingTarget>().isTypeOf);        
    }

    /*

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    */

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.removeHighlightFromClipboard();
    }
}
