using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// This class implements the basic controls for displays with three distinct stages progressing in one direction: normal values (white), problematic values (orange), critical values (red).
/// </summary>
public class AnzeigeSteuerung : MonoBehaviour
{
    [Space(10)]
    /// <param name="start_Number">float specifying the lower bound of the display</param>
    public float start_Number = 0;
    /// <param name="end_Number">float specifying the upper bound of the display</param>
    public float end_Number = 2000;
    /// <param name="komponente">string specifying the component the display corresponds to</param>
    public string komponente = "WP1";
    


    [Space(30)]
    /// <param name="CHANGEpercentage">float specifying the current percentage of the display</param>
    public float CHANGEpercentage = 0f;


    [Space(10)]
    /// <param name="toManiuplateNORMAL">MeshFilter component of the normal Mesh</param>
    public MeshFilter toManiuplateNORMAL;
    /// <param name="meshNORMAL">Mesh component of the normal Mesh</param>
    private Mesh meshNORMAL;
    /// <param name="originalVertices">Array of Vector3s containing the original vertices of the normal Mesh</param>
    private Vector3[] originalVertices;
    /// <param name="lastPercentage">float specifying the previous percentage of the normal bar</param>
    private float lastPercentage;
    /// <param name="percentage">int specifying the current percentage of the normal bar</param>
    public int percentage = 50;
    /// <param name="minIndex1">int specifying the index of the first minimum vertex of the normal Mesh (lower vertex)</param>
    private int minIndex1;
    /// <param name="minIndex2">int specifying the index of the second minimum vertex of the normal Mesh (upper vertex)</param>
    private int minIndex2;
    /// <param name="maxIndex">int specifying the index of the maximum vertex of the normal Mesh</param>
    private int maxIndex;

    /// <param name="toManiuplateORANGE">MeshFilter component of the orange Mesh</param>
    public MeshFilter toManiuplateORANGE;
    /// <param name="meshORANGE">Mesh component of the orange Mesh</param>
    private Mesh meshORANGE;
    /// <param name="originalVertices2">Array of Vector3s containing the original vertices of the orange Mesh</param>
    private Vector3[] originalVertices2;
    /// <param name="lastPercentage2">float specifying the previous percentage of the orange bar</param>
    private float lastPercentage2;
    /// <param name="percentage2">int specifying the percentage of the orange bar</param>
    public int percentage2 = 50;
    /// <param name="minIndex12">int specifying the index of the first minimum vertex of the orange Mesh (lower vertex)</param>
    private int minIndex12;
    /// <param name="minIndex22">int specifying the index of the second minimum vertex of the orange organge Mesh (upper vertex)</param>
    private int minIndex22;
    /// <param name="maxIndex2">int specifying the index of the maximum vertex of the orange Mesh</param>
    private int maxIndex2;

    /// <param name="toManiuplateRED">MeshFilter component of the red Mesh</param>
    public MeshFilter toManiuplateRED;
    /// <param name="meshRED">Mesh component of the red Mesh</param>
    private Mesh meshRED;
    /// <param name="originalVertices3">Array of Vector3s containing the original vertices of the red Mesh</param>
    private Vector3[] originalVertices3;
    /// <param name="lastPercentage3">float specifying the previous percentage of the red bar</param>
    private float lastPercentage3;
    /// <param name="percentage3">int specifying the current percentage of the red bar</param>
    public int percentage3 = 50;
    /// <param name="minIndex13">int specifying the index of the first minimum vertex of the red Mesh (lower vertex)</param>
    private int minIndex13;
    /// <param name="minIndex23">int specifying the index of the second minimum vertex of the red Mesh (upper vertex)</param>
    private int minIndex23;
    /// <param name="maxIndex3">int specifying the index of the maximum vertex of the red Mesh</param>
    private int maxIndex3;
    
    /// <param name="isDIGITAL">boolean specifying whether the display is digital</param>
    public bool isDIGITAL = false;
    /// <param name="CHANGEpercentageanimate">float specifying the percentage to animate the bars of a display</param>
    private float CHANGEpercentageanimate;
    /// <param name="DigitalText"> TextMeshPro component</param>
    private TextMeshPro DigitalText;
    /// <param name="isBar1Reset"> boolean tracking whether the first bar has been reset </param>
    private bool isBar1Reset = false;
    /// <param name="isBar2Reset"> boolean tracking whether the second bar has been reset </param>
    private bool isBar2Reset = false;
    /// <param name="animatePercentage">float specifying the current value that is being animated in setBar()</param>
    private float animatePercentage;    
    
