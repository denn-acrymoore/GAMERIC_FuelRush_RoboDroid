using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomGridManager : MonoBehaviour
{
    [SerializeField] private int numberOfActionsLeft;
    [SerializeField] private int rowNumber;
    [SerializeField] private int colNumber;
    [SerializeField] private GameObject floorListObject;

    [SerializeField] private GameObject winMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI energyLeftText;

    private CustomGrid[,] customGrids;
    private Vector2 playerPos;
    private Vector2 goalPos;
    private GameObject player;

    public static bool isMenuOpened;
    public static bool isWin;
    public static bool isGameOver;
    public static bool isPlayerMoving;
    public static bool isObjectMoving;
    private static float gridSize = 2f;

    // This is used for player animation event callback to call box and rock behaviour:
    private GameObject currRock;
    private GameObject currBox;
    private Vector3 currWorldDirection;

    public static float getGridSize()
    {
        return gridSize;
    }

    private void OnEnable()
    {
        PlayerBehaviour.OnBoxPunched += this.OnBoxPunched;
        PlayerBehaviour.OnRockPunched += this.OnRockPunched;
        PlayerBehaviour.OnPlayerDeadAnimFinish+= this.OnPlayerDeadAnimFinish;
        PlayerBehaviour.OnPlayerWinAnimFinish += this.OnPlayerWinAnimFinish;
    }

    private void OnDisable()
    {
        PlayerBehaviour.OnBoxPunched -= this.OnBoxPunched;
        PlayerBehaviour.OnRockPunched -= this.OnRockPunched;
        PlayerBehaviour.OnPlayerDeadAnimFinish -= this.OnPlayerDeadAnimFinish;
        PlayerBehaviour.OnPlayerWinAnimFinish -= this.OnPlayerWinAnimFinish;
    }

    private void Awake()
    {
        isMenuOpened = false;
        isWin = false;
        isGameOver = false;
        isPlayerMoving = false;
        isObjectMoving = false;

        customGrids = new CustomGrid[rowNumber, colNumber];
    }

    private void Start()
    {
        energyLeftText.SetText(numberOfActionsLeft.ToString());

        CustomGrid[] floorList = floorListObject.GetComponentsInChildren<CustomGrid>();

        if (floorList.Length == rowNumber * colNumber)
        {
            for (int i = 0; i < rowNumber; ++i)
            {
                for (int j = 0; j < colNumber; ++j)
                {
                    customGrids[i, j] = floorList[(i * colNumber) + j];
                    if (customGrids[i, j].GetObjectWithinGrid() != null)
                    {
                        GameObject objWithinGrid = customGrids[i, j].GetObjectWithinGrid();

                        if (objWithinGrid.tag == "Player")
                        {
                            playerPos = new Vector2(i, j);
                            player = objWithinGrid;
                        }
                        else if (objWithinGrid.tag == "Finish")
                        {
                            goalPos = new Vector2(i, j);
                        }
                    }

                    //Debug.Log("[" + i + ", " + j + "]: " + customGrids[i, j].gameObject.name);
                }
            }
        }
        else
        {
            Debug.LogError("Error: Invalid number of row and column");
        }

        //Debug.Log("Player Grid: " + playerCurrGrid.gameObject.name);
        //Debug.Log("Goal Grid: " + goalGrid.gameObject.name);
    }

    private void Update()
    {
        energyLeftText.SetText(numberOfActionsLeft.ToString());

        if (!isPlayerMoving && !isObjectMoving && !isWin && !isGameOver)
        {
            if (numberOfActionsLeft <= 0 && playerPos != goalPos)
            {
                isGameOver = true;

                player.GetComponent<PlayerBehaviour>().PlayerDeath();
            }
            else if (playerPos == goalPos)
            {
                isWin = true;

                player.GetComponent<PlayerBehaviour>().PlayerWin();
            }
        }

        if (!isMenuOpened && !isWin && !isGameOver && !isPlayerMoving && !isObjectMoving)
        {
            HandlePlayerAction();
        }
    }

    private void HandlePlayerAction()
    {
        Vector2 nextActionPos = new Vector2();
        bool playerInputExist = false;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            nextActionPos = playerPos + new Vector2(-1, 0);
            playerInputExist = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nextActionPos = playerPos + new Vector2(0, -1);
            playerInputExist = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            nextActionPos = playerPos + new Vector2(1, 0);
            playerInputExist = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextActionPos = playerPos + new Vector2(0, 1);
            playerInputExist = true;
        }

        if (playerInputExist && !IsOutOfBound(nextActionPos))
        {
            DeterminePlayerAction(nextActionPos);
        }
    }
    private bool IsOutOfBound(Vector2 pos)
    {
        if (pos.x < 0 || pos.x >= rowNumber || 
            pos.y < 0 || pos.y >= colNumber)
        {
            return true;
        }

        return false;
    }

    private Vector3 gridDirectionToWorldDirection(Vector2 gridDirection)
    {
        return new Vector3(gridDirection.y, 0, -gridDirection.x);
    }

    private void DeterminePlayerAction(Vector2 nextActionPos)
    {
        Vector2 direction = nextActionPos - playerPos;

        GameObject objWithinGrid = customGrids[(int)nextActionPos.x, (int)nextActionPos.y]
            .GetObjectWithinGrid();

        // EMPTY GRID:
        if (objWithinGrid == null)
        {
            customGrids[(int)playerPos.x, (int)playerPos.y].SetObjectWithinGrid(null);
            customGrids[(int)nextActionPos.x, (int)nextActionPos.y].SetObjectWithinGrid(player);
            playerPos = nextActionPos;
            PlayerMoveAction(player, gridDirectionToWorldDirection(direction));
        }

        // ROCK:
        else if (objWithinGrid.tag == "Rock")
        {
            GameObject objWithinNextRockGridPos;

            if (IsOutOfBound(nextActionPos + direction))
            {
                return;
            }

            objWithinNextRockGridPos
                    = customGrids[(int)(nextActionPos.x + direction.x)
                        , (int)(nextActionPos.y + direction.y)].GetObjectWithinGrid();

            // Check if next rock position is not out of bound and is empty:
            if (objWithinNextRockGridPos == null)
            {
                customGrids[(int)nextActionPos.x, (int)nextActionPos.y].SetObjectWithinGrid(null);

                customGrids[(int)(nextActionPos.x + direction.x)
                    , (int)(nextActionPos.y + direction.y)]
                    .SetObjectWithinGrid(objWithinGrid);

                PlayerRockAction(player, objWithinGrid, gridDirectionToWorldDirection(direction));
            }
        }

        // BOX:
        else if (objWithinGrid.tag == "Box")
        {
            customGrids[(int)nextActionPos.x, (int)nextActionPos.y].SetObjectWithinGrid(null);
            PlayerBoxAction(player, objWithinGrid, gridDirectionToWorldDirection(direction));
        }

        // FINISH GRID:
        else if (objWithinGrid.tag == "Finish")
        {
            customGrids[(int)playerPos.x, (int)playerPos.y].SetObjectWithinGrid(null);
            customGrids[(int)nextActionPos.x, (int)nextActionPos.y].SetObjectWithinGrid(player);
            playerPos = nextActionPos;
            PlayerMoveAction(player, gridDirectionToWorldDirection(direction));
        }
    }

    private void PlayerRockAction(GameObject player, GameObject rock, Vector3 direction)
    {
        numberOfActionsLeft--;
        currRock = rock;
        currWorldDirection = direction;
        player.GetComponent<PlayerBehaviour>().PlayerPunchStone(direction);
    }

    private void PlayerBoxAction(GameObject player,  GameObject box, Vector3 direction)
    {
        numberOfActionsLeft--;
        currBox = box;
        player.GetComponent<PlayerBehaviour>().PlayerPunchBox(direction);
    }

    private void PlayerMoveAction(GameObject player, Vector3 direction)
    {
        numberOfActionsLeft--;
        player.GetComponent<PlayerBehaviour>().MovePlayer(direction);
    }

    // This methods will be called when player punch contact the rock or box:
    private void OnRockPunched()
    {
        currRock.GetComponent<RockBehaviour>().MoveRock(currWorldDirection);
    }

    private void OnBoxPunched()
    {
        currBox.GetComponent<BoxBehaviour>().DestroyBox();
    }

    private void OnPlayerDeadAnimFinish()
    {
        gameOverPanel.SetActive(true);
    }

    private void OnPlayerWinAnimFinish()
    {
        winMenuPanel.SetActive(true);
    }
}
