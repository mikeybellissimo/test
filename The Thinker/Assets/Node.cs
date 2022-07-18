using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 forces = Vector3.zero;
    public TextMesh titleObject;
    public bool isFrozen = false;
    public Dictionary<string, Node> neighbors = new Dictionary<string, Node>();
    ForceDirectedGraph graph; 
    Transform player;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        transform.LookAt(player);
    }
    public void Construct(string title)
    {
        titleObject = transform.Find("Name").gameObject.GetComponent<TextMesh>();
        titleObject.text = title;
        print(transform.parent);
        graph = transform.parent.GetComponent<ForceDirectedGraph>();
    }
    public bool IsConnectedToFrozen(string nodeName)
    {
        if(neighbors.ContainsKey(nodeName))
        {
            return true;
        }
        return false;
    }
    public Edge GetEdge(string neighborName)
    {
        
        if(graph.edges.ContainsKey(name + neighborName))
        {
            return graph.edges[name+neighborName];
        }
        else if (graph.edges.ContainsKey(neighborName + name))
        {
            return graph.edges[neighborName+name];
        }
        print("Null edge problem");
        return null;
    }
}
