using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SelectorNode : INode
{
    List<INode> _childs;

    public SelectorNode(List<INode> childs)
    {
        _childs = childs;
    }

    public INode.NodeState Eval()
    {
        if (_childs == null)
            return INode.NodeState.Failure;

        foreach (var node in _childs)
        {
            switch(node.Eval())
            {
                case INode.NodeState.Running:
                    return INode.NodeState.Running;
                case INode.NodeState.Success: 
                    return INode.NodeState.Success;
            }
        }

        return INode.NodeState.Failure;
    }
}
