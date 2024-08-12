using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerControllerSeven : MonoBehaviour
{
    public static PlayerControllerSeven instance;

    private GameObject stackPoints,stackParent;

    public List<GameObject> stackList = new List<GameObject>();

    public float speed;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        stackPoints = transform.GetChild(2).gameObject;
        stackParent = transform.parent.transform.GetChild(1).gameObject;        
    }

    private void Update()
    {
        if (InputControllerSeven.instance.gameStarted)
        {
            transform.parent.transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        }
        else if(InputControllerSeven.instance.gameStarted == false)
        {
            transform.parent.transform.position += new Vector3(0, 0, 0);            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "collectable":
                StackObject(other.gameObject); break;
            case "obstacles":
                DestroyLastStack();
                if(stackList.Count == 0)
                    GameManagerSeven.instance.OpenLosePanel();
                break;
            case "Finish":
                if (stackList.Count > 0)
                {
                    GameManagerSeven.instance.OpenWinPanel();
                    InputControllerSeven.instance.gameStarted = false;
                    InputControllerSeven.instance.anim.SetBool("isWalking", false);
                    StartCoroutine(FinishSequence());
                }
                else
                    GameManagerSeven.instance.OpenLosePanel();
                break;
        }
    }

    public void DestroyLastStack()
    {
        if (stackList.Count > 0)
        {
            GameObject lastStackedObject = stackList[stackList.Count - 1];
            stackList.RemoveAt(stackList.Count - 1);
            Destroy(lastStackedObject);

            stackPoints.transform.localPosition -= Vector3.forward * .5f;
        }
    }

    public void StackObject(GameObject collectable)
    {
        if (!stackList.Contains(collectable))
        {
            collectable.transform.tag = "collected";
            collectable.transform.GetComponent<BoxCollider>().isTrigger = false;
            stackList.Add(collectable);
            collectable.transform.SetParent(stackParent.transform, true);
            collectable.transform.localPosition = stackPoints.transform.localPosition;
            stackPoints.transform.localPosition += Vector3.forward * .5f;
            
            ShapeState shapeState = collectable.GetComponent<ShapeState>();
            if (shapeState != null)
            {
                int scoreToAdd = shapeState.isSphere ? 10 : 5;
                GameManagerSeven.instance.AddScore(scoreToAdd);
            }

            
            if (stackList.Count == 1)
                stackList[0].GetComponent<CollectableMove>().SetLeadTransform(transform);
            else if (stackList.Count > 1)
                stackList[^1].GetComponent<CollectableMove>().SetLeadTransform(stackList[^2].transform);

            StartCoroutine(MakeObjectsBigger());
        }
    }

    public IEnumerator MakeObjectsBigger()
    {
        for (int i = stackList.Count - 1; i >= 0; i--)
        {
            Vector3 scale = new Vector3(0.3f, 0.3f, 0.3f);
            scale *= 1.5f;

            stackList[i].transform.DOScale(scale, 0.1f).OnComplete(() =>
                stackList[i].transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.1f));
            yield return new WaitForSeconds(0.1f);
        }

    }

    private IEnumerator FinishSequence()
    {
        yield return new WaitForSeconds(0.5f); 

        DisableColliders();


        for (int i = 0; i < stackList.Count; i++)
        {
            stackList[i].transform.DOLocalMove(new Vector3(0, i * 0.3f, 2), 0.5f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(0.25f); 
        }
    }


    private void DisableColliders()
    {
        foreach (GameObject obj in stackList)
        {
            obj.GetComponent<Collider>().enabled = false; 
        }
    }


}
