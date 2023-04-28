using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Scene 관리를 위한 라이브러리

public class TransferScene : MonoBehaviour
{

    //Build Settings에서 사용하기 전에 Scene Load 할 것!!


    //Variables

    //Public 
    public string transferMapName;  //이동할 맵의 이름

    //Private
    private MovingObject thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<MovingObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            thePlayer.currentMapName = transferMapName; // 만약 이동 영역과 부딪힌다면 이동할 맵의 이름을 Player오브젝트로 넘겨줌
            SceneManager.LoadScene(transferMapName); // transferMapName으로 이동
        }
    }
}
