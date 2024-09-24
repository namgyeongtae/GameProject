using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SequenceNode : INode
{
    private string _name;

    private List<INode> _childs;

    private Func<bool> _condition;

    public SequenceNode(string name, List<INode> childs, Func<bool> condition = null)
    {
        _name = name;
        _childs = childs;
        _condition = condition;
    }

    public INode.NodeState Eval()
    {
        if (_condition != null)
        {
            bool result = _condition.Invoke();
            if (!result)
                return INode.NodeState.Failure;
        }

        if (_childs == null || _childs.Count == 0)
            return INode.NodeState.Failure;

        foreach (var child in _childs)
        {
            switch (child.Eval())
            {
                case INode.NodeState.Running:
                    return INode.NodeState.Running;
                case INode.NodeState.Success:
                    continue;
                case INode.NodeState.Failure:
                    return INode.NodeState.Failure;
            }
        }

        return INode.NodeState.Success;
    }
}
