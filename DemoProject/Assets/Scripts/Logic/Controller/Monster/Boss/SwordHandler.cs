using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordHandler : MonoBehaviour
{
    [SerializeField] private List<BossSword> _swordGroup = new List<BossSword>();

    private float _throwTime = 0f;
    private float _throwCycleTime;

    private GameObject _enemyTarget;

    // Start is called before the first frame update
    void Start()
    {
        _throwCycleTime = Random.Range(5f, 10f);

        _enemyTarget = GameObject.FindGameObjectWithTag("Player");

        RotateSwordsRecursively();
    }

    // Update is called once per frame
    void Update()
    {
        _throwTime += Time.deltaTime;

        if (_throwTime >= _throwCycleTime)
        {
            ThrowSword(_enemyTarget);

            _throwCycleTime = Random.Range(5f, 10f);
            _throwTime = 0f;
        }
    }

    private void RotateSwordsRecursively()
    {
        if (_swordGroup.Count == 0)
            return;

        transform.DOLocalRotate(new Vector3(0, 0, 360), 1.0f, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .OnComplete(() =>
                 {
                     transform.localRotation = Quaternion.Euler(Vector3.zero);
                     RotateSwordsRecursively();
                 });
    }

    private void ThrowSword(GameObject target)
    {
        int ranNum = Random.Range(0, _swordGroup.Count - 1);

        BossSword selectedSword = _swordGroup[ranNum];
        
        _swordGroup.Remove(selectedSword);

        selectedSword.transform.SetParent(null);

        selectedSword.AttackDash(target);

        if (_swordGroup.Count <= 0)
            Managers.Resource.Destroy(this.gameObject);
    }
}
