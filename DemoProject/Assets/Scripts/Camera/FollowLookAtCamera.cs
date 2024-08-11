using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLookAtCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, -10);
    }
}
