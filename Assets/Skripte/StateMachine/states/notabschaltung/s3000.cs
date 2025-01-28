using UnityEngine;

public class s3000 : StateMachineBehaviour
{

    /*  script: notabschaltung
        set Control rods to 100 */

    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();        
        gazeGuidingPathPlayer.DirectionCueEnabled = true; // Roten Rand Deaktivieren        


        // state specific

        gazeGuidingPathPlayer.SetGazeGuidingClipboard("POS3");
        gazeGuidingPathPlayer.HighlightClipboard(1);
        target = GameObject.Find("ModPos").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("ModPos", target.GetComponent<GazeGuidingTarget>().isTypeOf);
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
