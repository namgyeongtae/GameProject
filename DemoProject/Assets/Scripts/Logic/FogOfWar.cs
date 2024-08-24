using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public SpriteRenderer fogRenderer; // ���� �Ȱ� ��������Ʈ ������
    public Transform player; // �÷��̾��� Ʈ������
    public float radius = 5f; // �þ� �ݰ�
    public Material fogMaterial; // �Ȱ� ȿ���� ���� ��Ƽ����

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        fogMaterial = fogRenderer.material;
        fogMaterial.SetFloat("_Radius", radius);
    }

    private void Update()
    {
        // ���� ��ǥ�迡���� �÷��̾� ��ġ�� �ؽ��� ������ UV ������ ��ȯ
        Vector3 leftBottom = transform.position + (Vector3.left * (transform.localScale.x / 2))
                                             + (Vector3.down * (transform.localScale.y / 2));

        float ratioX = (player.position.x - leftBottom.x) / transform.localScale.x;
        float ratioY = (player.position.y - leftBottom.y) / transform.localScale.y;

        Vector2 UV = new Vector2(ratioX, ratioY);
        fogMaterial.SetVector("_PlayerPosition", new Vector4(UV.x, UV.y, 0, 0));
    }
}
