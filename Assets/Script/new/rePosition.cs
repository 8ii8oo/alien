using System;
using UnityEngine;

public class rePosition : MonoBehaviour
{
    public float Ysize;
    void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag("Area"))
        return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        
        // 1. playerDir을 먼저 선언하고 값을 가져옵니다.
        // 참고: player 스크립트의 rigid 변수는 반드시 public으로 선언되어야 합니다.
        Vector3 playerDir = GameManager.instance.player.rigid.linearVelocity;
        
        // 2. 필요한 변수들을 계산합니다.
        float dirY = playerDir.y < 0 ? -1 : 1; // 이동 방향 부호
        float absY = Mathf.Abs(playerDir.y);   // Y 속도의 절대 크기
        float absX = Mathf.Abs(playerDir.x);   // X 속도의 절대 크기
        
        // (dirX는 사용되지 않으므로 삭제)

        switch(transform.tag)
        {
            case "Ground" :
                // Y축 속도의 절댓값이 X축 속도의 절댓값보다 클 때 (수직 이동 중일 때)
                if(absY > absX) 
                {
                    // Y 방향 부호(dirY)에 따라 위(1) 또는 아래(-1)로 40만큼 이동
                    transform.Translate(Vector3.up * dirY * Ysize);
                }
                break;
                case "GroundNew" :
                // Y축 속도의 절댓값이 X축 속도의 절댓값보다 클 때 (수직 이동 중일 때)
                if(absY > absX) 
                {
                    // Y 방향 부호(dirY)에 따라 위(1) 또는 아래(-1)로 40만큼 이동
                    transform.Translate(Vector3.up * dirY * 26f);
                }
                break;
        }
    }
}