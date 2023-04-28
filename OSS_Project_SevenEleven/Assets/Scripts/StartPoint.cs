using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{

    //Variables 

    //Public
    public string startPoint; // 맵이 이동되면 플레이어가 시작할 위치

    //Private
    private MovingObject thePlayer;
    private CameraManager theCamera; //카메라 이동을 위한 변수

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<MovingObject>(); //하이라키에 있는 모든 MovingObject 리턴
        theCamera = FindObjectOfType<CameraManager>(); //하이라키에 있는 모든 CameraManager 리턴

        if (startPoint == thePlayer.currentMapName)
        {
            thePlayer.transform.position = this.transform.position; //플레이어 위치 설정
            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, theCamera.transform.position.z);   
        }


    }
}
