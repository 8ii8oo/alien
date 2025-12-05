using UnityEngine;
using System.Collections;

public class player : MonoBehaviour
{
    public float LeftLimit = -6.5f;
    public float RightLimit = 6.5f;
    public float jumpPower = 15f;
    public float superJumpPower = 30f;

    [Header("점프 이미지")]
    public Sprite jumpSprite;

    private Sprite originalSprite;

    [HideInInspector]
    public bool isGround = true;

    public Rigidbody2D rigid;
    SpriteRenderer sr;

    [HideInInspector]
    public float currentMaxScoreY;
    public float currentJumpStartOrMaxY;
    

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalSprite = sr.sprite;
        currentMaxScoreY = transform.position.y; 
        currentJumpStartOrMaxY = transform.position.y;
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float halfWidth = transform.localScale.x / 2f;
        float leftBorder = LeftLimit + halfWidth;
        float rightBorder = RightLimit - halfWidth;

        float targetX = Mathf.Clamp(mousePos.x, leftBorder, rightBorder);
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, 0);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.2f);

        if (rigid.linearVelocity.y > 0 && transform.position.y > currentMaxScoreY)
    currentMaxScoreY = transform.position.y;

if (rigid.linearVelocity.y > 0 && transform.position.y > currentJumpStartOrMaxY)
    currentJumpStartOrMaxY = transform.position.y;

if (rigid.linearVelocity.y > 0)
    sr.sprite = originalSprite;
else
    sr.sprite = jumpSprite;
    }

    public void Jump()
    {
        if (!isGround) return;

        sr.sprite = jumpSprite;
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        isGround = false;
        currentJumpStartOrMaxY = transform.position.y;

        AudioManager.instance.PLaySfx(AudioManager.Sfx.platform);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("platform"))
        {
            if(collision.contacts[0].normal.y > 0.5f)
            {
            isGround = true;
            sr.sprite = originalSprite;
            StartCoroutine(AutoJump());
            }
            else
            {
                return;
            }
        }
        else if (collision.gameObject.CompareTag("booster"))
        {
            if(collision.contacts[0].normal.y > 0.5f)
            {
            AudioManager.instance.PLaySfx(AudioManager.Sfx.platform);

            isGround = true;
            sr.sprite = originalSprite;

            sr.sprite = jumpSprite;
            rigid.AddForce(Vector2.up * superJumpPower, ForceMode2D.Impulse);
            isGround = false;

            currentJumpStartOrMaxY = transform.position.y;
            
            }
        }
    }

    IEnumerator AutoJump()
    {
        yield return new WaitForSeconds(0.01f);
        Jump();
    }

    // 외부에서 스테이지 업그레이드 정보를 적용할 때 사용
    public void ApplyStageUpgrade(float newJumpPower, float newSuperJumpPower, Sprite newSprite)
    {
        jumpPower = newJumpPower;
        superJumpPower = newSuperJumpPower;
        if (newSprite != null)
        {
            originalSprite = newSprite;
            if (sr != null) sr.sprite = originalSprite;
        }
    }
}
