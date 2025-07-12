using UnityEngine;
using GameJolt.API;

public class MainScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Trophies.Unlock(273735);
    }

    
}
