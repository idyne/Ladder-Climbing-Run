using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class Obstacles : Collectible
{
    public LevelGenerator levelGenerator;
    private bool unlocked = false;
    public override void GetCollected()
    {
        print(" Death ! ");
        ActivateEffect();
        levelManager.FinishLevel(false);

    }
    private void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
    }
    private void Update()
    {
        if (GameManager.Instance.State == GameManager.GameState.STARTED && !unlocked && Mathf.Abs(levelManager.Player.transform.position.y - transform.position.y) < 10)
            unlocked = true;
        if (unlocked)
            MoveDown();
    }
    private void MoveDown()
    {
        Vector3 pos = transform.position + -Vector3.up;
        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * 1f);
        transform.Rotate(Vector3.up, Time.deltaTime * 30);

    }

}
