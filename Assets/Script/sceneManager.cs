using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    public void OnClickStart()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.button);  
        AudioManager.instance.PlayBGM(0);

        SceneManager.LoadScene("playing2");
    }

    public void OnClickmain()
    {
         
         SceneManager.LoadScene("Title");
    }

    public void OnClickGameOut()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.button);  
        Application.Quit();
    }
}
