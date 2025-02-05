using UnityEngine;

public class s2001 : StateMachineBehaviour
{

    /*  script: herunterfahren
        open SV2 valve          */

    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler palte
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        gazeGuidingPathPlayer.DirectionCueEnabled = true; // Roten Rand Deaktivieren        
        gazeGuidingPathPlayer.removeHighlightFromClipboard();  

        
        
        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(2);
        
        target = GameObject.Find("SV2").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("SV2", target.GetComponent<GazeGuidingTarget>().isTypeOf);
    
        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("SV2", true);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("SV2", true);
        }    
    }

    /*
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        see GazeGuidingPathPlayer.updade()
    }
    */

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.unsetDisplayHighlight();

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("SV2", false);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("SV2", false);
        }        
    }
    
}
