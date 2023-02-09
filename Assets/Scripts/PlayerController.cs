using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : Singleton<PlayerController>
{

    private GameManager gameManager;
    private Rigidbody playerRigidBody;
    private float velocityClampValue = 15;
    private float horizontal;
    private float vertital;
    private bool isGameRun;
    private bool isMove;
    private int hitParticleIndex;
    private Transform hitParticle;
    private Transform hitParticleBackUp;
    private List<Sprite> emojiList = new List<Sprite>();

    [SerializeField] private Joystick joystick;
    [SerializeField] private float speed = 5;
    [SerializeField] private float rotSpeed = 5;
    [SerializeField] private LineRenderer playerLineRenderer;
    [SerializeField] private BallController ballController;
    [SerializeField] private Transform playerTrailPoint;
    [SerializeField] private Transform ballTransform;
    [SerializeField] private SpriteRenderer emojiSpriteRenderer;

    public BallController BallController { get => ballController; set => ballController = value; }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.GameFail += GameFail;
        gameManager.GameStart += GameStarted;
        playerRigidBody = transform.GetComponent<Rigidbody>();
        emojiList = ObjectManager.Instance.EmojiList;
        hitParticle = Instantiate(ObjectManager.Instance.HitParticle);
        hitParticleBackUp = Instantiate(ObjectManager.Instance.HitParticle);

    }


    private void Update()
    {
        if (playerLineRenderer)
        {
            playerLineRenderer.SetPosition(0, playerTrailPoint.position);
            playerLineRenderer.SetPosition(1, ballTransform.position);
        }
        if (!gameManager.RunGame)
        {
            return;
        }

        if (transform.position.y > 0.4f && ballTransform.position.y < -0.05f)
        {
            ballTransform.position = new Vector3(ballTransform.position.x, 0, ballTransform.position.z);
        }

        if (isGameRun)
        {
            if (Input.GetMouseButton(0) && isMove)
            {
                PlayerMove();
            }
            if (transform.position.y < -1)
            {
                gameManager.GameFail();
            }

        }

    }


    private void PlayerMove()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            horizontal = joystick.Horizontal;
            vertital = joystick.Vertical;
        }


        Vector3 moveDirection = new Vector3(horizontal, 0, vertital);
        moveDirection.Normalize();
        playerRigidBody.AddForce(moveDirection * speed * Time.deltaTime);

        playerRigidBody.velocity = new Vector3(Mathf.Clamp(playerRigidBody.velocity.x, -velocityClampValue, velocityClampValue),
            playerRigidBody.velocity.y, Mathf.Clamp(playerRigidBody.velocity.z, -velocityClampValue, velocityClampValue));

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRot = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, rotSpeed * Time.deltaTime);
        }

    }


    void GameStarted()
    {
        isGameRun = true;
        isMove = true;
    }
    void GameFail()
    {
        isGameRun = false;
        isMove = true;
        StartCoroutine(WaitForFailEffect());
    }

    IEnumerator WaitForFailEffect()
    {
        yield return new WaitForSeconds(1);
        playerRigidBody.isKinematic = true;
        transform.DOMoveY(transform.position.y + 0.1f * 3, 1.5f).SetEase(Ease.InOutSine).SetLoops(6, LoopType.Yoyo).SetAutoKill(true);
    }


    public void ContactEnemy(Vector3 getForceDirection)
    {
        playerRigidBody.AddForce(getForceDirection * 0.2f);
        playerRigidBody.AddTorque(Vector3.forward * 10, ForceMode.Impulse);
    }

    public void HitParticleActive(Vector3 getParticlePos)
    {
        if (hitParticleIndex == 0)
        {
            hitParticle.position = getParticlePos;
            hitParticle.gameObject.SetActive(true);
        }
        else
        {
            hitParticleBackUp.position = getParticlePos;
            hitParticleBackUp.gameObject.SetActive(true);
        }
        if (hitParticleIndex > 1)
        {
            hitParticleIndex = 0;
        }
    }

    public void HappyEmoji(bool isHappy)
    {
        if (isHappy)
        {
            emojiSpriteRenderer.sprite = emojiList[Random.Range(4, 8)];
        }
        else
        {
            emojiSpriteRenderer.sprite = emojiList[Random.Range(0, 4)];
        }
        emojiSpriteRenderer.gameObject.SetActive(true);
        StartCoroutine(WaitForEmojiEffect());
    }
    IEnumerator WaitForEmojiEffect()
    {
        yield return new WaitForSeconds(1);
        emojiSpriteRenderer.transform.DOScale(Vector3.one * 0.4f, 0.15f).OnComplete(() =>
           {
               emojiSpriteRenderer.transform.DOScale(Vector3.one * 0.3f, 0.15f);
           });

        yield return new WaitForSeconds(1);
        emojiSpriteRenderer.gameObject.SetActive(false);
    }

}
