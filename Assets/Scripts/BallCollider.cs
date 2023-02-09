using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollider : MonoBehaviour
{
    private Vector3 currPos;
    private Vector3 previousPos;
    private bool isPlayerBall;

    private EnemyController globalEnemyController;
    private PlayerController globalPlayerController;
    private CameraController cameraController;
    private VibrationController vibrationController;

    [SerializeField] private Transform baseTransform;
    [SerializeField] private Vector3 ballForce;
    [SerializeField] private float forceValue;


    public Vector3 BallForce { get => ballForce; set => ballForce = value; }

    private void Start()
    {
        ballForce = Vector3.zero;
        globalPlayerController = PlayerController.Instance;

        if (!isPlayerBall)
        {
            globalEnemyController = baseTransform.GetComponent<EnemyController>();
        }


        cameraController = CameraController.Instance;
        vibrationController = VibrationController.Instance;
        Transform ballTransform = Instantiate(ObjectManager.Instance.BallList[Random.Range(0, ObjectManager.Instance.BallList.Count)]);
        ballTransform.parent = transform;
        ballTransform.localPosition = Vector3.zero;
        isPlayerBall = transform.parent.GetComponent<BallController>().IsPlayerBall;
    }

    private void FixedUpdate()
    {
        currPos = transform.position;
        ballForce.x = (currPos - previousPos).magnitude / Time.deltaTime;
        previousPos = currPos;

        if (transform.position.y > 2)
        {
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out EnemyController enemyController) && enemyController != globalEnemyController)
        {
            Vector3 direction = collision.contacts[0].point - transform.position;
            direction.y = .05f;
            enemyController.ContactEnemy((direction.normalized) * forceValue * ballForce.x);
            if (isPlayerBall)
            {
                globalPlayerController.HappyEmoji(true);
                globalPlayerController.HitParticleActive(collision.contacts[0].point + Vector3.up * 0.5f);
                if (ballForce.x > 5)
                {
                    cameraController.ShakeCamera(3, 0.3f);
                    vibrationController.Vibrate(Lofelt.NiceVibrations.HapticPatterns.PresetType.HeavyImpact);
                }
            }
        }
        if (collision.transform.TryGetComponent(out PlayerController playerController) && !isPlayerBall)
        {
            Vector3 direction = collision.contacts[0].point - transform.position;
            direction.y = .05f;
            playerController.ContactEnemy((direction.normalized) * forceValue * ballForce.x);
            playerController.HappyEmoji(false);
            globalPlayerController.HitParticleActive(collision.contacts[0].point + Vector3.up * 0.5f);
            vibrationController.Vibrate(Lofelt.NiceVibrations.HapticPatterns.PresetType.HeavyImpact);

            if (ballForce.x > 5)
            {
                cameraController.ShakeCamera(3, 0.3f);
            }
        }
    }
}
