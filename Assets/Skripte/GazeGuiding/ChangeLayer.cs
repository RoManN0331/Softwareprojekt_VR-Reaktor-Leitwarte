using UnityEngine;

public class ChangeLayer : MonoBehaviour
{

    /// <summary>
    /// This class is used to change the layer of an object and its children.
    /// </summary>

    /// <param name="target"> string specifying the target layer an object is to be moved to</param>
    /// <param name="propagateChildren"> boolean specifying whether all children of an object are to be moved as well</param>

    private string target;
    private bool propagateChildren = true;

    /// <summary>
    /// This method moves an object to the layer specified in layer if it exists else returns -1
    /// </summary>

    /// <param name="layer"> string specifying the target layer an object is to be moved to</param>
    /// <param name="propagate"> boolean specifying whether all children of an object are to be moved as well</param>

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
    /// This utility method moves all children of a GameObject to a specific layer by calling itself recursively.
    /// </summary>
    /// <param name="obj"> GameObject to be moved to a specific layer </param>
    /// <param name="target"> int specifying the target layer the object is to be moved to</param>

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
