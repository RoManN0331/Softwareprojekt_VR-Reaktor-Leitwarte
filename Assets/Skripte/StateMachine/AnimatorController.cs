using System.Linq;
using UnityEngine;

/// <summary>
/// This class is used to update the animator parameters based on the state of the simulation stored in an NPPReactorState object.
/// </summary>
public class AnimatorController : MonoBehaviour
{
    ///<param name="animator"> is a reference to the statemachine</param>
    private Animator animator;
    ///<param name="clientScript"> is a reference to the NPPClient instance in the scene</param>
    private NPPClient clientScript;

    /// <summary>
    /// This method initialises animator with a reference to the statemachine .
    /// </summary>
    void Awake()
    {
        clientScript = GameObject.Find("NPPclientObject").GetComponent<NPPClient>();

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found!");
        }
    }

    /// <summary>
    /// This method updates the statemachine parameters based on the state of the simulation stored in an NPPReactorState object.
    /// </summary>
    public void UpdateAnimatorParameters(NPPReactorState state)
    {
        //Set System parameter
        animator.SetBool("SimRunning", state.SystemStatus.running);
        //Set health parameter
        animator.SetBool("ReactorStatus",     GetComponent("RKS", state.ComponentHealth).broken);
        animator.SetBool("ReactorTankStatus", GetComponent("RKT", state.ComponentHealth).broken);
        animator.SetBool("CondenserStatus",   GetComponent("KNT", state.ComponentHealth).broken);
        animator.SetBool("TurbineStatus",     GetComponent("TBN", state.ComponentHealth).broken);
        animator.SetBool("WP1Status",         GetComponent("WP1", state.ComponentHealth).broken);
        animator.SetBool("WP2Status",         GetComponent("WP2", state.ComponentHealth).broken);
        animator.SetBool("CPStatus",          GetComponent("CP", state.ComponentHealth).broken);
        animator.SetBool("AtomicStatus",      GetComponent("AU", state.ComponentHealth).broken); 
        //Set valve parameter
        animator.SetBool("SV1Status", state.SV1.status);
        animator.SetBool("SV2Status", state.SV2.status);
        animator.SetBool("WV1Status", state.WV1.status);
        animator.SetBool("WV2Status", state.WV2.status);
        //Set pump parameter
        animator.SetInteger("WP1RPM", (int) state.WP1.rpm);
        animator.SetInteger("WP2RPM", (int) state.WP2.rpm);
        animator.SetInteger("CPRPM",  (int) state.CP.rpm);
        //Set reactor parameter
        animator.SetInteger("ControlRodsLvl",  (int) state.Reactor.rodPosition);
        animator.SetInteger("ReactorWaterLvl", (int) state.Reactor.waterLevel);
        animator.SetInteger("ReactorPressure", (int) state.Reactor.pressure);
        animator.SetInteger("Restheat",        (int) state.Reactor.restheat);
        //Set condenser parameter
        animator.SetInteger("CondenserWaterLvl", (int) state.Condenser.waterLevel);
        animator.SetInteger("CondenserPressure", (int) state.Condenser.pressure);
        //set generator parameter
        animator.SetInteger("GeneratorOutput", (int) state.Generator.power);

        
    }

    /// <summary>
    /// This method updates the scenario parameter of the statemachine.
    /// </summary>
    /// <param name="scenario"> specifies the scenario the player has chosen</param>
    public void updateScenario(int scenario){

        /* update the chosen scenario */

        animator.SetInteger("scenario", scenario);
    }

    /// <summary>
    /// This method resets the statemachine to the initial state.
    /// </summary>
    public void Reset(){
        animator.Play("initial", 0, 0);
    }

    /// <summary>
    /// This method returns the information stored in ComponentHealt for a component or initialises a new ComponentState.
    /// </summary>
    /// <param name="name"> is the name of a component</param>
    /// <param name="health"> is a ComponentHealth object containing the state of various components</param>

    private ComponentState GetComponent(string name, ComponentHealth health)
    {
        ComponentState component = health.components.FirstOrDefault(c => c.name == name);
        return (component == null) ? new ComponentState { name = name, status = false } : component;
    }

    /// <summary>
    /// This method returns the scenario the state machine is currently running.
    /// </summary>
    public int getScenario()
    {
        switch (animator.GetInteger("scenario"))
        {
            case 0:
                return 0;
            case 1:
                return 1;
            case 2:
                return 2;
            case 3:
                return 3;
        }

        return 0;
    }
}