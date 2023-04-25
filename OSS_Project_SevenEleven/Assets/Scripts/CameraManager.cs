using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //Variables

    //Static
    static public CameraManager instance; // 카메라 오브젝트 인스턴스 

    //Public
    public GameObject target; // 카메라가 따라갈 대상 
    public float moveSpeed; // 카메라 이동 속도

    public BoxCollider2D bound; // 맵 크기 만큼의 Bound

    //Private
    private Vector3 targetPosition; // 대상의 현재 위치 값

    private Vector3 minBound;
    private Vector3 maxBound; // BoxColider 영역의 최소 최대 좌푯값을 가질 변수

    private float halfWidth;
    private float halfHeight; // 카메라의 반너비, 반높이 값을 가질 변수

    private Camera theCamera; // 카메라의 좌표값을 가질 변수


    private void Awake() //Start보다 먼저 실행
    {
        //Scene 전환시 객체 파괴 방지 코드
        //전활할 씬의 카메라 오브젝트는 삭제 할 것!

        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialization
        theCamera = GetComponent<Camera>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize; // 카메라의 반높이
        halfWidth = halfHeight * Screen.width / Screen.height; // 반너비 구하는 공식
    }

    // Update is called once per frame
    void Update()
    {
        if (target.gameObject != null) // 대상이 있어야 카메라 이동
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);  // 1초에 moveSpeed 만큼 움직임   
            //Lerp 중간값 리턴 , deltaTime : 1초에 실행되는 프레임의 역수 

            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth); 
            // value가 min 과 max 사이에 있다면 리턴, max 초과시 max, min 미만 시 min 리턴
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);

        }
    }

    public void SetBound(BoxCollider2D newBound) 
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }
}
