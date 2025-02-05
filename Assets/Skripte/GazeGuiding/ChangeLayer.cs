using UnityEngine;

public class ChangeLayer : MonoBehaviour
{
    private string target;
    private bool propagateChildren = true;

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
