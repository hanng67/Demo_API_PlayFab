using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlay : MonoBehaviour
{
    private const string HIGH_SCORE_KEY = "HighScore";

    public enum GameState
    {
        Game_Start,
        Spawning_Blocks,
        Waitting_Input,
        Moving_Blocks,
        Game_Over,
    }


    [SerializeField] private Transform gridTransform;
    [SerializeField] private Transform blockTemplateTransform;
    [SerializeField] private GameObject gameOverObject;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private TextMeshProUGUI scoreText;

    private Dictionary<int, BlockType> blockTypeDict;
    private BlockType[] blockTypes = new BlockType[] {
        new BlockType(0, "767676"),
        new BlockType(2, "DF66BC"),
        new BlockType(4, "60BC42"),
        new BlockType(8, "3AB8CB"),
        new BlockType(16, "3680DB"),
        new BlockType(32, "E86947"),
        new BlockType(64, "8B80FB"),
        new BlockType(128, "968980"),
        new BlockType(256, "FFAD29"),
        new BlockType(512, "FE6472"),
        new BlockType(1024, "C42A09"),
        new BlockType(2048, "D33CEC"),
        new BlockType(4096, "800000"),
        new BlockType(8192, "808000"),
        new BlockType(16384, "008000"),
        new BlockType(32768, "008080"),
        new BlockType(65536, "000080"),
    };

    private List<Node> nodeList;
    private List<Block> blockList;

    private GameState gameState;
    private int round;
    private int score;
    private int highScore;
    private int gridWidth = 4;
    private int gridHeight = 4;

    private void Awake()
    {
        blockTemplateTransform.gameObject.SetActive(false);
        gameOverObject.SetActive(false);
        nodeList = new List<Node>();
        blockList = new List<Block>();
        blockTypeDict = new Dictionary<int, BlockType>();
        InitValueBlockTypeDict(blockTypes);

        playAgainButton.onClick.AddListener(() =>
        {
            ClearGrid();
            ChangeGameState(GameState.Game_Start);
        });

        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);

        Hide();
    }

    private void OnEnable()
    {
        ChangeGameState(GameState.Game_Start);
    }

    private void OnDisable()
    {
        ChangeGameState(GameState.Game_Over);
        ClearGrid();
        Hide();
    }

    private void InitValueBlockTypeDict(BlockType[] blockTypes)
    {
        for (int i = 0; i < blockTypes.Length; i++)
        {
            blockTypeDict.Add(i, blockTypes[i]);
        }
    }

    private void ChangeGameState(GameState gameState)
    {
        this.gameState = gameState;

        switch (gameState)
        {
            case GameState.Game_Start:
                GameInit();
                break;
            case GameState.Spawning_Blocks:
                SpawnBlock(round++ == 0 ? 2 : 1);
                break;
            case GameState.Waitting_Input:
                break;
            case GameState.Moving_Blocks:
                break;
            case GameState.Game_Over:
                ShowGameOver();
                break;
        }
    }

    private void Update()
    {
        if (!IsWaittingInput()) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveBlocks(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveBlocks(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveBlocks(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveBlocks(Vector2Int.right);
        }
    }

    private void GameInit()
    {
        round = 0;
        score = 0;
        scoreText.text = "Score: \n" + score;
        nodeList = new List<Node>();
        blockList = new List<Block>();
        gameOverObject.SetActive(false);

        if (nodeList.Count == 0)
        {
            nodeList = gridTransform.GetComponentsInChildren<Node>().ToList();

            int index = 0;
            for (int y = gridHeight - 1; y >= 0; y--)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    nodeList[index++].coordinates = new Vector2Int(x, y);
                }
            }
        }

        ChangeGameState(GameState.Spawning_Blocks);
    }

    private void SpawnBlock(int amount)
    {
        var freeNodes = nodeList.Where(n => n.occupiedBlock == null).OrderBy(n => Random.value).ToList();

        foreach (var node in freeNodes.Take(amount))
        {
            Transform blockTransform = Instantiate(blockTemplateTransform, gridTransform);
            blockTransform.gameObject.SetActive(true);
            Block block = blockTransform.GetComponent<Block>();
            block.Spawn(node);
            block.SetBlockType(GetBlockTypeByLevel(Random.value < .8f ? BlockLevel.Level_1 : BlockLevel.Level_2));
            blockList.Add(block);
        }

        int gridSize = gridWidth * gridHeight;
        if (blockList.Count == gridSize)
        {
            CheckForGameOver();
        }
        else
        {
            ChangeGameState(GameState.Waitting_Input);
        }
    }

    private void MoveBlocks(Vector2Int direction)
    {
        ChangeGameState(GameState.Moving_Blocks);
        var orderBlocks = blockList.OrderBy(b => b.coordinates.x).ThenBy(b => b.coordinates.y).ToList();

        if (direction == Vector2Int.up || direction == Vector2Int.right)
        {
            orderBlocks.Reverse();
        }

        bool isMoved = false;

        foreach (Block block in orderBlocks)
        {
            isMoved |= MoveBlock(block, direction);
        }

        if (isMoved)
        {
            StartCoroutine(WaitForChangeState());
        }
        else
        {
            ChangeGameState(GameState.Waitting_Input);
        }
    }

    private bool MoveBlock(Block block, Vector2Int direction)
    {
        Node newNode = null;
        Node nextNode = GetNextNode(block.node, direction);

        while (nextNode != null)
        {
            if (nextNode.occupiedBlock != null)
            {
                if (nextNode.occupiedBlock.CanMerge(block))
                {
                    MergeBlock(block, nextNode.occupiedBlock);

                    return true;
                }

                break;
            }

            newNode = nextNode;
            nextNode = GetNextNode(newNode, direction);
        }

        if (newNode != null)
        {
            block.MoveTo(newNode);
            return true;
        }

        return false;
    }

    private void MergeBlock(Block fromBlock, Block toBlock)
    {
        blockList.Remove(fromBlock);
        fromBlock.MergeTo(toBlock.node);

        int currentLevel = Mathf.CeilToInt(Mathf.Log(toBlock.value, 2));
        BlockLevel nextLevel = (BlockLevel)(currentLevel + 1);
        BlockType nextBlockType = GetBlockTypeByLevel(nextLevel);
        toBlock.SetBlockType(nextBlockType);

        UpdateScoreText(nextBlockType.value);
    }

    private IEnumerator WaitForChangeState()
    {
        while (blockList.Any(b => b.isMoving))
        {
            yield return null;
        }

        foreach (var block in blockList)
        {
            block.isLocked = false;
        }

        ChangeGameState(GameState.Spawning_Blocks);
    }

    private void CheckForGameOver()
    {
        bool isGameOver = true;

        foreach (var block in blockList)
        {
            Node nodeUp = GetNextNode(block.node, Vector2Int.up);
            Node nodeDown = GetNextNode(block.node, Vector2Int.down);
            Node nodeLeft = GetNextNode(block.node, Vector2Int.left);
            Node nodeRight = GetNextNode(block.node, Vector2Int.right);

            if (nodeUp != null && block.CanMerge(nodeUp.occupiedBlock))
            {
                isGameOver = false;
                break;
            }

            if (nodeDown != null && block.CanMerge(nodeDown.occupiedBlock))
            {
                isGameOver = false;
                break;
            }

            if (nodeLeft != null && block.CanMerge(nodeLeft.occupiedBlock))
            {
                isGameOver = false;
                break;
            }

            if (nodeRight != null && block.CanMerge(nodeRight.occupiedBlock))
            {
                isGameOver = false;
                break;
            }
        }

        if (isGameOver)
        {
            ChangeGameState(GameState.Game_Over);
        }
        else
        {
            ChangeGameState(GameState.Waitting_Input);
        }
    }

    private void ShowGameOver()
    {
        gameOverObject.SetActive(true);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        }
    }

    public void UpdateScoreText(int value)
    {
        score += value;
        scoreText.text = "Score: \n" + score;
    }

    private Node GetNodeByCoordinates(Vector2Int coordinates)
    {
        return nodeList.FirstOrDefault(n => n.coordinates == coordinates);
    }

    private Node GetNextNode(Node node, Vector2Int direction)
    {
        Vector2Int nextCoordinates = node.coordinates + direction;
        if (nextCoordinates.x < 0 || nextCoordinates.x >= gridWidth || nextCoordinates.y < 0 || nextCoordinates.y >= gridHeight)
        {
            return null;
        }
        return GetNodeByCoordinates(nextCoordinates);
    }

    private BlockType GetBlockTypeByLevel(BlockLevel blockLevel)
    {
        return blockTypeDict[(int)blockLevel];
    }

    private bool IsWaittingInput()
    {
        return gameState == GameState.Waitting_Input;
    }

    private void ClearGrid()
    {
        foreach (var block in blockList)
        {
            Destroy(block.gameObject);
        }
        foreach (var node in nodeList)
        {
            node.occupiedBlock = null;
        }
        blockList.Clear();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public struct BlockType
{
    public int value;
    public Color color;

    public BlockType(int value, string hexColor)
    {
        this.value = value;
        this.color = ColorUtils.GetColorFromString(hexColor);
    }
}

public enum BlockLevel
{
    Level_0, //0
    Level_1, //2
    Level_2, //4
    Level_3, //8
    Level_4, //16
    Level_5, //32
    Level_6, //64
    Level_7, //128
    Level_8, //256
    Level_9, //512
    Level_10, //1024
    Level_11, //2048
    Level_12, //4096
    Level_13, //8192
    Level_14, //16384
    Level_15, //32768
    Level_16, //65536
}
