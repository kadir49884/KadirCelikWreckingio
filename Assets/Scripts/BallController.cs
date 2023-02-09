using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private float powerTimer;
    private float ballDistance;
    private bool rotatePower;
    private Vector3 ballPos;
    private Transform ballColliderTransform;

    [SerializeField] private bool isPlayerBall;
    [SerializeField] private Transform baseTransform;
    [SerializeField] private Quaternion targetRot;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float powerStrenght;
    [SerializeField] private float powerTimeValue;
    [SerializeField] private BallCollider ballCollider;
    [SerializeField] private GameObject lineRendererGameObject;
    [SerializeField] private Transform secondBall;

    public bool IsPlayerBall { get => isPlayerBall; set => isPlayerBall = value; }

    void Start()
    {
        ballPos = ballCollider.transform.localPosition;
        ballColliderTransform = ballCollider.transform;
    }
    void FixedUpdate()
    {

        ballDistance = Vector3.Distance(transform.position, ballColliderTransform.position);

        if (ballDistance > 6)
        {
            speed = 10;
            ballCollider.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else if (speed != 2)
        {
            speed = 2;
        }

        transform.position = baseTransform.transform.position;
        if (rotatePower)
        {
            transform.Rotate(Vector3.up * powerStrenght * Time.deltaTime);
            powerTimer += Time.deltaTime;
            if (powerTimer > powerTimeValue)
            {
                lineRendererGameObject.SetActive(true);
                rotatePower = false;
                powerTimer = 0;
                secondBall.gameObject.SetActive(false);

                //baseTransform.GetComponent<Rigidbody>().mass = 1;
            }
        }
        else
        {
            targetRot = baseTransform.transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
        ballColliderTransform.localPosition = Vector3.Lerp(ballColliderTransform.localPosition, ballPos, speed * Time.deltaTime);

    }



    public void CollisionDropBox(bool isGolden)
    {
        lineRendererGameObject.SetActive(false);
        rotatePower = true;
        if (isGolden)
        {
            secondBall.gameObject.SetActive(true);
        }
        //baseTransform.GetComponent<Rigidbody>().mass = 1.4f;
    }



}
