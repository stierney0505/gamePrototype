using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum Status { SUCCESS, RUNNING, FAILURE };
    public Status status;
    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public string name;

    public Node()
    {

    }
    public Node(string n)
    {
        name = n;

    }

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }

    public void addChild(Node n) { children.Add(n); }

    public void printTree()
    {
        if (children.Count == 0)
            Debug.Log(name);
        else
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].printTree();
            }
            Debug.Log(name);
        }
    }
}