    /// <summary>
    /// This method initialises the display, i.e. initiates the initialisation of meshes and bars, and starts the animation.
    /// </summary>    
    void Start()     
    {
        InitializeText(komponente, start_Number, end_Number);
        
        InitializeMesh(toManiuplateNORMAL, ref meshNORMAL, ref originalVertices, ref minIndex1, ref minIndex2, ref maxIndex, ref lastPercentage, percentage);
        InitializeMesh(toManiuplateORANGE, ref meshORANGE, ref originalVertices2, ref minIndex12, ref minIndex22, ref maxIndex2, ref lastPercentage2, percentage2);
        InitializeMesh(toManiuplateRED, ref meshRED, ref originalVertices3, ref minIndex13, ref minIndex23, ref maxIndex3, ref lastPercentage3, percentage3);
        
        
        setBar(0);
        setBar(1);
        setBar(2);
        
        StartCoroutine(AnimatePercentage());
    }


    /// <summary>
    /// This method animates the bar of the display in 3 steps first animating it to 100%, then animating it to 0% and finally animating it to CHANGEpercentage.
    /// </summary>
    private IEnumerator AnimatePercentage() //initalisiere alle meshes
    {
        CHANGEpercentageanimate = CHANGEpercentage;
        float duration = 2.0f; // Duration for the animation
        float elapsedTime = 0f;
        
        // step 1: animate the bars from 0 to 100, spiking the display within the specified duration i.e. 2 seconds
        
        while (elapsedTime < duration)
        {
            CHANGEpercentage = Mathf.Lerp(0, 100, elapsedTime / duration);
            UpdateBars();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // step 2: animate the bars from 100 to 0, dropping the display within the specified duration i.e. 2 seconds
        
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            CHANGEpercentage = Mathf.Lerp(100, 0, elapsedTime / duration);
            UpdateBars();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // step 3: animate the bars to the desired CHANGEpercentage within the specified duration i.e. 2 seconds

        float targetPercentage = CHANGEpercentageanimate; // Replace with the desired value
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            CHANGEpercentage = Mathf.Lerp(0, targetPercentage, elapsedTime / duration);
            UpdateBars();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set to the final desired CHANGEpercentage
        CHANGEpercentage = targetPercentage;
        UpdateBars();
    }

    /// <summary>
    /// This method updates the display by checking if the percentage has changed and updating the bars accordingly.
    /// </summary>
    void Update()
    {
        if(isDIGITAL) DigitalText.text = Mathf.FloorToInt(CHANGEpercentage * (end_Number / 100f)).ToString();;
        
        if (CHANGEpercentage != lastPercentage || CHANGEpercentage != lastPercentage2 || CHANGEpercentage != lastPercentage3)
        {
            UpdateBars();
        }
    }

    /// <summary>
    /// This method labels the scale of the display.
    /// </summary>
    /// <param name="komponente">string containing the name of the corresponding component</param>
    /// <param name="startNumber">float specifying the lower bound for the display</param>
    /// <param name="endNumber">float specifying the upper bound of the display</param>
    private void InitializeText(string komponente, float startNumber, float endNumber)
    {
        Transform anzeigeBasic = transform.Find("AnzeigeBasic");
        if (anzeigeBasic == null) return;

        SetText(anzeigeBasic, "Text_komponente", komponente);
        SetText(anzeigeBasic, "Text_StartNummer", startNumber.ToString());
        SetText(anzeigeBasic, "Text_NummerEnde", endNumber.ToString());

        for (int i = 1; i <= 7; i++)
        {
            float fraction = (i / 8f) * endNumber;
            SetText(anzeigeBasic, $"Text_Nummer{i}-8", fraction.ToString());
        }

        if (isDIGITAL)
        {
            DigitalText = anzeigeBasic.transform.Find("TEXT_DIGITAL").gameObject.GetComponent<TextMeshPro>();
        }
    }


    /// <summary>
    /// This method sets a text to a child component of a component InitializeText() was called on.
    /// </summary>
    /// <param name="parent">Transform of the parent object on which SetText() was called</param>
    /// <param name="childName">string containing the name of the child object</param>
    /// <param name="text">string containing the text to be set</param>
    private void SetText(Transform parent, string childName, string text)
    {
        Transform child = parent.transform.Find(childName);
        if (child != null)
        {
            TextMeshPro textMeshPro = child.gameObject.GetComponent<TextMeshPro>();
            if (textMeshPro != null)
            {
                textMeshPro.text = text;
            }
        }
    }
    
    /// <summary>
    /// This method initializes the meshes of the bars of the display.
    /// </summary>
    /// <param name="meshFilter">MeshFilter component of the bar</param>
    /// <param name="mesh">Mesh component of the bar</param>
    /// <param name="originalVertices">Array of Vector3s containing the original vertices of the Mesh</param>
    /// <param name="minIndex1">int specifying the index of the first minimum vertex of the Mesh (lower vertex)</param>
    /// <param name="minIndex2">int specifying the index of the second minimum vertex of the Mesh (upper vertex)</param>
    /// <param name="maxIndex">int specifying the index of the maximum vertex of the Mesh</param>
    /// <param name="lastPercentage">float specifying the previous percentage of the bar</param>
    /// <param name="percentage">int specifying the current percentage of the bar</param>
    private void InitializeMesh(MeshFilter meshFilter, ref Mesh mesh, ref Vector3[] originalVertices, ref int minIndex1, ref int minIndex2, ref int maxIndex, ref float lastPercentage, int percentage)
    {
        if (meshFilter == null) return;

        mesh = meshFilter.mesh;
        originalVertices = mesh.vertices.Clone() as Vector3[];
        lastPercentage = percentage;

        minIndex1 = 0;
        minIndex2 = 1;
        maxIndex = 0;
        for (int i = 1; i < originalVertices.Length; i++)
        {
            if (originalVertices[i].x < originalVertices[minIndex1].x)
            {
                minIndex2 = minIndex1;
                minIndex1 = i;
            }
            else if (originalVertices[i].x < originalVertices[minIndex2].x)
            {
                minIndex2 = i;
            }

            if (originalVertices[i].x > originalVertices[maxIndex].x)
            {
                maxIndex = i;
            }
        }
    }

    /// <summary>
    /// This method updates the bars of the display.
    /// </summary>
    private void UpdateBars()
    {
        CHANGEpercentage = Mathf.Clamp(CHANGEpercentage,0 ,100);
        
        if (Mathf.Abs(animatePercentage - CHANGEpercentage) < 1)
        {
            return;
        }
        else if (animatePercentage < CHANGEpercentage)
        {
            animatePercentage++;
        }
        else if (animatePercentage > CHANGEpercentage)
        {
            animatePercentage--;
        }
        
        if (animatePercentage <= percentage)
        {
            setBar(0);
        }

        if (animatePercentage >= percentage && animatePercentage <= percentage3)
        {
            setBar(1);
            isBar1Reset = false;
        }
        else if (animatePercentage < percentage && !isBar1Reset)
        {
            ResetBar(1);
            isBar1Reset = true;
        }

        if (animatePercentage >= percentage2 && animatePercentage <= percentage3)
        {
            setBar(2);
            isBar2Reset = false;
        }
        else if (animatePercentage < percentage2 && !isBar2Reset)
        {
            ResetBar(2);
            isBar2Reset = true;
        }
    }

    /// <summary>
    /// This method resets the bar of the display by changing the mesh.
    /// </summary>
    /// <param name="ID">int specifying the ID of the bar to be reset</param>
    private void ResetBar(int ID)
    {
        Mesh mesh = null;
        switch (ID)
        {
            case 1:
                mesh = meshORANGE;
                break;
            case 2:
                mesh = meshRED;
                break;
        }


        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].x = 0;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        
    }

