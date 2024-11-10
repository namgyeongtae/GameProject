using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLookAtCamera : MonoBehaviour
{
    public Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, -10);
    }
}
