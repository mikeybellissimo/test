using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ForceDirectedGraph : MonoBehaviour
{


    bool isWaiting = true;
    
    Dictionary<string, Node> nodes;
    Dictionary<string, Edge> edges;
    Vector3 centerOfGraph = Vector3.zero;
    
    Knowledge knowledge;
    public GameObject nodePrefab;
    public GameObject edgePrefab;
    public Material edgeMaterial;

    public float maxDistance = 30;
    public float anchorForce = 1;
    public int startSpace = 5;
    public float repulsionStrength = 10;
    
    public float repulsionDistance = 3f;
    public float attractionStrength = 10;

    //build the Graph from the data 
    public void Start()
    {
        nodes = new Dictionary<string, Node>();
        edges = new Dictionary<string, Edge>();
        SetupKnowledgeBase();

    } 

    public void Update()
    {
        if(isWaiting == false)
        {
            ApplyForces();
            UpdateEdges();
        }
            

    }

    async void SetupKnowledgeBase()
    {
        while(Knowledge.isAwaiting == true)
        {
            await Task.Yield();
        }
        knowledge = Knowledge.knowledge;
        CreateNodes();
        ConnectNodes();
        isWaiting = false;
    }
    
    private Node node1;
    private Node node2;
    Vector3 placeItShouldBe;
    Vector3 attractiveForce;
    Vector3 repulsiveForce;
    void ApplyForces()
    {

        //all attached nodes attract
        
        foreach (KeyValuePair<string, Edge> p in edges)
        {
            Edge edge = p.Value;
            attractiveForce = ComputeAttractiveForce(edge);
            edge.node1.forces += attractiveForce;
            edge.node2.forces -= attractiveForce;
            

        }
        //all nodes repulse eachother
        // i and j are used to skip over the ones that have already been done so as to not double count them
        int i = 0;
        //loop through each relationship and calculate the force for each one then apply them all 
        

        foreach( KeyValuePair<string, Node> p in nodes)
        {
            node1 = p.Value;
            int j = 0;
            foreach( KeyValuePair<string, Node> q in nodes)
            {
                if (j < i)
                {
                    j++;
                    continue;
                }
                node2 = q.Value;
                if(!p.Key.Equals(q.Key))
                {
                    repulsiveForce = CalculateRepulsiveForce(node1, node2);
                    node1.forces += repulsiveForce;
                    node2.forces += repulsiveForce;
                    print(repulsiveForce);
                    

                }
            }
            i++;
            centerOfGraph += (1.0f/nodes.Count) * node1.transform.position;
            //distance from 0 is equal to position of node
            placeItShouldBe = node1.transform.position - centerOfGraph; 
            print((-anchorForce * ( node1.transform.position - placeItShouldBe)).normalized);
            
            node1.forces += (-anchorForce * ( node1.transform.position - placeItShouldBe)).normalized;
            if(Vector3.Distance(node1.transform.position, centerOfGraph) > maxDistance)
            {
                node1.GetComponent<Rigidbody>().velocity = Vector3.zero;
                node1.forces = -transform.position;
            } 
            //apply force here and zero it out right after
            node1.GetComponent<Rigidbody>().AddForce(node1.forces.normalized );
            
            centerOfGraph = Vector3.zero;
            node1.forces = Vector3.zero;
        }
        
        
        

    }

    void UpdateEdges()
    {
        Vector3[] lineCoords = new Vector3[2];
        foreach(KeyValuePair<string, Edge> p in edges)
        {
            Edge edge = p.Value;
            LineRenderer lr = edge.GetComponent<LineRenderer>();
            lineCoords[0] = edge.node1.transform.position;
            lineCoords[1] = edge.node2.transform.position;
            lr.SetPositions(lineCoords);
        }


    }

    //loop through data and create nodes
    void CreateNodes()
    {
        
        
        foreach (string value in knowledge.taxonomy.children.Values)
        {
            //make sure the node doesn't already exist
            if(!nodes.ContainsKey(value))
            {   
                GameObject newNode = Instantiate(nodePrefab, new Vector3(Random.Range(-startSpace, startSpace), Random.Range(-startSpace, startSpace),Random.Range(-startSpace, startSpace) ), Quaternion.identity);
                newNode.name = value;
                nodes[value] = newNode.GetComponent<Node>();
                
            }
            
        }
        //delete this when you have real data 
        

        //add the root node in the real thing



    }


    //Create edges for the nodes that are connected 
    void ConnectNodes()
    {
        foreach(KeyValuePair<string, string> p in knowledge.taxonomy.children)
        {
            
            CreateEdge(nodes[p.Value], nodes[knowledge.taxonomy.parents[p.Key]]);
        }
            






    }


    //Create the edge
    void CreateEdge(Node start, Node end)
    {
        if(!edges.ContainsKey(start.name + end.name) | edges.ContainsKey(end.name + start.name))
        {
            GameObject edgeObj = Instantiate(edgePrefab);
            LineRenderer lr = edgeObj.GetComponent<LineRenderer>();
            Edge edge = edgeObj.GetComponent<Edge>();
            edge.node1 = start;
            edge.node2 = end;
            lr.SetPosition(0, start.gameObject.transform.position);
            lr.SetPosition(1, end.gameObject.transform.position);
            edges[start.name + end.name] = edge;
        }
            
    }



    //repulse all nodes 
    private Vector3 CalculateRepulsiveForce(Node node, Node repulsiveNode)
    {
        // Compute distance
        float distance = Vector3.Distance(node.transform.position, repulsiveNode.transform.position);
        if (distance > repulsionDistance)
            return Vector3.zero;

        // Compute force direction
        Vector3 forceDirection = (node.transform.position - repulsiveNode.transform.position).normalized;

        // Compute distance force
        float distanceForce = (repulsionStrength - distance) / repulsionDistance;

        // Compute repulsive force
        return forceDirection * distanceForce * repulsionStrength * Time.deltaTime;
    }

    //attract attached nodes 
    private Vector3 ComputeAttractiveForce(Edge edge)
    {
        // Compute force direction
        Vector3 forceDirection = (edge.node1.transform.position - edge.node2.transform.position).normalized;

        // Compute attractive force
        return -forceDirection * attractionStrength * Time.deltaTime;
    }
    

    

}
