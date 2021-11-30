using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource[] walkSounds;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource loseSound;

    private Animator anim;
    private bool isPlayerMoving = false;
    private Vector3 direction;
    private Vector3 targetPos;
    private float moveSpeed = 5f;
    private float gridSize;

    public delegate void BoxAction();
    public static event BoxAction OnBoxPunched;

    public delegate void RockAction();
    public static event RockAction OnRockPunched;

    public delegate void PlayerDeadActionFinish();
    public static event PlayerDeadActionFinish OnPlayerDeadAnimFinish;

    public delegate void PlayerWinActionFinish();
    public static event PlayerWinActionFinish OnPlayerWinAnimFinish;

    private void Start()
    {
        anim = GetComponent<Animator>();
        gridSize = CustomGridManager.getGridSize();
    }

    private void Update()
    {
        if (isPlayerMoving)
        {
            MovingPlayer();
        }
    }

    public void MovePlayer(Vector3 direction)
    {
        this.direction = direction.normalized;
        targetPos = transform.position + direction.normalized * gridSize;
        isPlayerMoving = true;
        CustomGridManager.isPlayerMoving = true;
    }

    private void MovingPlayer()
    {
        float distance = Vector3.Distance(transform.position, targetPos);

        if (distance < 0.1)
        {
            anim.SetBool("IsSprinting", false);
            transform.position = new Vector3(Mathf.Round(transform.position.x)
                , Mathf.Round(transform.position.y)
                , Mathf.Round(transform.position.z));
            isPlayerMoving = false;
            CustomGridManager.isPlayerMoving = false;
        }
        else
        {
            anim.SetBool("IsSprinting", true);
            transform.rotation = Quaternion.LookRotation(direction);
            transform.Translate(
                direction * Time.deltaTime * moveSpeed
                * AnimSpeedMenuManager.animSpeedMultiplier
                , Space.World);
        }
    }

    public void PlayerPunchStone(Vector3 direction)
    {
        CustomGridManager.isPlayerMoving = true;
        transform.rotation = Quaternion.LookRotation(direction);
        anim.SetTrigger("PunchingStone");
    }

    public void PlayerPunchBox(Vector3 direction)
    {
        CustomGridManager.isPlayerMoving = true;
        transform.rotation = Quaternion.LookRotation(direction);
        anim.SetTrigger("PunchingBox");
    }

    public void PlayerDeath()
    {
        loseSound.Play();
        anim.SetTrigger("GameOver");
    }

    public void PlayerWin()
    {
        winSound.Play();
        anim.SetTrigger("Win");
    }

    public void OnPlayerFootAnimTouchGround()
    {
        int idx = Random.Range(0, walkSounds.Length);
        walkSounds[idx].Play();
    }

    private void OnPlayerContactToStone()
    {
        if (OnRockPunched != null)
            OnRockPunched();
    }

    private void OnPlayerContactToBox()
    {
        if (OnBoxPunched != null)
            OnBoxPunched();
    }

    private void OnPlayerMoveDone()
    {
        CustomGridManager.isPlayerMoving = false;
    }

    private void OnPlayerDeadAnimLoopOnce()
    {
        if (OnPlayerDeadAnimFinish != null)
            OnPlayerDeadAnimFinish();
    }

    private void OnPlayerWinAnimLoopOnce()
    {
        if (OnPlayerWinAnimFinish != null)
            OnPlayerWinAnimFinish();
    }
}
