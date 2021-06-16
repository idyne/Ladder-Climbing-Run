using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class Player : MonoBehaviour
{

    private LadderClimbingRunLevel levelManager = null;
    [SerializeField] private float verticalSpeed = 1;
    private int currentLane = 2;
    [SerializeField] private Animator anim = null;
    private bool isMovementEnabled = true;
    public CameraFollow cameraFollow;
    public LevelGenerator levelGenerator;
    [SerializeField] private GameObject bloodEffect = null;
    [SerializeField] private GameObject stunEffect = null;

    public enum PlayerState { CLIMB, RIGHTJUMP, LEFTJUMP, FALL, JUMPUP, IDLE };
    [SerializeField] private PlayerState state = PlayerState.IDLE;

    RaycastHit hit;

    // Start is called before the first frame update
    private void Awake()
    {
        levelManager = (LadderClimbingRunLevel)LevelManager.Instance;

    }
    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        //anim.SetTrigger("Climb");
        cameraFollow = FindObjectOfType<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.State == GameManager.GameState.STARTED)
        {
            if (isMovementEnabled)
            {
                Climb();
                CheckLadder();

            }

            CheckInput();
            Score();

        }

    }
    private void ActivateEffect()
    {
        Destroy(Instantiate(bloodEffect, transform.position, bloodEffect.transform.rotation), 5);
    }
    private void ActivateStunEffect()
    {
        Vector3 pos = transform.position;
        pos.y += 0.3f;
        GameObject obj = Instantiate(stunEffect, pos, stunEffect.transform.rotation);
        obj.LeanMoveY(-1, 3f);
        Destroy(obj, 5);
    }

    private void CheckLadder()
    {
        Vector3 position = transform.position;
        position.y += 0.8f;
        position.z = -1;
        Debug.DrawRay(position, transform.forward * 5);
        if (state != PlayerState.JUMPUP && Physics.Raycast(position, transform.forward, out hit, 5, levelManager.LadderLayerMask))
        {

            if (hit.collider.tag == "BrokenLadder")
            {
                Fall();
                levelManager.FinishLevel(false);

            }
            else if (hit.collider.tag == "SpikeLadder")
            {
                ActivateEffect();
                Fall();

                levelManager.FinishLevel(false);
            }
        }

    }

    public void ChangeState(PlayerState newState)
    {

        if (newState == state)
            return;
        state = newState;
        if (state == PlayerState.CLIMB)
        {
            anim.SetTrigger("Climb");
        }
        else if (state == PlayerState.JUMPUP)
        {
            anim.SetTrigger("Jump");
        }
    }


    private void CheckInput()
    {

        if (SwipeManager.swipeRight)
        {
            JumpRight();

        }
        else if (SwipeManager.swipeLeft)
        {
            JumpLeft();
        }
        else if (SwipeManager.swipeUp)
        {
            JumpOver();
        }
    }
    private void JumpRight()
    {
        if (isMovementEnabled)
        {
            if (!(transform.position.x == 2))
            {
                anim.SetTrigger("RightJump");
                Vector3 pos = transform.position;
                pos.x += 2;
                pos.y += 1;
                transform.LeanMove(pos, 0.2f);
                currentLane++;
                if (currentLane == 4)
                    currentLane = 3;
            }
            anim.SetTrigger("Climb");
        }

    }
    private void JumpLeft()
    {
        if (isMovementEnabled)
        {
            if (!(transform.position.x == -2))
            {
                anim.SetTrigger("LeftJump");
                Vector3 pos = transform.position;
                pos.x -= 2;
                pos.y += 1;
                transform.LeanMove(pos, 0.2f);
                currentLane--;
            }
            anim.SetTrigger("Climb");
        }


    }
    private void JumpOver()
    {


        if (isMovementEnabled)
        {
            ChangeState(PlayerState.JUMPUP);
            anim.SetTrigger("Jump");
            Vector3 pos = transform.position;
            pos.y += 2;
            transform.LeanMove(pos, 0.4f).setOnComplete(() => { ChangeState(PlayerState.CLIMB); });
        }


    }
    private void Climb()
    {
        if (isMovementEnabled)
        {

            Vector3 pos = transform.position + Vector3.up;
            transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * verticalSpeed);
        }

    }
    private void Fall()
    {
        state = PlayerState.FALL;
        anim.SetTrigger("Fall");
        cameraFollow.Target = null;
        isMovementEnabled = false;
        Vector3 pos = transform.position + Vector3.down;
        pos.y -= 20;
        transform.LeanMove(pos, 3f);

    }
    private void Score()
    {

        levelManager.playerScore = (int)transform.position.y;
        if (levelManager.playerScore > PlayerPrefs.GetInt("HighestScore"))
        {
            levelManager.playerHighestScore = levelManager.playerScore;
            PlayerPrefs.SetInt("HighestScore", levelManager.playerHighestScore);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (state != PlayerState.FALL && state != PlayerState.IDLE)
        {
            Collectible collectible = other.GetComponent<Collectible>();
            if (collectible)
                collectible.GetCollected();
            if (other.GetComponent<Obstacles>())
            {
                anim.SetTrigger("Fall");
                //ActivateEffect();
                Fall();
            }
        }

    }


}
