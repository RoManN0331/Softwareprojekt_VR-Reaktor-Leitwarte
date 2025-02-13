using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// This class implements logic for gaze guiding in a virtual environment.
/// <summary>
public class GazeGuidingPathPlayer : MonoBehaviour
{
    /// <param name="DirectionCueEnabledGlobal"> toggles a red frame (global) indicating that the player should turn</param>
    public bool DirectionCueEnabledGlobal = true;
    /// <param name="DirectionCueEnabled"> toggles a red frame indicating the player should turn</param>
    public bool DirectionCueEnabled = true;
    /// <param name="DirectionCueFadeDuration"> sets the duration of the red frame's fade in and fade out</param>
    public float DirectionCueFadeDuration = 1f;
    /// <param name="DirectionArrowEnabled"> toggles a directional arrow indicating a direction the player should turn to</param>
    public bool DirectionArrowEnabled = true;    
    /// <param name="Arrow3DEnabled"> toggles a 3D arrow indicating the direction an exact rotary switch should be turned in</param>
    public bool Arrow3DEnabled = true;
    /// <param name="Arrow3DBinearEnabled"> toggles 3D arrow indicating the direction a binary rotary switch should be turned in</param>
    public bool Arrow3DBinearEnabled = true;
    /// <param name="DirectionArrowOnScreen"> toggles the on screen display of the directional arrow</param>
    public bool DirectionArrowOnScreen = true;
    /// <param name="AnzeigenMarkierungEnabled"> toggles annotations to displays indicating their relevance and target values</param>
    public bool AnzeigenMarkierungEnabled = true;
    /// <param name="ClipboardHighlightEnabled"> toggles the highlighting of tasks on clipboards</param>
    public bool ClipboardHighlightEnabled = true;
    /// <param name="DisplayHighlightEnabled"> toggles the highlighting of displays </param>
    public bool DisplayHighlightEnabled = true;
    
    /// <param name="targets"> is a list of GazeGuidingTarget objects</param>
    public List<GazeGuidingTarget> targets;
    /// <param name="currentTarget"> is the currently active GazeGuidingTarget for this GazeGuidingPathPlayer</param>
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


    //Fügt einen Roten-Rand am Bildschirm ein um den Nutzer zu signalisieren, dass er sich drehen soll    

    /// <param name="DirectionCue"> is an image that is being rendered in calcDirectionCue() if the angle between the player and a target object is > 30 degrees </param>
    private Image DirectionCue;
    /// <param name="DirectionCue2"> is an image that is being rendered in calcDirectionCue() if the angle between the player and a target object is > 160 degrees </param>
    private Image DirectionCue2;

    /// <param name="arrow3DPrefab"> is a reference to an Arrow3D prefab used to render continuous rotational 3DArrows</param>    
    private GameObject arrow3DPrefab; 
    /// <param name="arrow3DInstance"> is an instance of a Arrow3D prefab used to render continuous rotational 3DArrows<param>

    private GameObject arrow3DInstance; 
    /// <param name="arrow3DBinaerPrefab"> is a reference to an Arrow3D prefab used to render binary rotational 3DArrows</param>
    
    private GameObject arrow3DBinaerPrefab; 
    /// <param name="arrow3DBinaerInstance"> is an instance of an Arrow3D prefab used to render binary rotational 3DArrows<param>
    private GameObject arrow3DBinaerInstance; // Instance of the Arrow3D prefab

    /// <summary>
    /// This method initialises the GazeGuidingPathPlayer. 
    /// </summary>
    void Start()
    {
        // um beide Augen in Windows zu zeigen
        UnityEngine.XR.XRSettings.gameViewRenderMode = UnityEngine.XR.GameViewRenderMode.OcclusionMesh;
        
        
        // initialising the LineRenderer and render material used for rendering the path computed in animatePath()        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;

        lineRenderer.positionCount = 0;
        lineRenderer.material = lineMaterial; 

        // Set texture mode to Tile
        lineRenderer.textureMode = LineTextureMode.Tile;

        // Set texture scale
        lineRenderer.textureScale = new Vector2(2.36f, 0.57f);
        
        // initialising the UI prefab used 
        // Load and instantiate the UI prefab
        GameObject uiPrefab = Resources.Load<GameObject>("Prefabs/UI/UI");
        GameObject uiInstance = Instantiate(uiPrefab);
        
        // Initialize the UI elements
        initUI(uiInstance);
        
        // Load the Arrow3D prefab
        arrow3DPrefab = Resources.Load<GameObject>("Prefabs/Arrow3D");
        arrow3DBinaerPrefab = Resources.Load<GameObject>("Prefabs/Arrow3DBinaer");
        
        // Automatically set all GazeGuidingTarget objects
        targets = new List<GazeGuidingTarget>(FindObjectsOfType<GazeGuidingTarget>());
        
        
        text1 = GameObject.Find("POS1").transform.Find("Clipboard/TEXT").GetComponent<TextMeshPro>();
        text2 = GameObject.Find("POS2").transform.Find("Clipboard/TEXT").GetComponent<TextMeshPro>();
        text3 = GameObject.Find("POS3").transform.Find("Clipboard/TEXT").GetComponent<TextMeshPro>();
        
        animator = FindObjectOfType<AnimatorController>();
    }

