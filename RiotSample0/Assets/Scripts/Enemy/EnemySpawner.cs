using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : EnemySpawnControl
{
    //맴버 변수
    private float HP=100;//체력
    private bool isDestroy=false;//스포너가 파괴됬는가
    private GameObject EnemyObj;//스폰한 오브젝트
    private int EnemyID;
    private Animator animator;

    private int[] EnemyCount=new int[5];//id//count


    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();//애니메이터연결
        EnemySet();//배치할 객체수 불러오기
        StartCoroutine(Deploy());//배치 실행
    }
    

    private IEnumerator Deploy()
    {
        while(!isDestroy)
        {
            HPCheck();
            if(RandomDeployChance())
            {//배치가능 확인
                EnemyID=RandomDeployEnemySet();
                if (EnemyID != 0)
                {//id 확인
                    if (EnemyCount[EnemyID] > 0)
                    {//소환가능 객체수 확인
                        //소환하니 적 객체 수 감소
                        EnemyCount[EnemyID] = EnemyCount[EnemyID] - 1;
                        //소환
                        EnemyObj = Instantiate
                          (Resources.Load<GameObject>((EnemyID+100) + "GameObj"), RandomPos(), Quaternion.identity) as GameObject;
                        yield return new WaitForSeconds(DeployTime);
                    }
                    else
                    {
                        Debug.Log(EnemyCount[EnemyID]+"소환 가능 횟수");
                    }
                }
            }
            else
            {//대기
                yield return new WaitForSeconds(DeployTime);
            }
        }
        yield return null;
    }


    protected void EnemySet()//스포너 맴버  변수 셋팅
    {
        Stage = PlayerPrefs.GetInt("Phase");
        switch (Stage)
        {
            case 0:
                EnemyCount[1] = 4;
                EnemyCount[2] = 4;
                EnemyCount[3] = 4;
                EnemyCount[4] = 0;
                DeployTime = 3f;
                DeployChance = 2f;
                break;
            case 1:
                EnemyCount[1] = 4;
                EnemyCount[2] = 4;
                EnemyCount[3] = 4;
                EnemyCount[4] = 2;
                DeployTime = 3f;
                DeployChance = 2.5f;
                break;
            case 2:
                EnemyCount[1] = 4;
                EnemyCount[2] = 4;
                EnemyCount[3] = 4;
                EnemyCount[4] = 2;
                DeployTime = 3f;
                DeployChance = 1.0f;
                break;
        }
    }

    #region 확률계산
    protected bool RandomDeployChance()//배치 가능한지 테스트
    {
        float radomFloat = UnityEngine.Random.Range(0f, 3f);
        if (DeployChance >= radomFloat)
        {// 확률보다 작은 경우
            return true;
        }
        else
        {// 확률보다 큰 경우
            return false;
        }
    }
    protected int RandomDeployEnemySet()//스폰 유닛 고르기
    {
        int randomInt = UnityEngine.Random.Range(1,5);
        if (EnemyCount[randomInt] <= 0)
        {//없음
            Debug.Log(randomInt + " " + EnemyCount[randomInt]);
            return 0;
        }
        else
        {//있음
            Debug.Log(randomInt + " " + EnemyCount[randomInt]);
            return randomInt;
        }
    }
    protected int RandomNum()//0부터 10까지 랜덤 수 나옴 
    {
        int randomNum = UnityEngine.Random.Range(0, 10);
        return randomNum;
    }
    protected Vector3 RandomPos()//생성위치 랜덤 지정
    {
        return new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.transform.position.z+plusminus * RandomNum() * 0.2f);
    }


    #endregion


    #region Hit
    public void Hit(int EnemyAP)
    {//체력을 깍고 체력을 확인
        HP -= EnemyAP;
        HPCheck();
    }
    private void HPCheck()
    {
        animator.SetFloat("HP",HP);
        if (HP <= 0)
        {
            GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameCombatManager>();
            GM.Win();
            Destroy(this.gameObject);
        }
    }
    #endregion


}
