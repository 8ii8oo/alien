using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public player player;

    public TMP_Text scoreText;
    

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
       
    }

    void Update()
    {
        scoreText.text = "SCORE \n" + Mathf.FloorToInt(CalculateScore());

    
    }

    float CalculateScore()
    {
        if(player != null)
        {
            return Mathf.Max(0f, player.currentMaxScoreY) * 10f;
        }
        return 0f;
    }


}
