using UnityEngine;

public class s1001 : StateMachineBehaviour
{

    /*  Skript: hochfahren
        set CPRPM to 1600   */

    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();        
        gazeGuidingPathPlayer.DirectionCueEnabled = true; // Roten Rand Deaktivieren        


        // state specific

        gazeGuidingPathPlayer.HighlightClipboard(2);
        target = GameObject.Find("CPRPM").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("CPRPM", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("CPRPM_display", GazeGuidingTarget.TargetType.Anzeige, 1600);
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
