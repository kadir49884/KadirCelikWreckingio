using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform target;
    private Vector3 direction;
    private Rigidbody enemyRigidBody;
    private bool isLookAt;
    private bool isMove;
    private bool isHasDamage;
    private bool isDied;
    private float followTimer;
    private float rotateTimer;
    private int lookAtRandomValue;
    private RaycastHit hit;
    private GameManager gameManager;
    private PlayerController playerController;




    [SerializeField] private float enemySpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private LineRenderer enemyLineRenderer;
    [SerializeField] private BallController ballController;
    [SerializeField] private Transform enemyTrailPoint;
    [SerializeField] private Transform ballTransform;
    [SerializeField] private MeshRenderer enemyMeshRenderer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SkinnedMeshRenderer characterSkinnedMesh;
    [SerializeField] private TextMeshPro carNumberText;



    public MeshRenderer EnemyMeshRenderer { get => enemyMeshRenderer; set => enemyMeshRenderer = value; }
    public BallController BallController { get => ballController; set => ballController = value; }
    public SkinnedMeshRenderer CharacterSkinnedMesh { get => characterSkinnedMesh; set => characterSkinnedMesh = value; }

    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.GameStart += GameStarted;
        enemyRigidBody = transform.GetComponent<Rigidbody>();
        playerController = PlayerController.Instance;
        target = playerController.transform;
        enemySpeed = Random.Range(2, 5);
        carNumberText.text = Random.Range(1, 99).ToString();
    }

    private void GameStarted()
    {
        isMove = true;

    }

    private void Update()
    {
        if (transform.eulerAngles.z > 30 && transform.eulerAngles.z < 350)
        {
            rotateTimer += Time.deltaTime;
            if (rotateTimer > 3)
            {
                Quaternion toRot = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, rotSpeed * 10);
                if (rotateTimer > 5)
                {
                    rotateTimer = 0;
                }
            }
        }

        if (enemyLineRenderer)
        {
            enemyLineRenderer.SetPosition(0, enemyTrailPoint.position);
            enemyLineRenderer.SetPosition(1, ballTransform.position);
        }

    }

    void FixedUpdate()
    {

        if (isMove && transform.position.y < 1f)
        {
            if (transform.position.y > 0.4f && ballTransform.position.y < -0.05f)
            {
                ballTransform.position = new Vector3(ballTransform.position.x, 0, ballTransform.position.z);
            }



            followTimer += Time.deltaTime;

            if (followTimer > Random.Range(1f, 3f) && !isLookAt)
            {
                StartCoroutine(WaitForLookAt());
            }

            if (!isHasDamage)
            {
                if (target && isLookAt)
                {
                    if (lookAtRandomValue == 0)
                    {
                        direction = target.position - transform.position;
                    }
                    else
                    {
                        direction = target.position + target.right + target.forward - transform.position;
                    }

                    direction.y = 0;
                    Quaternion toRot = Quaternion.LookRotation(direction.normalized, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, rotSpeed);
                    enemyRigidBody.velocity = transform.forward * enemySpeed;
                }
                else
                {
                    enemyRigidBody.velocity = transform.forward * enemySpeed;
                }
            }

        }


        if (!Physics.Raycast(transform.position + Vector3.up + Vector3.forward * 2, Vector3.down, out hit, 100, groundLayer))
        {
            Vector3 direction = (Vector3.zero - transform.position);
            direction.y = 0;
            Quaternion toRot = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, rotSpeed * Time.deltaTime);
        }
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 100, groundLayer) && !isDied)
        {
            isDied = true;
            isLookAt = false;
            Destroy(transform.parent.gameObject, 4);
            isMove = false;
            SpawnerManager.Instance.UpdateEnemyCount();
        }
    }


    IEnumerator WaitForLookAt()
    {
        lookAtRandomValue = Random.Range(0, 2);
        isLookAt = true;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        isLookAt = false;
        followTimer = 0;
        enemySpeed = Random.Range(2, 5);
    }


    public void ContactEnemy(Vector3 getForceDirection)
    {
        isHasDamage = true;
        StartCoroutine(WaitForDamage());
        enemyRigidBody.AddForce(getForceDirection);
        enemyRigidBody.AddTorque(Vector3.forward * 10, ForceMode.Impulse);

    }

    IEnumerator WaitForDamage()
    {
        yield return new WaitForSeconds(0.5f);
        isHasDamage = false;

    }

}
