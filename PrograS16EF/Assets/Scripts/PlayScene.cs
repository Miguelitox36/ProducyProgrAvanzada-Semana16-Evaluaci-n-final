using GameJolt.API;
using GameJolt.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PlayScene : MonoBehaviour
{
    [SerializeField] private Button trophiesButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button rankingButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        trophiesButton.onClick.AddListener(() =>
        {
            GameJoltUI.Instance.ShowTrophies();
        });

        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("PlayScene");
        });

        rankingButton.onClick.AddListener(() =>
        {
            GameJoltUI.Instance.ShowLeaderboards();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
