using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectableMove : MonoBehaviour
{
    public Transform currentLeadTransform;
    public GameObject player;
    private float currentVelocity;
    private float smoothTime = .1f;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!currentLeadTransform) return;
        
        else
            transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x,currentLeadTransform.position.x,
                ref currentVelocity,smoothTime), transform.position.y,transform.position.z);
    }


    public void SetLeadTransform(Transform leadTransform)
    {
        currentLeadTransform = leadTransform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("collectable"))
        {
            player.GetComponent<PlayerControllerSeven>().StackObject(other.gameObject);
        }

        if (other.CompareTag("obstacles"))
        {
            PlayerControllerSeven.instance.DestroyLastStack();
        }
    }
}
