using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTRunner
{
    INode _rootNode;

    public BTRunner(INode rootNode)
    {
        _rootNode = rootNode;
    }

    public void Execute()
    {
        _rootNode.Eval();
    }
}
