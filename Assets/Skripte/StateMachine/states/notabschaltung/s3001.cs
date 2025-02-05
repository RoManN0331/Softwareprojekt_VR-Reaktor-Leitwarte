using UnityEngine;

public class s3001 : StateMachineBehaviour
{
    /*  script: notabschaltung
        open WV2 valve          */

    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {    
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();        
        gazeGuidingPathPlayer.DirectionCueEnabled = true; // Roten Rand Deaktivieren        

        gazeGuidingPathPlayer.removeHighlightFromClipboard();
          
        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(2);

        target = GameObject.Find("WV2").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("WV2", target.GetComponent<GazeGuidingTarget>().isTypeOf);
    
        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WV2", true);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("WV2", true);
        }
    }

    /*
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        see gazeGuidingPathPlayer.Update();
    }
    */

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.unsetDisplayHighlight();

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WV2", false);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("WV2", false);
        }
    }
    
}
