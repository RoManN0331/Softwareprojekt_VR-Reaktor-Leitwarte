using UnityEngine;

public class s2005 : StateMachineBehaviour
{

    /*  script: herunterfahren
        set CPRPM to 0          */

    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler palte
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        gazeGuidingPathPlayer.DirectionCueEnabled = true; // Roten Rand Deaktivieren        


        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(6);
        target = GameObject.Find("CPRPM").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("CPRPM", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("CPRPM_display", GazeGuidingTarget.TargetType.Anzeige, 0);
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
        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
    }
    
}
