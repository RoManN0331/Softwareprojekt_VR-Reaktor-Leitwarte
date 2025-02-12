using UnityEngine;

public class s3005 : StateMachineBehaviour
{
    
    /*  script: notabschaltung
        close SV1, SV2, WV1 and WV2 valve   */

    private bool WVstatus;
    private bool SVstatus;
    private GameObject target;
    private GameObject target2;
    private GazeGuidingPathPlayer gazeGuidingPathPlayer;
    private GazeGuidingPathPlayerSecondPath gazeGuidingPathPlayer2;

    private NPPReactorState simulation;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        simulation = FindAnyObjectByType<NPPClient>().simulation;
        
        // boiler plate
        gazeGuidingPathPlayer = FindAnyObjectByType<GazeGuidingPathPlayer>();
        gazeGuidingPathPlayer2 = FindObjectOfType<GazeGuidingPathPlayerSecondPath>();            
        gazeGuidingPathPlayer.DirectionCueEnabled = false; // Roten Rand Deaktivieren      
        
        gazeGuidingPathPlayer.removeHighlightFromClipboard();
          

        // state specific
        gazeGuidingPathPlayer.HighlightClipboard(6);
        target = GameObject.Find("WV1").gameObject;
        target2 = GameObject.Find("WV2").gameObject;
        gazeGuidingPathPlayer.TriggerTargetNAME("WV1", target.GetComponent<GazeGuidingTarget>().isTypeOf, true);
        gazeGuidingPathPlayer2.TriggerTargetNAME("WV2", target2.GetComponent<GazeGuidingTarget>().isTypeOf, true);

        WVstatus = true;

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WV1", true);
            gazeGuidingPathPlayer.ToggleBlur("WV2", true);
            gazeGuidingPathPlayer.ToggleBlur("SV1", true);
            gazeGuidingPathPlayer.ToggleBlur("SV2", true);
        }

        if (gazeGuidingPathPlayer.detached)
            {
                gazeGuidingPathPlayer.ToggleObjectVisibility("WV1", true);
                gazeGuidingPathPlayer.ToggleObjectVisibility("WV2", true);
                gazeGuidingPathPlayer.ToggleObjectVisibility("SV1", true);
                gazeGuidingPathPlayer.ToggleObjectVisibility("SV2", true);
            }

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Zuerst WV1 und WV2 schließen danach SV1 und SV2 schließen
        if (simulation.WV1.status && simulation.WV2.status)
        {
            if (!WVstatus)
            {
                target = GameObject.Find("WV1").gameObject;
                target2 = GameObject.Find("WV2").gameObject;
                gazeGuidingPathPlayer.TriggerTargetNAME("WV1", target.GetComponent<GazeGuidingTarget>().isTypeOf, true);
                gazeGuidingPathPlayer2.TriggerTargetNAME("WV2", target2.GetComponent<GazeGuidingTarget>().isTypeOf, true);
                SVstatus = false;
                WVstatus = true;
            }
            
        }else{
            if (!SVstatus)
            {
                target = GameObject.Find("SV1").gameObject;
                target2 = GameObject.Find("SV2").gameObject;
                gazeGuidingPathPlayer.TriggerTargetNAME("SV1", target.GetComponent<GazeGuidingTarget>().isTypeOf, true);
                gazeGuidingPathPlayer2.TriggerTargetNAME("SV2", target2.GetComponent<GazeGuidingTarget>().isTypeOf, true);
                SVstatus = true;
                WVstatus = false;
            }
           
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gazeGuidingPathPlayer.unsetDisplayHighlight();

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("WV1", false);
            gazeGuidingPathPlayer.ToggleBlur("WV2", false);
            gazeGuidingPathPlayer.ToggleBlur("SV1", false);
            gazeGuidingPathPlayer.ToggleBlur("SV2", false);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("WV1", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("WV2", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("SV1", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("SV2", false);
        }
    }
    
}
