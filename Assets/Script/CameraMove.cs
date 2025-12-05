using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float topLimit = 4f;
    public float rightLimit = 15f;
    public float leftLimit = 0f;
    public float bottomLimit = 0f;

    public float FollowSpeed = 5f;
    public float YOffset = 0f;

    public Transform target;

    private bool fixedMode = false;

    void LateUpdate()
    {
        if (target == null || fixedMode) return;

        float targetY = Mathf.Clamp(target.position.y + YOffset, bottomLimit, topLimit);
        float targetX = Mathf.Clamp(target.position.x, leftLimit, rightLimit);

        Vector3 targetPos = new Vector3(targetX, targetY, -10f);

        transform.position = Vector3.Lerp(transform.position, targetPos, FollowSpeed * Time.deltaTime);
    }

    public void SetFixed()
    {
        fixedMode = true;
    }

    public void ReleaseFixed()
    {
        fixedMode = false;
    }
}
