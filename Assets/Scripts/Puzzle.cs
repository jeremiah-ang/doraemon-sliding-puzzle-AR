using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Puzzle : MonoBehaviour
{
    private const int NUMBER_OF_TILES = 8;
    private const float xRange = 1.1f;
    private const float zRange = -1.1f;
    private float[] xPositions = { -xRange, 0, xRange, -xRange, 0, xRange, -xRange, 0, xRange, };
    private float[] zPositions = { -zRange, -zRange, -zRange, 0, 0, 0, zRange, zRange, zRange, };

    private int emptyTile = NUMBER_OF_TILES;
    private int[] tilePositions;
    private int[] tileSolutions;

    private bool isGameActive = false;
    private float elapsedTime = 0;
    [SerializeField] private TextMeshProUGUI timerText;

    public List<GameObject> tiles;
    public List<Material> materials;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit Hit;
            if (Physics.Raycast(ray, out Hit))
            {
                switch(Hit.transform.name)
                {
                    case "Tile 0":
                        TapTile(0);
                        break;
                    case "Tile 1":
                        TapTile(1);
                        break;
                    case "Tile 2":
                        TapTile(2);
                        break;
                    case "Tile 3":
                        TapTile(3);
                        break;
                    case "Tile 4":
                        TapTile(4);
                        break;
                    case "Tile 5":
                        TapTile(5);
                        break;
                    case "Tile 6":
                        TapTile(6);
                        break;
                    case "Tile 7":
                        TapTile(7);
                        break;
                    default:
                        break;
                }
            }
        }
        UpdateTimer(Time.deltaTime);
    }

    private void ResetTimer()
    {
        elapsedTime = 0;
        UpdateTimer(0);
    }

    private void UpdateTimer(float deltaTime)
    {
        if (!isGameActive)
        {
            timerText.color = new Color32(255, 126, 126, 255);
            if (elapsedTime == 0) timerText.text = "--:--:--";
            return;
        };
        timerText.color = Color.white;
        elapsedTime += deltaTime;
        int seconds = (int)(elapsedTime % 60);
        int minutes = (int)(elapsedTime / 60 % 60);
        int hours = (int)(elapsedTime / 360);
        timerText.text = hours.ToString().PadLeft(2, '0')
                            + ':' + minutes.ToString().PadLeft(2, '0')
                            + ':' + seconds.ToString().PadLeft(2, '0');
    }

    private void TapTile(int tileNumber)
    {
        print(tileNumber);
    }

    private void Init()
    {
        emptyTile = NUMBER_OF_TILES;
        isGameActive = true;
        ResetTimer();
        tilePositions = new int[NUMBER_OF_TILES];
        for (int i = 0; i < 8; i++)
        {
            tilePositions[i] = i;
        }
        AssignSolution();
    }

    private void AssignSolution()
    {
        tileSolutions = GenerateRandomOrderArray();
        AssignImage();
    }

    private void AssignImage()
    {
        for (int i = 0; i < tileSolutions.Length; i++)
        {
            int image = tileSolutions[i];
            GameObject tile = tiles[i];
            tile.GetComponent<Renderer>().material = materials[image];
        }
    }

    private int[] GenerateRandomOrderArray(int size = 8)
    {
        int[] arr = new int[size];
        for (int i = 0; i < size; i++)
        {
            arr[i] = i;
        }
        for (int i = 0; i < size; i++)
        {
            int j = Random.Range(0, size);
            Swap(arr, i, j);
        }
        if (!IsSolvable(arr))
        {
            Swap(arr, 0, 1);
        }
        return arr;
    }

    private enum Movement : int
    {
        UP = 1,
        LEFT = 2,
        DOWN = 3,
        RIGHT = 4,
        INVALID = -1
    }

    private Movement CanMove(int tilePosition)
    {
        if (emptyTile - 3 == tilePosition) return Movement.UP;
        if (emptyTile - 1 == tilePosition) return Movement.LEFT;
        if (emptyTile + 3 == tilePosition) return Movement.DOWN;
        if (emptyTile + 1 == tilePosition) return Movement.RIGHT;
        return Movement.INVALID;
    }

    private Vector3 getTileVector3Position(int position)
    {
        return new Vector3(xPositions[position], 0, zPositions[position]);
    }

    private bool IsSolved()
    {
        for (int i = 0; i < tilePositions.Length; i++)
        {
            if (tilePositions[i] != tileSolutions[i])
            {
                return false;
            }
        }
        return true;
    }

    private void CheckSolution()
    {
         if (IsSolved())
        {
            isGameActive = false;
            print("SOLVED!");
        }
    }

    public void StartGame()
    {
        Init();
    }

    public void GiveUp()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].transform.localPosition = getTileVector3Position(tileSolutions[i]);
            tilePositions[i] = tileSolutions[i];
        }
        CheckSolution();
    }

    public void MoveTile(int tileNumber)
    {
        if (!isGameActive) return;

        int tilePosition = tilePositions[tileNumber];
        Movement movement = CanMove(tilePosition);
        if (movement == Movement.INVALID) return;
        tiles[tileNumber].transform.localPosition = getTileVector3Position(emptyTile);
        tilePositions[tileNumber] = emptyTile;
        emptyTile = tilePosition;
        CheckSolution();
    }


    static private void Swap(int[] arr, int i, int j)
    {
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }

    static private bool IsSolvable(int[] arr)
    {
        return CountInversion(arr) % 2 == 0;
    }

    static private int CountInversion(int[] arr)
    {
        int inversion = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            for (int j = 0; j < arr.Length; j++)
            {
                if (arr[i] > arr[j]) inversion++;
            }   
        }
        return inversion;
    }
}
