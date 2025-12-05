using UnityEngine;

public class Pop : MonoBehaviour
{
    public Sprite PopSprite;
    SpriteRenderer sr;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            
            sr.sprite = PopSprite;

            Destroy(gameObject, 0.3f);

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PLaySfx(AudioManager.Sfx.platformEnemy);
            Destroy(gameObject);

        }

    }
}
