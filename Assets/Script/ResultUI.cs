using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    public TMP_Text resultScoreText;

    void Start()
    {
        resultScoreText.text = "" + ScoreData.finalScore.ToString();
    }
}
