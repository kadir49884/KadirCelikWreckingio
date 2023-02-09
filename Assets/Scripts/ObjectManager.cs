using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform dropBoxPrefab;
    [SerializeField] private Transform goldenBoxPrefab;
    [SerializeField] private Transform hitParticle;
    [SerializeField] private List<Material> carMaterialList = new List<Material>();
    [SerializeField] private List<Material> characterMaterialList = new List<Material>();
    [SerializeField] private List<Sprite> emojiList = new List<Sprite>();
    [SerializeField] private List<Transform> ballList = new List<Transform>();


    public Transform EnemyPrefab { get => enemyPrefab; set => enemyPrefab = value; }
    public List<Material> CarMaterialList { get => carMaterialList; set => carMaterialList = value; }
    public Transform DropBoxPrefab { get => dropBoxPrefab; set => dropBoxPrefab = value; }
    public List<Sprite> EmojiList { get => emojiList; set => emojiList = value; }
    public List<Transform> BallList { get => ballList; set => ballList = value; }
    public List<Material> CharacterMaterialList { get => characterMaterialList; set => characterMaterialList = value; }
    public Transform HitParticle { get => hitParticle; set => hitParticle = value; }
    public Transform GoldenBoxPrefab { get => goldenBoxPrefab; set => goldenBoxPrefab = value; }
}
