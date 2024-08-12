using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeChangerDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("collected"))
        {
            ShapeState shapeState = other.GetComponent<ShapeState>();
            if (shapeState != null)
            {
                ChangeShape(other.gameObject, shapeState);
            }
        }
    }
    private void ChangeShape(GameObject obj,ShapeState shapeState)
    {
        GameObject newPrefab;

        if (shapeState.isSphere)
        {
            newPrefab = Resources.Load<GameObject>("SpherePrefab");
        }
        else
        {
            newPrefab = Resources.Load<GameObject>("CubePrefab");
        }

        if (newPrefab != null)
        {
            MeshFilter newMeshFilter = newPrefab.GetComponent<MeshFilter>();
            MeshRenderer newMeshRenderer = newPrefab.GetComponent<MeshRenderer>();

            MeshFilter objMeshFilter = obj.GetComponent<MeshFilter>();
            MeshRenderer objMeshRenderer = obj.GetComponent<MeshRenderer>();

            if (newMeshFilter != null && objMeshFilter != null)
            {
                objMeshFilter.mesh = newMeshFilter.sharedMesh;
            }

            if (newMeshRenderer != null && objMeshRenderer != null)
            {
                objMeshRenderer.material = newMeshRenderer.sharedMaterial;
            }

            shapeState.isSphere = !shapeState.isSphere;
        }
    }

}
