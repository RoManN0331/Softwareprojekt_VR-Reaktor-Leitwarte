using UnityEngine;

public class s1002 : StateMachineBehaviour
{

    /*  Skript: hochfahren
        open WV1 valve      */

    private GameObject target;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();        
        gazeGuidingPathPlayer.DirectionCueEnabled = true; // Roten Rand Deaktivieren        

        gazeGuidingPathPlayer.removeHighlightFromClipboard();  
        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
        
        // state specific

        gazeGuidingPathPlayer.HighlightClipboard(3);
        
        target = GameObject.Find("WV1").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("WV1", target.GetComponent<GazeGuidingTarget>().isTypeOf);

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WV1", true);
        }

        if (gazeGuidingPathPlayer.detached)
            {
                gazeGuidingPathPlayer.ToggleObjectVisibility("WV1", true);
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
            gazeGuidingPathPlayer.ToggleBlur("WV1", false);
        }

        if (gazeGuidingPathPlayer.detached)
            {
                gazeGuidingPathPlayer.ToggleObjectVisibility("WV1", false);
            }

    }

}
