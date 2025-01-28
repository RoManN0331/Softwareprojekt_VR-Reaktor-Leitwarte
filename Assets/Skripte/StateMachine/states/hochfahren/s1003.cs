using UnityEngine;

public class s1003 : StateMachineBehaviour
{

    /*  Skript: hochfahren
        set WP1RPM to 200   */

    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();        
        gazeGuidingPathPlayer.DirectionCueEnabled = true; // Roten Rand Deaktivieren        


        // state specific

        gazeGuidingPathPlayer.HighlightClipboard(4);
        target = GameObject.Find("WP1RPM").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("WP1RPM", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("WP1RPM_display", GazeGuidingTarget.TargetType.Anzeige, 200);
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
