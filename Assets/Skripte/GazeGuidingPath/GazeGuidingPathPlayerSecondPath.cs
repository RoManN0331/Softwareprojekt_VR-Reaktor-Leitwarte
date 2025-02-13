using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// This class implements logic for gaze guiding in a virtual environment.
/// <summary>
public class GazeGuidingPathPlayerSecondPath : MonoBehaviour
{
/// <param name="DirectionArrowEnabled"> toggles a directional arrow indicating a direction the player should turn to</param>
    public bool DirectionArrowEnabled = true;    
    /// <param name="Arrow3DEnabled"> toggles a 3D arrow indicating the direction an exact rotary switch should be turned in</param>
    public bool Arrow3DEnabled = true;
    /// <param name="Arrow3DBinearEnabled"> toggles 3D arrow indicating the direction a binary rotary switch should be turned in</param>
    public bool Arrow3DBinearEnabled = true;
    /// <param name="DirectionArrowOnScreen"> toggles the on screen display of the directional arrow</param>
    
    /// <param name="targets"> is a list of GazeGuidingTarget objects</param>
    public List<GazeGuidingTarget> targets;
    /// <param name="currentTarget"> is the currently active GazeGuidingTarget for this GazeGuidingPathPlayerSecondPath</param>
    private GazeGuidingTarget currentTarget;
    /// <param name="pathDisplayDistance">  specifies the distance between the player and an animated path that is displayed as visual cue </param>
    public float pathDisplayDistance = 5.0f;
    /// <param name="animationDuration"> specifies the duration of the path animation</param>
    public float animationDuration = 1.0f;
    /// <param name="lineMaterial">  is a Material used to render a path as visual cue</param>
    public Material lineMaterial; 
    /// <param name="lineRenderer"> is a LineRenderer object used to render a path as visual cue</param>
    private LineRenderer lineRenderer;
    /// <param name="animatePathCoroutine"> is a Coroutine used to animate a path as visual cue</param>
    private Coroutine animatePathCoroutine;
    /// <param name="isAnimating"> tracks whether a path is currently being animated as visual cue</param>
    private bool isAnimating = false;
    
    /// <param name="arrow3DPrefab"> is a reference to an Arrow3D prefab used to render continuous rotational 3DArrows</param>    
    private GameObject arrow3DPrefab; 
    /// <param name="arrow3DInstance"> is an instance of a Arrow3D prefab used to render continuous rotational 3DArrows<param>

    private GameObject arrow3DInstance; 
    /// <param name="arrow3DBinaerPrefab"> is a reference to an Arrow3D prefab used to render binary rotational 3DArrows</param>
    
    private GameObject arrow3DBinaerPrefab; 
    /// <param name="arrow3DBinaerInstance"> is an instance of an Arrow3D prefab used to render binary rotational 3DArrows<param>
    private GameObject arrow3DBinaerInstance; // Instance of the Arrow3D prefab
    

    /// <summary>
    /// This method initialises the GazeGuidingPathPlayerSecondPath. 
    /// </summary>
    void Start()
    {
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;

        lineRenderer.positionCount = 0;
        lineRenderer.material = lineMaterial; 

        // Set texture mode to Tile
        lineRenderer.textureMode = LineTextureMode.Tile;

        // Set texture scale
        lineRenderer.textureScale = new Vector2(2.36f, 0.57f);
        
        // Load the Arrow3D prefab
        arrow3DPrefab = Resources.Load<GameObject>("Prefabs/Arrow3D");
        arrow3DBinaerPrefab = Resources.Load<GameObject>("Prefabs/Arrow3DBinaer");
        
        targets = new List<GazeGuidingTarget>(FindObjectsOfType<GazeGuidingTarget>());
    }
    
    /// <param name="arrow3DInstanceCreated"> is a flag tracking whether an Arrow3D instance has been created </param>
    private bool arrow3DInstanceCreated = false; 
    /// <param name="arrow3DBinaerInstanceCreated"> is a flag tracking whether an Arrow3DBinary instance has been created </param>    
    private bool arrow3DBinaerInstanceCreated = false; 
    /// <summaQry>
    /// This method updates all active gaze-guiding tools.
    /// </summary>
    void Update()
    {   
        if (currentTarget != null)
        {
            if (Time.frameCount % 2 == 0 && DirectionArrowEnabled)
            {
                float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
                if (distance < pathDisplayDistance)
                {
                    lineRenderer.positionCount = 0;
                    isAnimating = false;
                    if (animatePathCoroutine != null)
                    {
                        StopCoroutine(animatePathCoroutine);
                        animatePathCoroutine = null;
                    }
                    
                    // Instantiate Arrow3D prefab if not already instantiated
                    if (!arrow3DInstanceCreated)
                    {
                        Arrow3D();
                        arrow3DInstanceCreated = true;
                    }

                    if (!arrow3DBinaerInstanceCreated)
                    {
                        Arrow3DBinaer();
                        arrow3DBinaerInstanceCreated = true;
                    }
                }
                else
                {
                    
                    // Remove the Arrow3D instance if the distance is too high
                    RemoveArrow3D();
                    if (!isAnimating)
                    {
                        animatePathCoroutine = StartCoroutine(AnimatePath(currentTarget.transform.position));
                    }
                }
            }
            else if(!DirectionArrowEnabled)
            {
                if (animatePathCoroutine != null)
                {
                    StopCoroutine(animatePathCoroutine);
                    animatePathCoroutine = null;
                }
                isAnimating = false;
                if (animatePathCoroutine != null)
                {
                    StopCoroutine(animatePathCoroutine);
                    animatePathCoroutine = null;
                }
                lineRenderer.positionCount = 0;
            }
        }
    }

