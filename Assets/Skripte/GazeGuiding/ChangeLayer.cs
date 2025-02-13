using UnityEngine;

/// <summary>
/// This class implements logic to move objects and their children between layers.
/// </summary>
public class ChangeLayer : MonoBehaviour
{
    /// <param name="target"> specifies the layer an object will be moved to</param>
    private string target;
     /// <param name="propagateChildren"> tracks whether all children of an object will be moved</param>
    private bool propagateChildren = true;

    /// <summary>
    /// This method moves an object to a specific layer.
    /// </summary>
    /// <param name="layer">  specifies the layer  an object will be moved to</param>
    /// <param name="propagate"> tracks whether all children of an object will be moved</param>
    public void setLayer(string layer, bool propagate = true)
    {

        /* moves an object to the layer specified in layer if it exists else returns -1 */

        if (!propagate)
            propagateChildren = false;

        target = layer;

        int newLayer = LayerMask.NameToLayer(target);

        if (newLayer == -1)
        {
            Debug.LogWarning("Layer " + target + " not found!");
            return;
        }

        gameObject.layer = newLayer;

        if(propagateChildren)
        {
            PropagateChildren(gameObject, newLayer);
        }
    }

    /// <summary>
    /// This method moves all children of an object to a specific layer by calling itself recursively.
    /// </summary>
    /// <param name="obj">  is a reference to an object that will be moved to a specific layer </param>
    /// <param name="target"> specifies the layer an object will be moved to</param>
    private void PropagateChildren(GameObject obj, int target)
    {
        
        /* utility method moving children of a GameObject obj to layer target*/

        foreach(Transform child in obj.transform)
        {
            child.gameObject.layer = target;
            PropagateChildren(child.gameObject, target);
        }
    }

}
