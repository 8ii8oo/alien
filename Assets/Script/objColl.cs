// objColl.cs 스크립트 전체 수정본

using UnityEngine;

public class objColl : MonoBehaviour
{
    public player PlayerScript; 

    private Collider2D platformCollider;

    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
        
        if (platformCollider == null)
        {
            Debug.LogError("PlatformCollider is missing on this GameObject!");
            enabled = false;
            return;
        }
        
        // 처음에는 콜라이더를 켜서 착지할 수 있도록 합니다.
        platformCollider.enabled = true; 
    }

    void Update()
    {
        // 플레이어 스크립트와 Rigidbody가 없으면 중단
        if (PlayerScript == null || PlayerScript.rigid == null) return;
        
        // 🔥 변경된 핵심 로직: Y축 속도를 확인합니다.
        float playerVelocityY = PlayerScript.rigid.linearVelocity.y;
        
        // 플레이어가 하강 중일 때 (속도가 0보다 작을 때)
        // 이 조건이 충족되어야 콜라이더가 켜지고 착지할 수 있습니다.
        if (playerVelocityY < 0)
        {
            // 콜라이더 켜기 (착지를 준비)
            if (!platformCollider.enabled)
            {
                platformCollider.enabled = true;
            }
        }
        // 플레이어가 상승 중이거나 멈춰있을 때 (속도가 0 이상일 때)
        else
        {
            // 콜라이더 끄기 (위로 통과)
            if (platformCollider.enabled)
            {
                platformCollider.enabled = false;
            }
        }
    }
}