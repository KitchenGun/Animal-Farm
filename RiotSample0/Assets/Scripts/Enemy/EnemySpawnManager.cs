using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class EnemySpawnControl : MonoBehaviour
{

    protected int Stage;//스테이지 확인용
    protected GameCombatManager GM;//전투 매니져

    protected float DeployChance;//배치를 할 확률
    protected float DeployTime;//배치할 시간

    //랜덤위치용 변수 받아오기
    protected int randomPosValue;
    protected int plusminus;

   
    
}