using UnityEngine;

public class s3004 : StateMachineBehaviour
{
    
    /*  script: notabschaltung
        set Condenser pressure to 0 */

    private GameObject target;
    //private GameObject target2;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;
    //private GazeGuidingPathPlayerSecondPath gazeGuidingPathPlayer2;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        //gazeGuidingPathPlayer2 = FindObjectOfType<GazeGuidingPathPlayerSecondPath>();            
        gazeGuidingPathPlayer.DirectionCueEnabled = false; // Roten Rand Deaktivieren        

        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
        gazeGuidingPathPlayer.removeHighlightFromClipboard();  
        
        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(5);
        
        target = GameObject.Find("CPressure").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("CPressure", target.GetComponent<GazeGuidingTarget>().isTypeOf);
        
        // show arrows
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("CPressure", GazeGuidingTarget.TargetType.Anzeige, 0);
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("RPressure", GazeGuidingTarget.TargetType.Anzeige, 0);
    
        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("RPressure", true);
            gazeGuidingPathPlayer.ToggleBlur("CPressure", true);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("RPressure", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPressure", true);
        }
    
    }

    /*
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        see gazeGuidingPathPlayer.Update();
        and
        see gazeGuidingPathPlayer2.Update();        
    }
    */

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
        gazeGuidingPathPlayer.unsetDisplayHighlight();

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("RPressure", false);
            gazeGuidingPathPlayer.ToggleBlur("CPressure", false);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("RPressure", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPressure", false);
        }
    }
    
}
