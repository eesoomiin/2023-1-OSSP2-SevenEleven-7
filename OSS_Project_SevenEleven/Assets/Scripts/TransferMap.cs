using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Scene 관리를 위한 라이브러리

public class TransferMap : MonoBehaviour
{

    //Build Settings에서 사용하기 전에 Scene Load 할 것!!


    //Variables

    //Public 
    public Transform target;

    public BoxCollider2D targetBound;

    //Private
    private MovingObject thePlayer;
    private CameraManager theCamera;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<MovingObject>();
        theCamera = FindObjectOfType<CameraManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            theCamera.SetBound(targetBound);
            theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
            thePlayer.transform.position = target.transform.position;
        }
    }
}
