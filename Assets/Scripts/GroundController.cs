using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{

    private int layerIndex;

    private float groundTimer;
    private bool isTimerActive;

    [SerializeField] private Transform groundCircle;

    [SerializeField] private List<Transform> groundLayerList = new List<Transform>();


    private void Start()
    {
        GameManager.Instance.GameStart += GameStarted;
    }

    private void GameStarted()
    {
        isTimerActive = true;
    }

    private void Update()
    {
        if (isTimerActive)
        {
            groundTimer += Time.deltaTime;
            if (groundTimer > 5)
            {
                groundTimer = 0;
                isTimerActive = false;
                StartCoroutine(WaitForGroundDown());
            }
        }
    }

    IEnumerator WaitForGroundDown()
    {
        //groundCircle.gameObject.SetActive(true);
        groundCircle.DOMoveY(-8, 3).SetEase(Ease.Linear);
        yield return new WaitForSeconds(7f);
        groundCircle.DOMoveY(-10, 6).SetEase(Ease.Linear);

        for (int i = 0; i < groundLayerList[layerIndex].childCount; i++)
        {
            Transform groundPiece = groundLayerList[layerIndex].GetChild(i);
            groundPiece.GetComponent<Rigidbody>().isKinematic = false;
            groundPiece.GetComponent<MeshCollider>().enabled = false;
            Destroy(groundPiece.gameObject, 5);
            yield return new WaitForSeconds(0.2f);
        }
        groundCircle.localScale = new Vector3(groundCircle.localScale.x - 0.2f, groundCircle.localScale.x - 0.2f, groundCircle.localScale.z);

        if (layerIndex < 4)
        {
            isTimerActive = true;
            layerIndex++;
        }
    }



}
