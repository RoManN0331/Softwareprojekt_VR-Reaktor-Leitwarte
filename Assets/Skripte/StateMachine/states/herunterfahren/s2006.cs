using UnityEngine;

public class s2006 : StateMachineBehaviour
{
    
    /*  script: herunterfahren
        close SV2 valve         */
    
    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler palte
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        gazeGuidingPathPlayer.DirectionCueEnabled = true; // Roten Rand Deaktivieren        

        gazeGuidingPathPlayer.removeHighlightFromClipboard();  

        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(7);
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
        gazeGuidingPathPlayer.ClearLine();

        // reset the scenario
        FindObjectOfType<AnimatorController>().updateScenario(0);

    }
    
}