    /// <summary>
    /// This method initialises the necessessary components for calcDirectionCue() and adds them to the UI. 
    /// </summary>
    /// <param name="uiInstance"> is a UI instance</param> 
    private void initUI(GameObject uiInstance)
    {
        // Assign the DirectionCue and DirectionCue2 images
        DirectionCue = uiInstance.transform.Find("Canvas/DirectionCue").GetComponent<Image>();
        DirectionCue2 = uiInstance.transform.Find("Canvas/DirectionCue2").GetComponent<Image>();

        // Set initial states
        DirectionCue.gameObject.SetActive(false);
        DirectionCue2.gameObject.SetActive(false);
            
        // Find the main camera in the XR Origin (XR Rig) > Camera Offset > Main Camera
        Camera mainCamera = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Main Camera").GetComponent<Camera>();

        // Set the render camera of the Canvas component
        Canvas canvas = uiInstance.transform.Find("Canvas").GetComponent<Canvas>();
        canvas.worldCamera = mainCamera;
        // Set the plane distance to bring the UI closer to the camera
        canvas.planeDistance = 0.05f;

        // Set the sorting order to ensure the UI is rendered on top of other objects but behind the HUD
        canvas.sortingOrder = 99;
        
        // Set the transform as a child of the current camera
        
        // Set the transform as a child of the current camera
        uiInstance.transform.SetParent(mainCamera.transform);
        
    }

    /// <param name="animator"> is refenrence to the AnimatorController object of the scene</param>
    private AnimatorController animator;
    /// <param name="arrow3DInstanceCreated"> is a flag tracking whether an Arrow3D instance has been created </param>
    private bool arrow3DInstanceCreated = false; 
    /// <param name="arrow3DBinaerInstanceCreated"> is a flag tracking whether an Arrow3DBinary instance has been created </param>
    private bool arrow3DBinaerInstanceCreated = false; 
    ///<param name="reset"> tracks whether old visual cues have been removed</param>
    private bool reset = true;

    /// <summary>
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
                    
