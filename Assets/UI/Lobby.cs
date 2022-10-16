using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby: MonoBehaviour
{
    public RenderTexture playerRenderTexture;

    public Animator playerAnimator;

    public void StartGame()
    {
        SceneManager.LoadScene("PolyStadium");
    }
}
