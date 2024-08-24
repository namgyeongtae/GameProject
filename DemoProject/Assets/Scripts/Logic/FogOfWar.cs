using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public SpriteRenderer fogRenderer; // 검은 안개 스프라이트 렌더러
    public Transform player; // 플레이어의 트랜스폼
    public float radius = 5f; // 시야 반경
    public Material fogMaterial; // 안개 효과를 위한 머티리얼

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        fogMaterial = fogRenderer.material;
        fogMaterial.SetFloat("_Radius", radius);
    }

    private void Update()
    {
        // 월드 좌표계에서의 플레이어 위치를 텍스쳐 기준의 UV 값으로 전환
        Vector3 leftBottom = transform.position + (Vector3.left * (transform.localScale.x / 2))
                                             + (Vector3.down * (transform.localScale.y / 2));

        float ratioX = (player.position.x - leftBottom.x) / transform.localScale.x;
        float ratioY = (player.position.y - leftBottom.y) / transform.localScale.y;

        Vector2 UV = new Vector2(ratioX, ratioY);
        fogMaterial.SetVector("_PlayerPosition", new Vector4(UV.x, UV.y, 0, 0));
    }
}
