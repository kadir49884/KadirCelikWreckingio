using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

enum DropBoxType
{
    Normal,
    Gold
}

public class DropBoxController : MonoBehaviour
{
    private float groundPosY = 5;
    private Rigidbody dropRigidBody;
    private VibrationController vibrationController;

    [SerializeField] private Transform parachut;
    [SerializeField] private Transform dropParticle;
    [SerializeField] private Transform collectParticle;
    [SerializeField] private DropBoxType dropBoxType;

    private void Start()
    {
        vibrationController = VibrationController.Instance;

        dropRigidBody = transform.GetComponent<Rigidbody>();
        transform.DOMoveY(groundPosY, 3).SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed).OnComplete(() =>
        {
            parachut.gameObject.SetActive(false);
            dropRigidBody.isKinematic = false;
        });
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.TryGetComponent(out PlayerController playerController))
        {
            playerController.BallController.CollisionDropBox(dropBoxType == DropBoxType.Gold);
            vibrationController.Vibrate(Lofelt.NiceVibrations.HapticPatterns.PresetType.HeavyImpact);
            CloseObject();
        }
        if (collision.transform.TryGetComponent(out EnemyController enemyController))
        {
            enemyController.BallController.CollisionDropBox(dropBoxType == DropBoxType.Gold);
            vibrationController.Vibrate(Lofelt.NiceVibrations.HapticPatterns.PresetType.HeavyImpact);
            CloseObject();
        }

        if (!dropParticle.gameObject.activeInHierarchy)
        {
            dropParticle.gameObject.SetActive(true);
        }

    }

    private void CloseObject()
    {
        collectParticle.parent = null;
        collectParticle.gameObject.SetActive(true);
        gameObject.SetActive(false);
        Destroy(gameObject, 2);
    }
}
