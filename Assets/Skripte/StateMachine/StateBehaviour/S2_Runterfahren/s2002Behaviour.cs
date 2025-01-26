using System;
using TMPro;
using UnityEngine;

public class s2002Behaviour : StateMachineBehaviour
{
    GameObject clipboard;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        clipboard = GameObject.Find("POS2");
        TextMeshPro clipboardText = clipboard.transform.Find("Clipboard/TEXT").GetComponent<TextMeshPro>();    
        
        GazeGuidingClipboard GGClipboard = new GazeGuidingClipboard(clipboardText.text);
        GGClipboard.HighlightTask(3);
        clipboardText.text = GGClipboard.GetFormattedClipboardText();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
