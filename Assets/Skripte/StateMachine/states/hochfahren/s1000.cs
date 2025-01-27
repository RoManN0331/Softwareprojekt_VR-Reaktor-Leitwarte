using UnityEngine;

public class s1000 : StateMachineBehaviour
{

    /*  Skript: hochfahren
        open SV2 valve      */

    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler palte
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        gazeGuidingPathPlayer.DirectionCueEnabled = false; // Roten Rand Deaktivieren        


        // state specific

        gazeGuidingPathPlayer.SetGazeGuidingClipboard("POS1");
        gazeGuidingPathPlayer.HighlightClipboard(1);
        target = GameObject.Find("SV2").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("SV2", target.GetComponent<GazeGuidingTarget>().isTypeOf);

    }

    /* 
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        see GazeGuidingPathPlayer.updade()
    }
    */

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.removeHighlightFromClipboard();        
    }
}
