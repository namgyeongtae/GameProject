using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private BaseEntity _targetEntity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectNearestTarget();

        if (Input.GetKeyUp(KeyCode.F))
            InteractWithEntity();
    }

    private void DetectNearestTarget()
    {
        BaseEntity resultEntity = null;

        var entities = Physics2D.OverlapCircleAll(transform.position, 1.4f, 1 << LayerMask.NameToLayer("Entity"));

        if (entities.Length <= 0)
        {
            _targetEntity?.OnTargetCancel_Event?.Invoke();
            _targetEntity = null;
            return;
        }

        float minDistance = float.MaxValue;

        foreach (var entity in entities)
        {
            if (Vector2.Distance(transform.position, entity.transform.position) < minDistance)
                resultEntity = entity.GetComponent<BaseEntity>();
        }

        _targetEntity = resultEntity;
        _targetEntity?.OnTarget_Event?.Invoke();
    }

    private void InteractWithEntity()
    {
        if (_targetEntity != null) 
            _targetEntity.Interaction();
    }

    public void SetTargetEntity(BaseEntity targetEntity)
    {
        _targetEntity = targetEntity;
    }
}
