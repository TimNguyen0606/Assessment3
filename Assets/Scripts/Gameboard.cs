using UnityEngine;

public class Gameboard : MonoBehaviour
{
    private static int boardwidth = 100;
    private static int boardheight = 50;


    public int totalPellets = 0;
    public int score = 0;

    public GameObject[,] board = new GameObject[boardwidth, boardheight];
    // Start is called before the first frame update
    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject obj in objects)
        {
            Vector2 pos = obj.transform.position;
            if (obj.name != "Pacman" && obj.name != "Nodes" && obj.name != "NonNodes" && obj.name != "Maze" && obj.name != "Pallets")
            {
                if (obj.GetComponent<Tile>() != null)
                {
                    if (obj.GetComponent<Tile>().isPellet || obj.GetComponent<Tile>().isPellet)
                    {
                        totalPellets++;
                    }

                }
                board[Mathf.Abs((int)pos.x), Mathf.Abs((int)pos.y)] = obj;

            }
            else
            {
                Debug.Log("Pacman is:" + pos);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
