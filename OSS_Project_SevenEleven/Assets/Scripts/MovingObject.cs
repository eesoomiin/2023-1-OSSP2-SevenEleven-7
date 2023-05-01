using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{

    // Variables

    //Static
    static public MovingObject instance; //static으로 선언된 변수의 값을 공유

    // Public
    public float speed; // 캐릭터 이동속력 변수
    public float runSpeed; // 달리기 속력

    public int walkCount;

    public LayerMask layerMask; // 통과 불가 레이어 설정

    public string currentMapName;

    public string walkSound_1; // 이름으로 접근해서 사운드 이용
    public string walkSound_2;
    public string walkSound_3;
    public string walkSound_4;

    //TransferMap에 있는 transferMapName을 저장할 변수


   
    // Private
    private Vector3 vector; // x,y,z의 값을 동시에 갖는 Vector 변수
    private float applyRunSpeed; // 실제 적용 RunSpeed

    private int currentWalkCount; //현재 walkCount loop를 빠져나가기 위한 변수

    private bool canMove = true; //코루틴 다중 실행 방지

    private bool applyRunFlag = false;

    private  Animator animator; // 애니메이션 관리를 위한 변수

    private BoxCollider2D boxCollider;

    private AudioManager theAudio;

    // Start is called before the first frame update
    void Start()
    {

        if (instance == null) // 처음 생성된 경우
        {
            //Scene 전환시 객체 파괴 방지 코드
            DontDestroyOnLoad(this.gameObject);
            animator = GetComponent<Animator>(); // 컴포넌트를 animator 변수에 불러옴
            boxCollider = GetComponent<BoxCollider2D>();
            theAudio = FindObjectOfType<AudioManager>();
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }

    }

    IEnumerator MoveCoroutine() // 대기시간을 만들어줄 Coroutine
    {
        while(Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) // 단일 코루틴 속 이동을 계속 가능하게 함
        {
            //Runs
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }

            //Vector Set
            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z); //벡터값 설정

            if (vector.x != 0) vector.y = 0; //좌우로 이동한다면 y벡터를 0으로 만들어 이동에 이상함이 없도록 함

            //Animation
            animator.SetFloat("DirX", vector.x); // x벡터 값을 전달해서 animation을 실행시킴
            animator.SetFloat("DirY", vector.y); // y벡터 값을 전달해서 animation을 실행시킴

            //RayCast : A -> B로 레이저를 쏴 아무것도 맞지 않는다면 hit == Null; else hit = 방해물
            RaycastHit2D hit;

            Vector2 start = transform.position; // 현재 캐릭터 위치 값
            Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount); //이동하려고 하는 위치 값

            boxCollider.enabled = false; //hit 값에 해당 오브젝트가 들어가지 않도록 함
            hit = Physics2D.Linecast(start, end, layerMask);
            boxCollider.enabled = true;

            if (hit.transform != null) break; //충돌되는 물체가 있다면 break


            animator.SetBool("Walking", true); // Bool 값을 전달해서 animation 전이

            //AUDIO
            int temp = Random.Range(1, 4);
            switch (temp)
            {
                case 1:
                    theAudio.Play(walkSound_1);
                    break;

                case 2:
                    theAudio.Play(walkSound_2);
                    break;

                case 3:
                    theAudio.Play(walkSound_3);
                    break;

                case 4:
                    theAudio.Play(walkSound_4);
                    break;
            }

            //Add Value
            while (currentWalkCount < walkCount)
            {
                if (vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0); //Translate 현재 위치 값에 ()안의 값을 더해줌, 즉 speed의 값만큼 더해줌
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }

                if (applyRunFlag) currentWalkCount++; // Run Flag가 잡혔을 때 cuurentWalkCount를 두배씩 증가시킴

                currentWalkCount++;

                yield return new WaitForSeconds(0.01f); // () 안만큼 대기
                                                        //speed = 2.4, walkcount = 20 => 2.4 * 20 = 48 
                                                        //방향키 한번마다 48픽셀씩 움직이도록 함

                //if currentWalkCount가 20이 되면 반복문을 빠져나감 

            }
            currentWalkCount = 0; // 0으로 초기화
        }
        animator.SetBool("Walking", false);
        canMove = true; //방향키 입력이 가능하도록 함
    } // 다중 처리 기능 함수


    // Update is called once per frame
    void Update()
    {

        if (canMove) //코루틴 다중 실행 방지 분기문
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) //방향키에 따라 -1 또는 1 리턴
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }
    }
}
