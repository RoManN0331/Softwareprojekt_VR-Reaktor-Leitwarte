using UnityEngine;

public class s3002 : StateMachineBehaviour
{
    
    /*  script: notabschaltung
        set WP2RPM to 800       */

    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();        
        gazeGuidingPathPlayer.DirectionCueEnabled = false; // Roten Rand Deaktivieren        

        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(3);
        target = GameObject.Find("WP2RPM").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("WP2RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf);    
    }

    /*
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        see gazeGuidingPathPlayer.Update();
    }
    */


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.removeHighlightFromClipboard();
    }
    
}
