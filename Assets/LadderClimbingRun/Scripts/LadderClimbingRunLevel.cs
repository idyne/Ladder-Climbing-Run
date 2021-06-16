using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FateGames;
using TMPro;

public class LadderClimbingRunLevel : LevelManager
{
    public int playerScore = 0;
    public int playerHighestScore = 0;
    [SerializeField] private TextMeshProUGUI coinText = null;
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private TextMeshProUGUI highestScoreText = null;
    [SerializeField] private LayerMask ladderLayerMask = 0;
    private Player player = null;
    private Camera mainCamera = null;

    public Player Player { get => player; }
    public LayerMask LadderLayerMask { get => ladderLayerMask; }

    private new void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
        coinText.text = GameManager.GOLD.ToString();
        scoreText.text = playerScore.ToString();
        highestScoreText.text = "BEST : " + PlayerPrefs.GetInt("HighestScore", 0).ToString();
        mainCamera = Camera.main;
    }
    public override void FinishLevel(bool success)
    {
        GameManager.Instance.State = GameManager.GameState.FINISHED;
        if (!success)
            LeanTween.delayedCall(1f, () => { GameManager.Instance.FinishLevel(success); });

    }

    public override void StartLevel()
    {
        player.ChangeState(Player.PlayerState.CLIMB);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = playerScore.ToString();
    }

    public void IncrementGold()
    {
        GameManager.GOLD++;
        coinText.text = GameManager.GOLD.ToString();
    }

    public bool IsTargetVisible(GameObject go)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        var point = go.transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
                return false;
        }
        return true;
    }
}
