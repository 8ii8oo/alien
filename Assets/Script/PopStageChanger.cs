using UnityEngine;

public class PopStageChanger : MonoBehaviour
{
    private SpriteRenderer sr;

    public Sprite defaultSprite;   // 평상시 이미지
    public Sprite popSprite;       // 터지는 이미지

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null && defaultSprite != null)
            sr.sprite = defaultSprite;
    }

    // change.cs에서 스테이지 전환 시 호출됨
    public void ApplyStagePopSprites(Sprite newDefault, Sprite newPop)
    {
        if (newDefault != null)
            defaultSprite = newDefault;

        if (newPop != null)
            popSprite = newPop;

        // 기본 스프라이트 갱신
        if (sr != null)
            sr.sprite = defaultSprite;
    }

    // Pop.cs의 원래 기능 그대로 유지
    public void TriggerPop()
    {
        if (sr != null && popSprite != null)
            sr.sprite = popSprite;

        Destroy(gameObject, 0.3f);
    }
}
