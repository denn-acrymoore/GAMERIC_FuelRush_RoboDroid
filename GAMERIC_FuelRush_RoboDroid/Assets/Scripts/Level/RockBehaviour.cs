using UnityEngine;

public class RockBehaviour : MonoBehaviour
{
    private bool isRockMoving = false;
    private Vector3 targetPos;
    private float moveSpeed = 5f;
    private float gridSize;

    [SerializeField] private AudioSource rockPunchedSound;

    private void Start()
    {
        gridSize = CustomGridManager.getGridSize();
    }

    private void Update()
    {
        if (isRockMoving)
        {
            MovingRock();
        }
    }

    public void MoveRock(Vector3 direction)
    {
        rockPunchedSound.Play();
        targetPos = transform.position + direction.normalized * gridSize;
        isRockMoving = true;
        CustomGridManager.isObjectMoving = true;
    }

    private void MovingRock()
    {
        float distance = Vector3.Distance(transform.position, targetPos);

        if (distance < 0.1)
        {
            isRockMoving = false;
            CustomGridManager.isObjectMoving = false;
        }
        else
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            transform.Translate(direction * Time.deltaTime * moveSpeed
                * AnimSpeedMenuManager.animSpeedMultiplier, Space.World);
        }
    }
}