                    isAnimating = false;
                    if (animatePathCoroutine != null)
                    {
                        StopCoroutine(animatePathCoroutine);
                        animatePathCoroutine = null;
                    }
                    lineRenderer.positionCount = 0;
                }
            }

            if (Time.frameCount % 4 == 0 && DirectionCueEnabled && DirectionCueEnabledGlobal)
            {
                calcDirectionCue();
            }
            else if (!(isDirectionCue2Fading && isDirectionCueFading) && !DirectionCueEnabled)
            {
                DirectionCue.gameObject.SetActive(false);
                DirectionCue2.gameObject.SetActive(false);
            }

            if (!DirectionCueEnabledGlobal)
            {
                DirectionCue.gameObject.SetActive(false);
                DirectionCue2.gameObject.SetActive(false);
            }
            
            if (DirectionArrowOnScreen)
            {
                renderDirectionArrow();
            }else
            {
                removeDirectionArrow();
            }

            if (AnzeigenMarkierungEnabled && !Anzeigeninitialized)
            {
                for(int i = 0; i < anzeigenTargets.Count; i++)
                {
                    Transform anzeigenMarkerTransform = anzeigenTargets[i].transform.Find("AnzeigenMarker");
                    if (anzeigenMarkerTransform != null)
                    {
                        anzeigenMarkerTransform.gameObject.SetActive(true);
                        anzeigenMarkerTransform.GetComponent<AnzeigenMarker>().targetNumber = anzeigenNumbers[i];
                    }
                }
                Anzeigeninitialized = true;
            }
            else if(!AnzeigenMarkierungEnabled && Anzeigeninitialized)
            {
                foreach (var target in anzeigenTargets)
                {
                    Transform anzeigenMarkerTransform = target.transform.Find("AnzeigenMarker");
                    if (anzeigenMarkerTransform != null)
                    {
                        anzeigenMarkerTransform.gameObject.SetActive(false);
                    }
                }
                Anzeigeninitialized = false;
            }
           
        }

        //Stellt wirklich sicher das alles gelöscht wurde, da manche states es nicht richtig hinbekommen :/
        if (animator.getScenario() == 0 && !reset)
        {
            removeHighlightFromClipboard();
            ClearAnzeigenMarkierung();
            ClearLine();
            FindAnyObjectByType<GazeGuidingPathPlayerSecondPath>().ClearLine();
            UnsetGazeGuidingClipboard();
            reset = true;
        }
        else if(reset && (animator.getScenario() == 1 ||
                          animator.getScenario() == 2 ||
                          animator.getScenario() == 3))
        {
            reset = false;
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
    //GazeGuidingPathPlayer gazeGuidingPathPlayer = FindObjectOfType<GazeGuidingPathPlayer>();
    //gazeGuidingPathPlayer.TriggerTargetNAME("WP1RPM", GazeGuidingTarget.TargetType.Anzeige);
    //dadruch wird dann das gazeGuiding auf WP1RPM-anzeige gesetzt
    //die restlichen eingaben sind die GameObjektnamen mit dem GazeGuidingTarget skript
    
    /// <param name="lastCalledHighlight"> contains the name of a previously highlighted object</param>
    [FormerlySerializedAs("lastCalleHighlight")]public string lastCalledHighlight = "";


    /// <summary>
    /// This method sets the current target of a GazeGuidingPathPlayer instance.
    /// </summary>
    /// <param name="targetName">  contains the name of a rotary switch that is the new target for the GazeGuidingPathPlayer </param>
    /// <param name="type"> is an Enum specifying the type of rotary switch passed in targetName </param>
    /// <param name="Flip3DArrow"> toggles whether 3D arrows rotate clockwise </param>
    public void TriggerTargetNAME(string targetName,  GazeGuidingTarget.TargetType type, bool Flip3DArrow = false)
    {
        ClearLine();

        // locating the gaze guiding target for the component specified in targetName
        currentTarget = targets.Find(t => t.name == targetName && t.isTypeOf == type);

        if (targetName == "WP1RPM" || targetName == "WP2RPM" || targetName == "CPRPM" || targetName == "ModPos")
        {
            setDisplayHighlight(targetName);
            
            // Zwischenspeichern des letzten Calls um auf Abruf wieder die Highlights zu aktivieren, wenn sie deaktiviert wurden
            lastCalledHighlight = targetName;
        }


        if (currentTarget != null)
        {

            // adding a 3D arrow indicating the direction the player is supposed to turn a rotary switch in

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
        lastCalledHighlight = "";
        
        // Remove the Arrow3D instance
        RemoveArrow3D();                            // removes 3D rotating arrow

        removeDirectionArrow();                     // removes 3D directional arrow
        
        ClearAnzeigenMarkierung();                  // removes annotations to displays

        unsetDisplayHighlight();                    // removes glow effects from displays
        
        DirectionCue.gameObject.SetActive(false);   // removes red frame
        DirectionCue2.gameObject.SetActive(false);  // removes red frame
        
        currentTarget = null;
        isAnimating = false;
        if (animatePathCoroutine != null)           // removes gaze guiding path
        {
            StopCoroutine(animatePathCoroutine);
            animatePathCoroutine = null;
        }
        lineRenderer.positionCount = 0;
        
        
    }


// deprecated this method was used for debugging, therefore it is not included in the documentation

    public void triggerTEST()
    {
        ClearLine();
        currentTarget = targets[0];
    }

// deprecated this method was used for debugging, therefore it is not included in the documentation

    public void triggerTEST2(bool Flip3DArrow)
    {
        ClearLine();
        currentTarget = targets[0];
        if (Flip3DArrow)
        {
            arrow3DPrefab.GetComponent<Rotate3DArrow>().flipDirection = true;
        }
        else
        {
            arrow3DPrefab.GetComponent<Rotate3DArrow>().flipDirection = false;
        }
    }


    /// <summary>
    /// This method animates a path of arrows guiding the player towards the component set as currentTarget of the GazeGuidingPathPlayer.
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

    /// <param name="isDirectionCueFading"> tracks whether the first cue is fading out</param>
    private bool isDirectionCueFading = false;
    /// <param name="isDirectionCueFading2"> tracks whether the second cue is fading out</param>
    private bool isDirectionCue2Fading = false;

    /// <summary>
    /// This method displays a red cue indicating to the player where to turn if the player is facing away from the currentTarget of the GazeGuidingPathPlayer in an angle > 30 degrees. 
    /// If the angle is > 160 degrees a second cue is being rendered indicating the object is behind the player. 
    /// The Method calculates the alpha of the cues based on the angle between the player and the currentTarget of the GazeGuidingPathPlayer. If the current angle is smaller than the old angle the cue begins to fade out.
    /// </summary>
    public void calcDirectionCue()
    {
        Vector3 directionToTarget = currentTarget.transform.position - transform.position;
        float angle = Mathf.DeltaAngle(transform.eulerAngles.y, Quaternion.LookRotation(directionToTarget).eulerAngles.y);

        bool directionCueActive = Mathf.Abs(angle) > 30;
        DirectionCue.gameObject.SetActive(directionCueActive);

        if (directionCueActive)
        {
            RectTransform directionCueRect = DirectionCue.rectTransform;
            RectTransform directionCue2Rect = DirectionCue2.rectTransform;

            if (angle > 0)
            {
                directionCueRect.anchorMin = new Vector2(1, 0.5f); // Right middle
                directionCueRect.anchorMax = new Vector2(1, 0.5f); // Right middle
                directionCueRect.anchoredPosition = new Vector2(-311.3f, directionCueRect.anchoredPosition.y);
                directionCueRect.localRotation = Quaternion.Euler(0, 0, 0); // Rotate the DirectionCue
            }
            else
            {
                directionCueRect.anchorMin = new Vector2(0, 0.5f); // Left middle
                directionCueRect.anchorMax = new Vector2(0, 0.5f); // Left middle
                directionCueRect.anchoredPosition = new Vector2(311.3f, directionCueRect.anchoredPosition.y);
                directionCueRect.localRotation = Quaternion.Euler(0, -180, 0); // Rotate the DirectionCue
            }

            // Calculate the alpha value based on the angle
            float alpha = Mathf.InverseLerp(30, 180, Mathf.Abs(angle));
            if (!isDirectionCueFading && Mathf.Abs(DirectionCue.canvasRenderer.GetAlpha() - alpha) > 0.075f)
            {
                isDirectionCueFading = true;
                DirectionCue.CrossFadeAlpha(alpha, DirectionCueFadeDuration, false);
                StartCoroutine(ResetFadingFlag(DirectionCueFadeDuration, () => isDirectionCueFading = false));
            }

            bool directionCue2Active = Mathf.Abs(angle) > 160 || isDirectionCue2Fading;
            if (!DirectionCue2.gameObject.activeSelf)
            {
                DirectionCue2.gameObject.SetActive(directionCue2Active);
            }
            
            if (directionCue2Active)
            {
                if (angle > 160)
                {
                    directionCue2Rect.anchorMin = new Vector2(0, 0.5f); // Opposite anchor
                    directionCue2Rect.anchorMax = new Vector2(0, 0.5f); // Opposite anchor
                    directionCue2Rect.anchoredPosition = new Vector2(311.3f, directionCue2Rect.anchoredPosition.y); // Opposite position
                    directionCue2Rect.localRotation = Quaternion.Euler(0, -180, 0); // Rotate the DirectionCue2
                }
                else if (angle < -160)
                {
                    directionCue2Rect.anchorMin = new Vector2(1, 0.5f); // Opposite anchor
                    directionCue2Rect.anchorMax = new Vector2(1, 0.5f); // Opposite anchor
                    directionCue2Rect.anchoredPosition = new Vector2(-311.3f, directionCue2Rect.anchoredPosition.y); // Opposite position
                    directionCue2Rect.localRotation = Quaternion.Euler(0, 0, 0); // Rotate the DirectionCue2
                }
                if (!isDirectionCue2Fading && Mathf.Abs(DirectionCue2.canvasRenderer.GetAlpha() - alpha) > 0.075f)
                {
                    isDirectionCue2Fading = true;
                    DirectionCue2.CrossFadeAlpha(alpha, DirectionCueFadeDuration, false);
                    StartCoroutine(ResetFadingFlag(DirectionCueFadeDuration, () => isDirectionCue2Fading = false));
                }
            }
            else
            {
                if (!isDirectionCue2Fading && DirectionCue2.canvasRenderer.GetAlpha() > 0 && !directionCue2Active)
                {
                    isDirectionCue2Fading = true;
                    DirectionCue2.CrossFadeAlpha(0, DirectionCueFadeDuration, false);
                    StartCoroutine(ResetFadingFlag(DirectionCueFadeDuration, () => isDirectionCue2Fading = false));
                }
            }
        }
        else
        {
            if (!isDirectionCueFading && DirectionCue.canvasRenderer.GetAlpha() > 0)
            {
                isDirectionCueFading = true;
                DirectionCue.CrossFadeAlpha(0, DirectionCueFadeDuration, false);
                StartCoroutine(ResetFadingFlag(DirectionCueFadeDuration, () => isDirectionCueFading = false));
            }
            if (!isDirectionCue2Fading && DirectionCue2.canvasRenderer.GetAlpha() > 0)
            {
                isDirectionCue2Fading = true;
                DirectionCue2.CrossFadeAlpha(0, DirectionCueFadeDuration, false);
                StartCoroutine(ResetFadingFlag(DirectionCueFadeDuration, () => isDirectionCue2Fading = false));
            }
        }
    }


    /// <summary>
    /// This method resets the isDirectionCueFading or isDirectionCue2Fading flag to false via a callback to calcDirectionCue().
    /// </summary>
    /// <param name="duration"> specifies a delay before resetAction() is called</param>
    /// <param name="resetAction"> is a System.Action used to set isDirectionCueFading or isDirectionCue2Fading to false</param>
    private IEnumerator ResetFadingFlag(float duration, System.Action resetAction)
    {
        yield return new WaitForSeconds(duration);
        resetAction();
    }


    /// <param name="arrowInstance"> is an instance of a directional arrow </param>
    private GameObject arrowInstance; // Store the instance of the arrow

    /// <summary>
    /// This method renders a directional arrow indicating to the player the direction the currentTarget of the GazeGuidingPathPlayer is in.
    /// </summary>
    public void renderDirectionArrow()
    {
        // Check if an instance already exists
        if (arrowInstance != null)
        {
            return; // Exit the method if an instance already exists
        }

        // Load the 3DPfeilNfS prefab from the Resources folder
        GameObject arrowPrefab = Resources.Load<GameObject>("Prefabs/3DPfeilNfS");

        if (arrowPrefab != null)
        {
            // Instantiate the prefab
            arrowInstance = Instantiate(arrowPrefab);
            arrowInstance.GetComponent<PfeilNfS>().target = currentTarget;
            
            // Find the main camera
            Camera mainCamera = Camera.main;

            if (mainCamera != null)
            {
                // Calculate the position at the top of the screen
                Vector3 screenPosition = new Vector3(Screen.width / 2, Screen.height, mainCamera.nearClipPlane + 0.1f);
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

                // Set the position of the instantiated prefab
                arrowInstance.transform.position = worldPosition;

                // Optionally, set the parent to the camera to keep it in the same position relative to the screen
                arrowInstance.transform.SetParent(mainCamera.transform, true);
            }
            else
            {
                Debug.LogWarning("Main camera not found.");
            }
        }
        else
        {
            Debug.LogWarning("3DPfeilNfS prefab not found in Resources/Prefabs.");
        }
    }
    
    /// <summary>
    /// This method is used to remove the directional arrow set in renderDirectionArrow() from the UI.
    /// </summary>
    public void removeDirectionArrow()
    {
        // Check if an instance exists
        if (arrowInstance != null)
        {
            // Destroy the instance
            Destroy(arrowInstance);
            arrowInstance = null;
        }
    }
    

    /// <param name="anzeigenTargets"> is a list of GazeGuidingTarget objects used for gaze guiding</param>
    private List<GazeGuidingTarget> anzeigenTargets = new List<GazeGuidingTarget>();
    /// <param name="anzeigenNumbers"> is a list of float values used to calculate the position of an arrow indicating a target value on a display</param>
    private List<float> anzeigenNumbers = new List<float>();
    /// <param name="Anzeigeninitialized"> tracks whether displays have been initialised</param>    
    bool Anzeigeninitialized = false;

    /// <summary>
    /// This method sets the currentTarget for the GazeGuidingPathPlayer. Additionally the method indicates the display corresponding to the target and adds an arrow indicating the target value the player must set to the display.
    /// </summary>
    /// <param name="targetName"> contains the name of a rotary switch </param>
    /// <param name="type"> is an Enum specifying the type of rotary switch passed as targetName </param>
    /// <param name="NumberToHighlight"> used to calculate the position of an arrow indicating a target value on a display</param>
    public void TriggerAnzeigenMarkierung(string targetName, GazeGuidingTarget.TargetType type, float NumberToHighlight)
    {
        // locating the gaze guiding target for the component specified in targetName

        GazeGuidingTarget target = targets.Find(t => t.name == targetName && t.isTypeOf == type);
        
        if (target != null) anzeigenTargets.Add(target);
        anzeigenNumbers.Add(NumberToHighlight);
        

        // setting annotation to a display indicating relevant displays and target values 

        if (AnzeigenMarkierungEnabled)
        {
            if (target != null)
            {
                Transform anzeigenMarkerTransform = target.transform.Find("AnzeigenMarker");
                if (anzeigenMarkerTransform != null && !anzeigenMarkerTransform.gameObject.activeSelf)
                {
                    anzeigenMarkerTransform.gameObject.SetActive(true);
                    anzeigenMarkerTransform.GetComponent<AnzeigenMarker>().targetNumber = NumberToHighlight;
                    Anzeigeninitialized = true;
                }
                else
                {
                    Debug.LogError("Child GameObject 'AnzeigenMarker' not found.");
                }
            }
            else
            {
                Debug.LogWarning($"Target with name {targetName} and type {type} not found.");
            }
        }
    }
    
    /// <summary>
    /// This method removes annotations from a display. It removes a red ! highlighting the display as well as an arrow indicating the target value the player was supposed to set for the component associated with that display.
    /// </summary>
    public void ClearAnzeigenMarkierung()
    {
        foreach (var target in anzeigenTargets)
        {
            Transform anzeigenMarkerTransform = target.transform.Find("AnzeigenMarker");
            if (anzeigenMarkerTransform != null)
            {
                anzeigenMarkerTransform.gameObject.SetActive(false);
            }
        }
        
        anzeigenNumbers.Clear();
        anzeigenTargets.Clear();
        Anzeigeninitialized = false;
    }

    /*******************************
    ** gazeguiding for clipboards **
    *******************************/

    /// <param name="clipboardText"> is a TextMeshPro</param>
    private TextMeshPro clipboardText;
    /// <param name="text1"> is a TextMeshPro object containing the clipboardText for clipboard POS1 </param>
    private TextMeshPro text1;
    /// <param name="text2"> is a TextMeshPro object containing the clipboardText for clipboard POS2 </param>
    private TextMeshPro text2;
    /// <param name="text3"> is a TextMeshPro object containing the clipboardText for clipboard POS3 </param>
    private TextMeshPro text3;
    /// <param name="initialText"> contains the original unformatted text of the active clipboard</param>
    private string initalText;
    /// <param name="init"> checks whether the active clipboard has been initialised </param>
    private bool init = false;
    /// <param name="lastindex"> tracks the index of the previous highlighted task </param>
    private int lastindex = 0;
    /// <param name="GGClipboard"> is a reference to a GazeGuidingClipboard</param>
    public GazeGuidingClipboard GGClipboard;
    /// <param name="ClipBoardTextColor"> contains colour code for normal clipboard text </param>
    public string ClipBoardTextColor = "<color=#00FF00>";
    /// <param name="lastClipboardName"> contains the name of the previous highlighted clipboard </param>
    public string lastClipboardName = "";

    /// <sumary>
    /// This method activates highlighting for the text of a specific clipboard by creating a new GazeGuidingClipboard object for its text.
    /// </summary>
    /// <param name="clipboardName"> contains the name of a clipboard</param>
    public void SetGazeGuidingClipboard (string clipboardName){

        /* activates gaze guiding for clipboard clipboardName */

    
        lastClipboardName = clipboardName;
        //Clipboards wurden nicht gefunden, wenn sie gehalten wurden?? -> vorher alle clipboards initialisieren
        if (clipboardName.Equals("POS1")) clipboardText = text1;
        if (clipboardName.Equals("POS2")) clipboardText = text2;
        if (clipboardName.Equals("POS3")) clipboardText = text3;
        
        
        if (!init)
        {
            initalText = clipboardText.text;
            init = true;
        }
        GGClipboard = new GazeGuidingClipboard(clipboardText.text, ClipBoardTextColor);
    }


    // deprecated?
    public void UnsetGazeGuidingClipboard(){

        /* deactivates gaze guiding for a clipboard */

        GGClipboard = null;
        clipboardText = null;
        lastClipboardName = "";
    }

    /// <summary>
    /// This method highlights a task specified by index.                           
    /// </summary>
    /// <param name ="index"> specifies a task to highlight </param>
    public void HighlightClipboard(int index){
        /* highlights a portion of the text on the clipboard specified by index */
        
        lastindex = index;
        
        if (GGClipboard == null || clipboardText == null){

            Debug.LogError("No clipboard set for gaze guiding");
            
        } else {

            GGClipboard.HighlightTask(index);
            clipboardText.text = GGClipboard.GetFormattedClipboardText();
            
            //Give Text to the HUD

            giveTexttoHUD();
        }
    }

/// <summary>
/// Sets the highlighted text on the HUD.
/// </summary>

    public void giveTexttoHUD(){
        HUD hud = FindAnyObjectByType<HUD>();

        if (hud != null)
        {
            if(GGClipboard != null && clipboardText != null)
            {
                hud.setText(GGClipboard.GetFormattedClipboardText());
            }
        }
    }

    /// <summary>
    /// This method removes highlighting from a clipboard by reinitialising the respective clipboard without any highlighted text.
    /// </summary>
    public void removeHighlightFromClipboard(){

        /* removes highlighting by reinitialising the clipboard */

        if (clipboardText == null){

            Debug.LogError("No clipboard set for gaze guiding");
            
        } else {

            GGClipboard = new GazeGuidingClipboard(initalText, ClipBoardTextColor);
            clipboardText.text = initalText;
            init = false;
        }
        
        //Clear HUD
        HUD hud = FindAnyObjectByType<HUD>();

        if(hud != null) hud.clearText();

        lastindex = 0;

    }

    /// <summary>
    /// This method removes highlighting from a clipboard by reinitialising the respective clipboard without any highlighted text. This method is used if the player is no longer looking at the button associated with the clipboard.
    /// </summary>
    public void removeHighlightFromClipboardForButton(){

        /* removes highlighting by reinitialising the clipboard */

        if (!lastClipboardName.Equals(""))
        {
            SetGazeGuidingClipboard(lastClipboardName);
            if(lastindex != 0) HighlightClipboard(lastindex);
        }
    }
    

    /**********************************
    ** Highlighting displays utility **
    **********************************/

    /// <param name="WP1RPMisGlowing"> tracks whether WP1RPM_display is currently glowing</param>
    private bool WP1RPMisGlowing = false;
    /// <param name="WP2RPMisGlowing"> tracks whether WP2RPM_display is currently glowing</param>
    private bool WP2RPMisGlowing = false;
    /// <param name="CPRPMisGlowing"> tracks whether CPRPM_display is currently glowing</param>
    private bool CPRPMisGlowing = false;
    /// <param name="ControlRodsisGlowing"> tracks whether the controlRods display is currently glowing</param>
    private bool ControlRodsisGlowing = false;
    /// <param name="EngergyisGlowing"> tracks whether the Energy display is currently glowing</param>
    private bool EnergyisGlowing = false;
    /// <param name="RPressureisGlowing"> tracks whether the RPressure display is currently glowing</param>
    private bool RPressureisGlowing = false;
    /// <param name="CPressureisGlowing"> tracks whether the CPressure display is currently glowing</param>
    private bool CPressureisGlowing = false;
    /// <param name="RWaterLvlisGlowing"> tracks whether the RWaterLvl display is currently glowing</param>
    private bool RWaterLvlisGlowing = false;
    /// <param name="CWaterLvlisGlowing"> tracks whether the CWaterLvl display is currently glowing</param>
    private bool CWaterLvlisGlowing = false;

    /// <summary>
    /// This method enables a glow effect on all displays related to a the component that is the currentTarget of the GazeGuidingPathPlayer.
    /// </summary>
    /// <param name="targetName"> contains the name of a component that is the currentTarget of the GazeGuidingPathPlayer </param>
    public void setDisplayHighlight(string targetName)
    {
        if(!DisplayHighlightEnabled) return;
        Debug.Log("setDisplayHighlight");
        /* toggles highlight for displays when setting pumps or moderator position */
        if(targetName == "WP1RPM")
        {
            /* sets highlight for the rpm display for WP1, reactor and condenser water levels*/

            if (!WP1RPMisGlowing)
            {
                GameObject.Find("WP1RPM_display").GetComponent<Glow>().setGlowing();
                WP1RPMisGlowing = true;
            }
            
            if (!RWaterLvlisGlowing)
            {
                GameObject.Find("RWaterLvl").GetComponent<Glow>().setGlowing();
                RWaterLvlisGlowing = true;
            }
            
            if (!CWaterLvlisGlowing)
            {
                GameObject.Find("CWaterLvl").GetComponent<Glow>().setGlowing();
                CWaterLvlisGlowing = true;

            }
        }
        else if (targetName == "WP2RPM")
        {
            /* sets highlight for the rpm display for WP2, reactor and condenser water levels*/

            if (!WP2RPMisGlowing)
            {
                GameObject.Find("WP2RPM_display").GetComponent<Glow>().setGlowing();
                WP2RPMisGlowing = true;                    
            }
            
            if (!RWaterLvlisGlowing)
            {
                GameObject.Find("RWaterLvl").GetComponent<Glow>().setGlowing();
                RWaterLvlisGlowing = true;
            }

            
            if (!CWaterLvlisGlowing)
            {
                GameObject.Find("CWaterLvl").GetComponent<Glow>().setGlowing();
                CWaterLvlisGlowing = true;
            }
        }
        else if (targetName == "CPRPM")
        {
            Debug.Log("CP");

            /* sets hightlight for the rpm display for CP, condenser pressure and condenser water level */
            if (!CPRPMisGlowing)
            {
                GameObject.Find("CPRPM_display").GetComponent<Glow>().setGlowing();
                CPRPMisGlowing = true;
                Debug.Log("CPRPMisGlowing");
            }
            
            if (!CPressureisGlowing)
            {
                GameObject.Find("CPressure").GetComponent<Glow>().setGlowing();
                CPressureisGlowing = true;
                Debug.Log("CPressureisGlowing");
            }
            
            if (!CWaterLvlisGlowing)
            {
                GameObject.Find("CWaterLvl").GetComponent<Glow>().setGlowing();
                CWaterLvlisGlowing = true;
                Debug.Log("CWaterLvlisGlowing");
            }

        }
        else if (targetName == "ModPos")
        {
            /* sets highlight for moderator position, reactor water and reactor pressure level */

            if (!ControlRodsisGlowing)
            {
                GameObject.Find("controlRods").GetComponent<Glow>().setGlowing();
                ControlRodsisGlowing = true;
            }

            if (!RWaterLvlisGlowing)
            {
                GameObject.Find("RWaterLvl").GetComponent<Glow>().setGlowing();
                RWaterLvlisGlowing = true;
            }
            
            if (!RPressureisGlowing)
            {
                GameObject.Find("RPressure").GetComponent<Glow>().setGlowing();
                RPressureisGlowing = true;
            }
            
            if (!EnergyisGlowing)
            {
                GameObject.Find("Energy").GetComponent<Glow>().setGlowing();
                EnergyisGlowing = true;
            }
        }
    }

    /// <summary>
    /// This method removes a glow effect from all displays it is currently applied to.
    /// </summary>
    public void unsetDisplayHighlight()
    {
        if (WP1RPMisGlowing)
        {
            GameObject.Find("WP1RPM_display").GetComponent<Glow>().setGlowing();
            WP1RPMisGlowing = false;
        }
        if (WP2RPMisGlowing)
        { 
            GameObject.Find("WP2RPM_display").GetComponent<Glow>().setGlowing();
            WP2RPMisGlowing = false;
        }
        if (CPRPMisGlowing)
        {
            GameObject.Find("CPRPM_display").GetComponent<Glow>().setGlowing();
            CPRPMisGlowing = false;
        }
        if (ControlRodsisGlowing)
        {
            GameObject.Find("controlRods").GetComponent<Glow>().setGlowing();
            ControlRodsisGlowing = false;
        }
        if (EnergyisGlowing)
        {
            GameObject.Find("Energy").GetComponent<Glow>().setGlowing();
            EnergyisGlowing = false;
        }
        if (RPressureisGlowing)
        {
            GameObject.Find("RPressure").GetComponent<Glow>().setGlowing();
            RPressureisGlowing = false;
        }
        if (CPressureisGlowing)
        {
            GameObject.Find("CPressure").GetComponent<Glow>().setGlowing();
            CPressureisGlowing = false;
        }
        if (RWaterLvlisGlowing)
        {
            GameObject.Find("RWaterLvl").GetComponent<Glow>().setGlowing();
            RWaterLvlisGlowing = false;
        }
        if (CWaterLvlisGlowing)
        {
            GameObject.Find("CWaterLvl").GetComponent<Glow>().setGlowing();
            CWaterLvlisGlowing = false;
        }
    }


    /***********************************
    ** toggling visibility of objects **
    ***********************************/

    /// <param name="detached"> tracks whether the detach effect is currently applied (true) </param>
    public bool detached = false;
    /// <param name="checkCullingMask"> checks if main camera is rendering "detached" </param> 
    private bool checkCullingMask = true;

    /// <summary>
    /// This method enables the detach effect moving all rotary switches and displays on the main console from the "Default" layer (rendered by main camera) to an unrendered Layer "detached".
    /// </summary>
    /// <param name="on"> toggles the detach effect </param>
    public void SetDetach(bool on)
    {
        if (on)
            {
                if (checkCullingMask && (Camera.main.cullingMask & (1 << LayerMask.NameToLayer("detached"))) != 0)
                {
                    /* checks camera settings on initialization */

                    Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("detached"));
                    checkCullingMask = false;
                }

                GameObject.Find("Anzeigen").GetComponent<ChangeLayer>().setLayer("detached");
                GameObject.Find("Regler").GetComponent<ChangeLayer>().setLayer("detached");
                detached = true;
            }
        else
            {
                GameObject.Find("Anzeigen").GetComponent<ChangeLayer>().setLayer("Default");
                GameObject.Find("Regler").GetComponent<ChangeLayer>().setLayer("Default");
                detached = false;
            }
    }

    /// <summary>
    /// This method moves objects between layers to toggle their visibility, because main camera does not render the layer "detached".
    /// </summary>
    /// <param name="target"> contains the name of an object to attacj or detach to the "Default" layer </param>
    /// <param name="on"> toggles the attachment (true) or detachment (false) of target </param>
    public void ToggleObjectVisibility(string target, bool on)
    {
        /*  toggles an objects visibility by moving it between layers */

        if (on)
            GameObject.Find(target).GetComponent<ChangeLayer>().setLayer("Default");
        else
            GameObject.Find(target).GetComponent<ChangeLayer>().setLayer("detached");
    }


    /****************
    ** camera blur **
    ****************/

    /// <param name="blur"> tracks whether the blur effect is switched on (true) </param>
    public bool blur = false;
    /// <param name="focusCamera"> is a camera object tracking the camera rendering "Focused" layer </param>
    private Camera focusCamera;
    /// <param name="mainVolume"> is a Volume object assigned to main camera </param>
    private Volume mainVolume;
    /// <param name="focusVolume"> is a Volume object assigned to focus camera </param>
    private Volume focusVolume;

    /// <summary>
    /// This method toggles the blur effect according to the boolean value passed as an argument.
    /// </summary>
    /// <param name ="on"> toggles the blur effect </param>
    public void SetBlur(bool on)
    {
        if (on)
            BlurCamera();
        else
            UnblurCamera();
    }

    /// <summary>
    /// This method blurs or unblurs an object by moving it between layers. Objects on the "Focused" layer are not blurred while objects on the "Default" layer are blurred.
    /// </summary>
    /// <param name = "target"> contains the name of the object to blur or unblur </param>
    /// <param name = "on"> toggles the blurring (false) or unblurring (true) of target </param>
    public void ToggleBlur(string target, bool on)
    {
        if (on)
            GameObject.Find(target).GetComponent<ChangeLayer>().setLayer("Focused");
        else
            GameObject.Find(target).GetComponent<ChangeLayer>().setLayer("Default");
    }

    /// <summary>
    /// This method activates the blur effect by setting the focus distance, focal length and aperture of the volumes assigned to main camera, setting the culling mask of main camera and focus camera and setting the blur switch to true.
    /// </summary>
    private void BlurCamera()
    {

        /* sets up the blur effect */

        if(GameObject.Find("FocusCamera"))
            focusCamera = GameObject.Find("FocusCamera").GetComponent<Camera>();
        else
            Debug.LogError("Focus Camera not found");


        // setting the culling Mask of main camera to exclude "focused" and "detached"
        
        Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("Focused"));
        Camera.main.cullingMask = Camera.main.cullingMask & ~(1 << LayerMask.NameToLayer("Detached"));
    
        // setting occlusion culling for main camera

        if (!Camera.main.useOcclusionCulling)
            Camera.main.useOcclusionCulling = true;
    
        // setting the Depth of Field for main camera

        if (Camera.main.GetComponent<Volume>())
        {
            mainVolume = Camera.main.GetComponent<Volume>();
            
            if (!mainVolume.enabled)
                mainVolume.enabled = true;

            if (mainVolume.profile.TryGet(out DepthOfField mainDepth))
            {
                mainDepth.active = true;
                mainDepth.focusDistance.overrideState = true;
                mainDepth.focusDistance.value = 0.9f;
                mainDepth.focalLength.overrideState = true;
                mainDepth.focalLength.value = 80;
                mainDepth.aperture.overrideState = true;
                mainDepth.aperture.value = 32f;
            } else {
                Debug.LogWarning("Depth of Field not found in Main Camera Volume");
            }
        }

        // setting up the focus camera

        if (!focusCamera.enabled)
            focusCamera.enabled = true;

        // setting the culling mask of focus camera to only include "focused"

        focusCamera.cullingMask = 1 << LayerMask.NameToLayer("Focused");

        // setting occlusing culling for focus camera

        if (!focusCamera.useOcclusionCulling)
            focusCamera.useOcclusionCulling = true;
        
        // setting the Depth of Field for focus camera

        if(focusCamera.GetComponent<Volume>())
        {
            focusVolume = focusCamera.GetComponent<Volume>();

            if (!focusVolume.enabled)
                focusVolume.enabled = true;
            
            if (focusVolume.profile.TryGet(out DepthOfField focusDepth))
            {
                focusDepth.active = true;
                focusDepth.focusDistance.overrideState = true;
                focusDepth.focusDistance.value = 0.75f;
                focusDepth.focalLength.overrideState = true;
                focusDepth.focalLength.value = 30.7f;
                focusDepth.aperture.overrideState = true;
                focusDepth.aperture.value = 5.6f;
            } else {
                Debug.LogWarning("Depth of Field not found in Focus Camera Volume");
            }
        }

        blur = true;
    }


    /// <summary>
    /// This method deactivates the blur effect by deactivating the volume assigned to main camera as well as deactivating focus camera and setting the switch for the blur effect to false.
    /// </summary>
    private void UnblurCamera()
    {
        Debug.Log("Unblurring Camera");
        // disable the Volume on main camera
        if(Camera.main.GetComponent<Volume>().enabled)
            Camera.main.GetComponent<Volume>().enabled = false;

        // disable focusd camera
        if (focusCamera.enabled)
            focusCamera.GetComponent<Volume>().enabled = false;

        blur = false;
    }

}