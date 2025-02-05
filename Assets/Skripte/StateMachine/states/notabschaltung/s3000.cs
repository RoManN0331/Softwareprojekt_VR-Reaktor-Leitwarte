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
        gazeGuidingPathPlayer.TriggerTargetNAME("ModPos", target.GetComponent<GazeGuidingTarget>().isTypeOf, true);
        
        // show arrows
        gazeGuidingPathPlayer.TriggerAnzeigenMarkierung("ModPos_display", GazeGuidingTarget.TargetType.Anzeige, 0);

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("ModPos", true);
            gazeGuidingPathPlayer.ToggleBlur("controlRods", true);
            gazeGuidingPathPlayer.ToggleBlur("CPressure", true);
            gazeGuidingPathPlayer.ToggleBlur("RWaterLvl", true);
            gazeGuidingPathPlayer.ToggleBlur("Energy", true);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("ModPos", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("controlRods", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPressure", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RWaterLvl", true);
            gazeGuidingPathPlayer.ToggleObjectVisibility("Energy", true);
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
        gazeGuidingPathPlayer.ClearAnzeigenMarkierung();
        gazeGuidingPathPlayer.unsetDisplayHighlight();

        if (gazeGuidingPathPlayer.blur)
        {
            gazeGuidingPathPlayer.ToggleBlur("ModPos", false);
            gazeGuidingPathPlayer.ToggleBlur("controlRods", false);
            gazeGuidingPathPlayer.ToggleBlur("CPressure", false);
            gazeGuidingPathPlayer.ToggleBlur("RWaterLvl", false);
            gazeGuidingPathPlayer.ToggleBlur("Energy", false);
        }

        if (gazeGuidingPathPlayer.detached)
        {
            gazeGuidingPathPlayer.ToggleObjectVisibility("ModPos", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("controlRods", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("CPressure", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("RWaterLvl", false);
            gazeGuidingPathPlayer.ToggleObjectVisibility("Energy", false);
        }
    }
    
}
