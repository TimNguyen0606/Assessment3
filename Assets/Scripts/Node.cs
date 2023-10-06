using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node[] neighbors;
    public Vector2[] ValidDirections;
    // Start is called before the first frame update
    void Start()
    {
      
        ValidDirections = new Vector2[neighbors.Length];
        for (int i = 0; i < neighbors.Length; i++)
        {
            Node neighbor = neighbors[i];
            Vector2 tempVector = neighbor.transform.localPosition - transform.position;
            ValidDirections[i] = tempVector.normalized;
        }
       
    }

    
}
