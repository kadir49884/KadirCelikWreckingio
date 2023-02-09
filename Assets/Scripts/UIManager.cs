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
        gameWinPanel.gameObject.SetActive(true);
    }
    private void GameFail()
    {
        gameFailPanel.gameObject.SetActive(true);
    }
    
}
