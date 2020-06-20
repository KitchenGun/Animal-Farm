using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artillery : MonoBehaviour
{
    private GameCombatManager gameCombatManager;
    private Image artyGage;
    private float pressGage = 0f;
    // Start is called before the first frame update
    void Start()
    {
        gameCombatManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameCombatManager>();
        artyGage =this.gameObject.GetComponent<Image>();
        artyGage.fillAmount = 0;
    }
    private void Update()
    {
        pressGage = gameCombatManager.pressSpaceBar * 0.01f;
        artyGage.fillAmount = pressGage;//게이지 표시
       
    }

}
