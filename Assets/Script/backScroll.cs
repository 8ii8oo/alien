using UnityEngine;
using System.Collections;

public class backScroll : MonoBehaviour
{
    public GameObject[] backgrounds;

    public float initialDuration = 40f;
    public float transitionDuration = 10f;

    private float screenHeight;
    private int currentBGIndex = 0;

    private CameraMove cam;
    private PlayerJumpManager playerJump;
    private TileGenerator tileGen;

    void Start()
    {
        cam = FindObjectOfType<CameraMove>();
        playerJump = FindObjectOfType<PlayerJumpManager>();
        tileGen = FindObjectOfType<TileGenerator>();

        screenHeight = Camera.main.orthographicSize * 2f;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].transform.position = Vector3.zero;
            backgrounds[i].SetActive(false);
        }

        backgrounds[currentBGIndex].SetActive(true);

        StartCoroutine(BackgroundSequence());
    }

    IEnumerator BackgroundSequence()
    {
        cam.ReleaseFixed();
        playerJump.StartJumpPhase();
        tileGen.StartGenerate();

        yield return new WaitForSeconds(initialDuration);

        for (int i = 0; i < backgrounds.Length - 1; i++)
        {
            int nextIndex = i + 1;

            backgrounds[nextIndex].transform.position = new Vector3(0, screenHeight, 0);
            backgrounds[nextIndex].SetActive(true);

            cam.SetFixed();
            playerJump.StopJumpPhase();
            tileGen.StopGenerate();

            yield return StartCoroutine(ScrollTransition(backgrounds[i].transform, backgrounds[nextIndex].transform));

            backgrounds[i].SetActive(false);
            currentBGIndex = nextIndex;

            yield return new WaitForSeconds(10f);

            cam.ReleaseFixed();
            playerJump.StartJumpPhase();
            tileGen.StartGenerate();
        }
    }

    IEnumerator ScrollTransition(Transform currentBG, Transform nextBG)
    {
        float start = Time.time;

        Vector3 fromA = currentBG.position;
        Vector3 fromB = nextBG.position;

        Vector3 toA = fromA + new Vector3(0, -screenHeight, 0);
        Vector3 toB = fromB + new Vector3(0, -screenHeight, 0);

        while (Time.time < start + transitionDuration)
        {
            float t = (Time.time - start) / transitionDuration;

            currentBG.position = Vector3.Lerp(fromA, toA, t);
            nextBG.position = Vector3.Lerp(fromB, toB, t);

            yield return null;
        }

        currentBG.position = toA;
        nextBG.position = toB;
    }
}
