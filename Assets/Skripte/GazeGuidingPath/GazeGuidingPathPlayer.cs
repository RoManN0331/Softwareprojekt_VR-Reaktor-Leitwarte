using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GazeGuidingPathPlayer : MonoBehaviour
{
    public List<GazeGuidingTarget> targets;
    public float pathDisplayDistance = 5.0f;
    public float animationDuration = 1.0f; // Duration of the path drawing animation
    public Material lineMaterial; 
    private LineRenderer lineRenderer;
    private GazeGuidingTarget currentTarget;
    private Coroutine animatePathCoroutine;
    private bool isAnimating = false;

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

    }

    void Update()
    {
        if (currentTarget != null)
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
            }
            else
            {
                if (!isAnimating)
                {
                    animatePathCoroutine = StartCoroutine(AnimatePath(currentTarget.transform.position));
                }
            }
        }
    }

    public void TriggerTarget(GazeGuidingTarget target)
    {
        ClearLine();
        currentTarget = target;
    }

    public void ClearLine()
    {
        currentTarget = null;
        isAnimating = false;
        if (animatePathCoroutine != null)
        {
            StopCoroutine(animatePathCoroutine);
            animatePathCoroutine = null;
        }
        lineRenderer.positionCount = 0;
    }

    public void triggerTEST()
    {
        currentTarget = targets[0];
    }

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