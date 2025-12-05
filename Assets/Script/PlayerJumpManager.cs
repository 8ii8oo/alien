using UnityEngine;
using System.Collections;

public class PlayerJumpManager : MonoBehaviour
{
    private player p;
    public bool isJumpPhase = false;
    public float jumpInterval = 0.35f;
    Coroutine jumpRoutine;

    void Start()
    {
        p = GetComponent<player>();
    }

    public void StartJumpPhase()
    {
        if (jumpRoutine != null) StopCoroutine(jumpRoutine);
        jumpRoutine = StartCoroutine(JumpLoop());
        isJumpPhase = true;
    }

    public void StopJumpPhase()
    {
        if (jumpRoutine != null) StopCoroutine(jumpRoutine);
        isJumpPhase = false;
    }

    IEnumerator JumpLoop()
    {
        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            if (p.isGround)
                p.Jump();

            yield return new WaitForSeconds(jumpInterval);
        }
    }
}
