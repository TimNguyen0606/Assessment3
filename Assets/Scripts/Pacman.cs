using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pacman : MonoBehaviour
{
    //Audio
    public AudioClip chomp1;
    public AudioClip chomp2;
    public float pacmanSpeed = 4.0f;


    private Vector2 direction = Vector2.zero;
    private Vector2 nextDirection;


    private  bool playChomp1=false;
    private AudioSource audio;

    private int pelletsconsumed = 0;

    private Node currentNode,previousNode, targetNode;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
       Node node = GetNodePosition(transform.localPosition);

        if(node != null)
        {
            currentNode = node;
            Debug.Log(currentNode);
        }

        direction = Vector2.right;
        ChangePosition(direction);
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Score:" + GameObject.Find("Game").GetComponent<Gameboard>().score);
        CheckInput(); // Check for the user input (up, dow, left, right arrows)
       Move(); //move the sprite at the speed
        UpdateDirection(); // update the orientation of the sprite
        ConsumePellet();    // method to consume pellets


    }

    void PlayChomp()
    {
        if(playChomp1)
        {
            //
            audio.PlayOneShot(chomp2);
            playChomp1 = false;
        }
        else
        {
            //
            audio.PlayOneShot(chomp1);
            playChomp1=true;
        }
    }
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangePosition(Vector2.left);
           
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangePosition(Vector2.right);
            
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(Vector2.up);
           
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(Vector2.down);
            
        }
    }

    // change position 
   void ChangePosition(Vector2 d)
    {
        if(d != direction)
        {
            nextDirection= d;
        }
        if(currentNode != null)
        {
            Node moveToNode = CanMove(d);

            if(moveToNode != null)
            {
                direction = d;
                targetNode= moveToNode;
                previousNode = currentNode;
                currentNode = null;
            }
        }
    }



    void Move()
    {
        if(targetNode != currentNode && targetNode != null)
        {

            if(nextDirection == direction * -1)
            {
                direction*= -1; //chage diection when pacman is moving

                Node tempNode = targetNode; 
                targetNode = previousNode;
                previousNode = tempNode;
            }
            if (OverShotTarget())
            {
                currentNode = targetNode;
                transform.localPosition = currentNode.transform.position;
                Node moveToNode = CanMove(nextDirection);
                if(moveToNode != null)
                {
                    direction= nextDirection;
                }
                if(moveToNode != null)
                    moveToNode = CanMove(direction); 
                if(moveToNode != null)
                {
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                }
                else
                {
                    direction=Vector2.zero;
                }


            }
            else
            {
                transform.position += (Vector3)(direction * pacmanSpeed) * Time.deltaTime;
            }
        }
        
    }

    //move from node to node
    void MoveToNode(Vector2 dir)
    {
        Node movetoNode = CanMove(dir);
        if(movetoNode != null )
        {
            transform.localPosition= movetoNode.transform.position;
            currentNode = movetoNode;
        }
    }

    void UpdateDirection()
    {
        if(direction==Vector2.left)
        {
             transform.localScale = new Vector3 (-3, 3, 3);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if(direction==Vector2.right)
        {
            transform.localScale = new Vector3(3, 3, 3);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if(direction==Vector2.up)
        {
            transform.localScale = new Vector3(3, 3, 3);
            transform.localRotation = Quaternion.Euler(0, 0, 90) ;
        }
        else if (direction == Vector2.down)
        {
            transform.localScale = new Vector3(3, 3, 3);
            transform.localRotation = Quaternion.Euler(0, 0, 270);

        }
    }
    // consume Pellets
    void ConsumePellet()
    {
        GameObject o = GetTilePosition(transform.position);

        if(o != null)
        {
            Tile tile= o.GetComponent<Tile>();
            if(tile != null)
            {
                if(!tile.diConsume && (tile.isPellet || tile.isSuperPellet))
                {
                    o.GetComponent<SpriteRenderer>().enabled = false;
                    tile.diConsume = true;
                    GameObject.Find("Game").GetComponent<Gameboard>().score += 1;

                    pelletsconsumed += 1;
                    PlayChomp();
                }
            }
        }
    }

    //checking if pacman can move
    Node CanMove(Vector2 dir)
    {
        Node moveToNode = null;
        for (int i = 0;i<currentNode.neighbors.Length;i++)
        {
            if (currentNode.ValidDirections[i]==dir)
            {
                moveToNode = currentNode.neighbors[i];
                break;
            }
        }
        return moveToNode;
    }

    // Get pacman position in motion
    GameObject GetTilePosition(Vector2 pos)
    {
        int tileX= Mathf.RoundToInt(pos.x);
        int tileY = Mathf.RoundToInt(pos.y);

        GameObject tile =  GameObject.Find("Game").GetComponent<Gameboard>().board[Mathf.Abs((int)tileX), Mathf.Abs((int)tileY)];

        if(tile != null)
            return tile;

        return null;
    }




    //Getting the position of pacman
   Node GetNodePosition(Vector2 position)
    {
        GameObject tile = GameObject.Find("Game").GetComponent<Gameboard>().board[Mathf.Abs((int)position.x), Mathf.Abs((int)position.y)];
        if(tile != null)
        {
            return tile.GetComponent<Node>();
        }
        return null;
    }

    bool OverShotTarget() {
        float nodeToTarget = LengthFromNode(targetNode.transform.position);
        float nodeToSelf = LengthFromNode(transform.localPosition);

        return nodeToSelf > nodeToTarget;
    
    }

    float LengthFromNode(Vector2 TargetPosition)
    {
        Vector2 vec= TargetPosition-(Vector2)previousNode.transform.position;
        return vec.sqrMagnitude;
    }
}