    /// <summary>
    /// This method sets the bar of the display by changing the mesh, vertices and indices.
    /// </summary>
    /// <param name="ID">int specifying the ID of the bar to be set</param>
    private void setBar(int ID)
    {
        
        Vector3[] vertices = null;
        float t = 0;
        int minIndex1 = 0, minIndex2 = 0, maxIndex = 0;
        Mesh mesh = null;
        int percentage = 0;

        switch (ID)
        {
            case 0:
                vertices = originalVertices.Clone() as Vector3[];
                mesh = meshNORMAL;
                minIndex1 = this.minIndex1;
                minIndex2 = this.minIndex2;
                maxIndex = this.maxIndex;
                percentage = this.percentage;
                break;
            case 1:
                vertices = originalVertices2.Clone() as Vector3[];
                mesh = meshORANGE;
                minIndex1 = minIndex12;
                minIndex2 = minIndex22;
                maxIndex = maxIndex2;
                percentage = percentage2;
                break;
            case 2:
                vertices = originalVertices3.Clone() as Vector3[];
                mesh = meshRED;
                minIndex1 = minIndex13;
                minIndex2 = minIndex23;
                maxIndex = maxIndex3;
                percentage = percentage3;
                break;
        }

        if (animatePercentage < percentage)
        {
            t = 1 - (animatePercentage / 100f);
        }
        else
        {
            t = 1 - (percentage / 100f);
        }

        vertices[minIndex1].x = Mathf.Lerp(vertices[minIndex1].x, vertices[maxIndex].x, t);
        vertices[minIndex2].x = Mathf.Lerp(vertices[minIndex2].x, vertices[maxIndex].x, t);

        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        switch (ID)
        {
            case 0:
                lastPercentage = animatePercentage;
                break;
            case 1:
                lastPercentage2 = animatePercentage;
                break;
            case 2:
                lastPercentage3 = animatePercentage;
                break;
        }
    }
}