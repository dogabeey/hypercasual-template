using Dogabeey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINode : MonoBehaviour
{
    public static List<UINode> nodes = new List<UINode>();

    public UINode leftNode, rightNode, upNode, downNode;

    void Start()
    {
        nodes.Add(this);

        DetectAndAssignNeighbors();
    }
    private void OnDestroy()
    {
        nodes.Remove(this);
    }
    
    // Detect and assign left, right, up and down nodes based on transform's position and their closest neighbor on corresponding direction.
    public void DetectAndAssignNeighbors()
    {
        float minLeftDistance = Mathf.Infinity;
        float minRightDistance = Mathf.Infinity;
        float minUpDistance = Mathf.Infinity;
        float minDownDistance = Mathf.Infinity;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] != this)
            {
                float distance = Vector3.Distance(transform.position, nodes[i].transform.position);
                if (nodes[i].transform.position.x < transform.position.x)
                {
                    if (distance < minLeftDistance)
                    {
                        minLeftDistance = distance;
                        leftNode = nodes[i];
                    }
                }
                else if (nodes[i].transform.position.x > transform.position.x)
                {
                    if (distance < minRightDistance)
                    {
                        minRightDistance = distance;
                        rightNode = nodes[i];
                    }
                }
                else if (nodes[i].transform.position.y > transform.position.y)
                {
                    if (distance < minUpDistance)
                    {
                        minUpDistance = distance;
                        upNode = nodes[i];
                    }
                }
                else if (nodes[i].transform.position.y < transform.position.y)
                {
                    if (distance < minDownDistance)
                    {
                        minDownDistance = distance;
                        downNode = nodes[i];
                    }
                }
            }
        }
    }
}
