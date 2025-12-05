using UnityEngine;

public class StageSpriteReceiver : MonoBehaviour
{
    [Header("이 오브젝트가 Pop인지 여부")]
    public bool isPopObject = false;

    [Header("현재 적용된 스프라이트")]
    public Sprite currentSprite;           // 일반 오브젝트
    public Sprite currentDefaultSprite;    // Pop 기본
    public Sprite currentExplosionSprite;  // Pop 터짐

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("SpriteRenderer가 없습니다: " + gameObject.name);
        }
    }

    // ===============================
    //  일반 오브젝트 스프라이트 적용
    // ===============================
    public void ApplySprite(Sprite newSprite)
    {
        if (newSprite == null || isPopObject) return;

        currentSprite = newSprite;
        if (sr != null)
            sr.sprite = currentSprite;
    }


    // ==========================================
    //  Pop 오브젝트 스프라이트(기본/터짐) 적용
    // ==========================================
    public void ApplyPopSprites(Sprite defaultSprite, Sprite explosionSprite)
    {
        if (!isPopObject) return;

        if (defaultSprite != null)
            currentDefaultSprite = defaultSprite;

        if (explosionSprite != null)
            currentExplosionSprite = explosionSprite;

        if (sr != null)
            sr.sprite = currentDefaultSprite;  // 기본 스프라이트로 변경
    }


    // ===========================
    // Pop 터짐 이미지 강제 적용
    // ===========================
    public void SetExplosionSprite()
    {
        if (!isPopObject) return;

        if (sr != null && currentExplosionSprite != null)
            sr.sprite = currentExplosionSprite;
    }
}
