using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : INode
{
    private float _waitTime;
    private float _elapsedTime;
    private bool _isWaiting;

    private INode.NodeState _state;

    public WaitNode(float waitTime)
    {
        _waitTime = waitTime;
        _elapsedTime = 0f;
        _isWaiting = false;
        _state = INode.NodeState.Running;
    }

    public INode.NodeState Eval()
    {
        if (_state == INode.NodeState.Success)
            return _state;

        if (!_isWaiting)
        {
            _isWaiting = true;
            _elapsedTime = 0f;
        }

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > _waitTime)
        {
            _state = INode.NodeState.Success;
            _isWaiting = false;
        }

        return _state;
    }
}