    /// <summary>
    /// This method adds a continuous rotating 3D arrow that is rendered above an exact rotary switch to indicate the direction in which the player is supposed to turn the switch.
    /// </summary>
    public void Arrow3D()
    {
        if (Arrow3DEnabled && currentTarget.isTypeOf == GazeGuidingTarget.TargetType.Genau)
        {
            if (arrow3DInstance == null)
            {
                arrow3DInstance = Instantiate(arrow3DPrefab, 
                    currentTarget.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
            }
            
        }
    }

    /// <summary>
    /// This method adds a binary rotating 3D arrow rendered above a binary rotary switch to indicate the direction in which the player is supposed to turn the switch.
    /// </summary>    
    public void Arrow3DBinaer()
    {
        if (Arrow3DBinearEnabled && currentTarget.isTypeOf == GazeGuidingTarget.TargetType.Binaer)
        {
            if (arrow3DBinaerInstance == null)
            {
                arrow3DBinaerInstance = Instantiate(arrow3DBinaerPrefab, 
                    currentTarget.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
            }
            
        }
    }

    /// <summary>
    /// This method removes a rotating 3D arrow or 3DBinary arrow that is currently being rendered above an exact or a binary rotary switch.
    /// </summary>    
    public void RemoveArrow3D()
    {
        if (arrow3DInstance != null)
        {
            Destroy(arrow3DInstance);
            arrow3DInstance = null;
        }

        arrow3DInstanceCreated = false;

        if (arrow3DBinaerInstance != null)
        {
            Destroy(arrow3DBinaerInstance);
            arrow3DBinaerInstance = null;
        }
        
        arrow3DBinaerInstanceCreated = false;
    }
    
    // deprecated, not included in documentation

    public void TriggerTarget(GazeGuidingTarget target, bool Flip3DArrow = false)
    {
        ClearLine();
        currentTarget = target;
        if (Flip3DArrow)
        {
            arrow3DPrefab.GetComponent<Rotate3DArrow>().flipDirection = true;
        }
        else
        {
            arrow3DPrefab.GetComponent<Rotate3DArrow>().flipDirection = false;
        }
    }

    
    
    //Aus einem anderen Skript aufrufbar mit z.B.:
    //GazeGuidingPathPlayerSecondPath gazeGuidingPathPlayerSecondPath = FindObjectOfType<GazeGuidingPathPlayerSecondPath>();
    //gazeGuidingPathPlayerSecondPath.TriggerTargetNAME("WP1RPM", GazeGuidingTarget.TargetType.Anzeige);
    //dadruch wird dann das gazeGuiding auf WP1RPM-anzeige gesetzt
    //die restlichen eingaben sind die GameObjektnamen mit dem GazeGuidingTarget skript
    

    /// <summary>
    /// This method sets the current target of a GazeGuidingPathPlayerSecondPath instance.
    /// </summary>
    /// <param name="targetName">  contains the name of a rotary switch that is the new target for the GazeGuidingPathPlayerSecondPath </param>
    /// <param name="type"> is an Enum specifying the type of rotary switch passed in targetName </param>
    /// <param name="Flip3DArrow"> toggles whether 3D arrows rotate clockwise </param>
    public void TriggerTargetNAME(string targetName,  GazeGuidingTarget.TargetType type, bool Flip3DArrow = false)
    {
        ClearLine();
        currentTarget = targets.Find(t => t.name == targetName && t.isTypeOf == type);
        if (currentTarget != null)
        {
            if (type == GazeGuidingTarget.TargetType.Genau)
            {
                if (Flip3DArrow)
                {
                    arrow3DPrefab.GetComponent<Rotate3DArrow>().flipDirection = true;
                }
                else
                {
                    arrow3DPrefab.GetComponent<Rotate3DArrow>().flipDirection = false;
                }
            }else if (type == GazeGuidingTarget.TargetType.Binaer)
            {
                if (Flip3DArrow)
                {
                    arrow3DBinaerPrefab.GetComponent<Rotate3DArrowBinaer>().flipDirection = true;
                }
                else
                {
                    arrow3DBinaerPrefab.GetComponent<Rotate3DArrowBinaer>().flipDirection = false;
                }
            }
        }
        else
        {
            Debug.LogWarning($"Target with name {targetName} and type {type} not found.");
        }
    }
    
    /// <summary>
    /// This method clears the scene of all visual aids that are currently being rendered.
    /// </summary>    
    public void ClearLine()
    {
        // Remove the Arrow3D instance
        RemoveArrow3D();
        
        currentTarget = null;
        isAnimating = false;
        if (animatePathCoroutine != null)
        {
            StopCoroutine(animatePathCoroutine);
            animatePathCoroutine = null;
        }
        lineRenderer.positionCount = 0;
    }

    /// <summary>
    /// This method animates a path of arrows guiding the player towards the component set as currentTarget of the GazeGuidingPathPlayerSecondPath.
    /// </summary>
    /// <param name="targetPosition"> is a Vector3 position of the endpoint of the path </param>
    private IEnumerator AnimatePath(Vector3 targetPosition)
    {
        isAnimating = true;
        while (true)
        {
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / animationDuration);
                Vector3 currentPoint = Vector3.Lerp(transform.position, targetPosition, t);

                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, currentPoint);

                yield return null;
            }

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, targetPosition);

            float waitTime = 0.5f;
            while (waitTime > 0)
            {
                lineRenderer.SetPosition(0, transform.position);
                waitTime -= Time.deltaTime;
                yield return null;
            }
        }
    }
}
