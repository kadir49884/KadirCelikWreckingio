using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Transform gameWinPanel;
    [SerializeField] private Transform gameFailPanel;


    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.GameWin += GameWin;
        gameManager.GameFail += GameFail;
    }

    private void GameWin()
    {
        DOVirtual.DelayedCall(1, () =>
        {
            gameWinPanel.gameObject.SetActive(true);
        });

    }
    private void GameFail()
    {
        DOVirtual.DelayedCall(1, () =>
        {
            gameFailPanel.gameObject.SetActive(true);
        });

    }
}
