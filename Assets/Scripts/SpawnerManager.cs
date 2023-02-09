using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : Singleton<SpawnerManager>
{
    private ObjectManager objectManager;
    private Transform enemyPrefab;
    private Vector3 randomSpawnPos;
    private float spawnArea = 4;
    private List<Material> carMaterialList = new List<Material>();
    private List<Material> characterMaterialList = new List<Material>();
    private EnemyController enemyController;
    private Transform playerController;
    private int enemyCount;

    //private List<Transform> enemyList = new List<Transform>();
    private Transform dropPrefab;
    private Transform goldenDropBoxPrefab;
    [SerializeField] private int enemySpawnCount = 4;


    private float dropTimer;

    private GameManager gameManager;



    private void Start()
    {
        gameManager = GameManager.Instance;
        objectManager = ObjectManager.Instance;
        dropPrefab = objectManager.DropBoxPrefab;
        goldenDropBoxPrefab = objectManager.GoldenBoxPrefab;
        enemyPrefab = objectManager.EnemyPrefab;
        carMaterialList = objectManager.CarMaterialList;
        characterMaterialList = objectManager.CharacterMaterialList;


        playerController = PlayerController.Instance.transform;
        for (int i = 0; i < enemySpawnCount; i++)
        {
            randomSpawnPos = new Vector3(transform.GetChild(i).position.x, 0.8f, transform.GetChild(i).position.z);
            Transform enemyTransform = Instantiate(enemyPrefab, randomSpawnPos, Quaternion.Euler(0, Random.Range(0, 350), 0));
            enemyController = enemyTransform.GetComponentInChildren<EnemyController>();
            enemyController.EnemyMeshRenderer.material = carMaterialList[Random.Range(0, carMaterialList.Count)];
            int characterRandomMaterail = Random.Range(0, characterMaterialList.Count);
            if (characterRandomMaterail % 2 == 1)
            {
                characterRandomMaterail--;
            }
            Material[] chaMats = new Material[] { characterMaterialList[characterRandomMaterail], characterMaterialList[characterRandomMaterail + 1] };
            enemyController.CharacterSkinnedMesh.materials = chaMats;
            enemyCount++;
        }

    }

    private void Update()
    {
        if (!gameManager.RunGame)
        {
            return;
        }
        dropTimer += Time.deltaTime;
        if (dropTimer > 4)
        {
            dropTimer = 0;
            Vector3 dropSpawnPos = new Vector3(Random.Range(-spawnArea, spawnArea), 20, Random.Range(-spawnArea, spawnArea));
            int randomDropBox = Random.Range(0, 2);
            if (randomDropBox == 0)
            {
                Instantiate(dropPrefab, dropSpawnPos, Quaternion.identity);
            }
            else
            {
                Instantiate(goldenDropBoxPrefab, dropSpawnPos, Quaternion.identity);

            }
        }
    }



    public void UpdateEnemyCount()
    {
        enemyCount--;
        if (enemyCount < 1)
        {
            DOVirtual.DelayedCall(1, () =>
             {
                 GameManager.Instance.GameWin();
             });
        }
    }

    //public void UpdateEnemyCount(Transform getEnemy)
    //{
    //    enemyList.Remove(getEnemy);
    //    randomSpawnPos = new Vector3(transform.GetChild(Random.Range(0, transform.childCount)).position.x, 0.5f, transform.GetChild(Random.Range(0, transform.childCount)).position.z);
    //    Transform enemyTransform = Instantiate(enemyPrefab, randomSpawnPos, Quaternion.Euler(new Vector3(0, Random.Range(0f, 350f), 0)));
    //    enemyController = enemyTransform.GetComponentInChildren<EnemyController>();
    //    enemyController.EnemyMeshRenderer.material = carMaterialList[Random.Range(0, 3)];
    //    enemyList.Add(enemyTransform);
    //}


}
