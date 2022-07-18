using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInput : MonoBehaviour
{
    ForceDirectedGraph graph; 
    RaycastHit hit; 
    Ray ray; 
    public Node selectedNode;
    Renderer selectedNodeRenderer;
    public Material selectedNodeMaterial;
    public Material normalEdgeMaterial;
    public Material normalNodeMaterial;
    public GameObject selectedObjectScreen;
    void Start()
    {
        graph = GetComponent<ForceDirectedGraph>();
    }
    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown (0)){ 
            
            ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            if ( Physics.Raycast (ray,out hit,100.0f)) {
                if(selectedNode != null)
                {
                    Deselect(selectedNode);
                    
                    selectedNode.isFrozen = false;
                    if(selectedNodeRenderer == null)
                    {
                        selectedNodeRenderer = selectedNode.GetComponent<Renderer>();
                    }
                    selectedNodeRenderer.material = normalNodeMaterial;
                }
                
                selectedNode = graph.nodes[hit.transform.name];
                Select(selectedNode);
                
                

            } else
            {
                if(selectedNode != null)
                {
                    Deselect(selectedNode);
                    selectedNode.isFrozen = false;
                    if(selectedNodeRenderer == null)
                    {
                        selectedNodeRenderer = selectedNode.GetComponent<Renderer>();
                    }
                    selectedNodeRenderer.material = normalNodeMaterial;
                    selectedNode = null;
                }
                
            }
        } 
    }

    void Deselect(Node node)
    {

        selectedObjectScreen.SetActive(false);
        DeselectNode(node);
        DeselectNeighbors();
    }
    void Select(Node currentNode)
    {
        selectedObjectScreen.SetActive(true);
        selectedObjectScreen.transform.Find("Title").GetComponent<TextMeshProUGUI>().text  = currentNode.name;
        SelectNode(currentNode);

        SelectNeighbors();
    }
    Renderer nodeRenderer;
    void SelectNode(Node node, bool freeze = false)
    {


        nodeRenderer = node.gameObject.GetComponent<Renderer>();
        nodeRenderer.material = selectedNodeMaterial;
        node.titleObject.color = selectedNodeMaterial.color;
        if(freeze)
        {
            node.isFrozen = true;
        }
    }
    Node neighbor;
    void SelectNeighbors()
    {
        foreach(KeyValuePair<string, Node> p in selectedNode.neighbors)
        {
            
            neighbor = p.Value;
            
            SelectNode(neighbor);

            SelectEdge(neighbor);

        }
    }
    Edge edge;
    LineRenderer lr;
    void SelectEdge(Node otherNode)
    {  
        
        edge = selectedNode.GetEdge(otherNode.name);
        
        if(edge != null)
        {
            lr = edge.GetComponent<LineRenderer>();
            lr.material = selectedNodeMaterial;
            lr.SetWidth(graph.selectedEdgeWidth, graph.selectedEdgeWidth);

        }
           
    }
    //on refactoring change all of this diffe4rent override nonsense. not good strucvure. 
    
    void DeselectNode(Node node)
    {
        nodeRenderer = node.gameObject.GetComponent<Renderer>();
        nodeRenderer.material = normalNodeMaterial;
        node.titleObject.color = normalNodeMaterial.color;
        node.isFrozen = false;
    }
    void DeselectEdge(Node otherNode)
    {
        edge = selectedNode.GetEdge(otherNode.name);
        if(edge != null)
        {
            lr = edge.GetComponent<LineRenderer>();
            lr.material = normalEdgeMaterial;
            lr.SetWidth(graph.normalEdgeWidth, graph.normalEdgeWidth );

        }
    }
    void DeselectNeighbors()
    {
        foreach(KeyValuePair<string, Node> p in selectedNode.neighbors)
        {
            neighbor = p.Value;
            DeselectNode(neighbor);
            DeselectEdge(neighbor);
        }
    }
}

