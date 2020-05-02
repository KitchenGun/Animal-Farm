using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAirStrike : MonoBehaviour
{

    private GameObject dropObj;//떨구는 오브젝트
    private int bombCount;//떨군 오브젝트 개수
    public bool BirdFly;//현재 이 스크립트가 실행중인가
    private void Start()
    {
        BirdFly = true;//현재 새가 스폰했는가
        dropObj = Resources.Load<GameObject>("Egg");//달걀을 떨구는 오브젝트로 설정
        StartCoroutine(BombsAway());
    }

    private IEnumerator BombsAway()
    {
        //공습지역 도착 대기 시간
        yield return new WaitForSeconds(3.0f);
        //투하
        do
        {
            //떨군 폭탄수 증가
            bombCount++;
            //dropobj 스폰
            dropObj = Instantiate
                (dropObj, this.transform.position , Quaternion.identity) as GameObject;
            dropObj.GetComponent<BoxCollider>();
            yield return new WaitForSeconds(1.0f);
        } while (bombCount<5);//떨굴 횟수
        //투하 종료후 코드
        BirdFly = false;
        Destroy(this.gameObject);
    }
}
