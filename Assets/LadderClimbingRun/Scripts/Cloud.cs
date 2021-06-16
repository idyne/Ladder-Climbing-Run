using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class Cloud : MonoBehaviour
{
    private Vector3 direction = Vector3.right;
    private bool unlocked = false;
    private LadderClimbingRunLevel levelManager = null;
    [SerializeField] private float speed = 1;

    private void Awake()
    {
        if (!levelManager)
            levelManager = (LadderClimbingRunLevel)LevelManager.Instance;
        if (Random.value < 0.5f)
            direction = Vector3.left;
    }

    private void Update()
    {
        if (!unlocked && levelManager.IsTargetVisible(gameObject))
            unlocked = true;
        if (unlocked)
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * speed);
    }
}
