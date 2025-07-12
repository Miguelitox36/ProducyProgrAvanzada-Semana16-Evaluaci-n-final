using UnityEngine;
using GameJolt.UI;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameJoltUI.Instance.ShowSignIn((success) =>
        {
            if (success)
            {
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                Debug.Log("No se pudo logear.");
            }
        });
    }
    
}
