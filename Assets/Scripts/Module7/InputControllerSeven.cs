using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

public class InputControllerSeven : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public GameObject player;
    public static InputControllerSeven instance;
    public float moveSens;
    public float forwardSpeed;

    [HideInInspector] public bool gameStarted = false;
    
    
    public Animator anim;


    private void Awake()
    {
        instance = this;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!gameStarted) return;

        float horizontal = eventData.delta.x;

        Vector3 moveDirection = Vector3.right * horizontal * moveSens;

        player.transform.position += moveDirection * Time.deltaTime;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameStarted = true;
    }

    private void Update()
    {
        if (gameStarted)
        {
            player.transform.parent.position += Vector3.forward * forwardSpeed * Time.deltaTime;
            BoundsSeven();
            anim.SetBool("isWalking", true);
        }
    }

    private void BoundsSeven()
    {
        Vector3 pos = player.transform.position;
        pos.x = Mathf.Clamp(pos.x, -3f, 3f);
        player.transform.position = pos;
    }
}
