using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerDamage : MonoBehaviour
{
    private const string bulletTag = "TmpBullet";
    private float initHp = 300.0f;   /* 잠시 수정 */
    private Color currColor;
    private readonly Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
    public float currHP;
    public bool hpitem = false;

    public Image bloodScreen;
    public Image hpBar;

    [Header("HPItem")]
    public GameObject HPItem;
    public float destroyTime = 10f;

    private bool isUpdate = false;

    private OneEneyAI oneEneyAI;
    private GameObject enemySpawn;
    private String EnemyTag = "Enemy";
    //public delegate void PlayerDieHandler();
    //public static event PlayerDieHandler OnPlayerDie;

    void Start()
    {   
        enemySpawn = GameObject.Find("SpawnScript");

        currHP = initHp;

        hpBar.color = initColor;
        currColor = initColor;

        isUpdate = true;
    }

    void Update()
    {   
        if(enemySpawn == null){
            enemySpawn = GameObject.Find("SpawnScript");
        }

            HpItem();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == bulletTag)
        {
            StartCoroutine(ShowBloodScreen());
            currHP -= 5.0f;
            DisPlayHpbar();

            if (currHP <= 0.0f) //플레이어 죽었을때
            {   
                Destroy(enemySpawn);
                PlayerDie();
                coll.enabled = false;
                FindObjectOfType<GameManager>().playerLose();
            }
        }
    }

    private void PlayerDie()
    {
        Debug.Log("Player Die!!!");

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);

        for(int i=0; i<enemies.Length; i++){
            enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }
    }

    IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1, 0, 0,UnityEngine.Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.color = Color.clear;
    }

    void DisPlayHpbar()
    {
        if ((currHP / initHp) > 0.5f)
            currColor.r = (1 - (currHP / initHp)) * 2.0f;
        else
            currColor.g = (currHP / initHp) * 2.0f;
        hpBar.color = currColor;
        hpBar.fillAmount = (currHP / initHp);
    }

    IEnumerator DestroyHpItem()
    {
        yield return new WaitForSeconds(destroyTime);
        HPItem.SetActive(false);
    }
    void HpItem()
    {
        if ((hpitem == false) && (currHP == 75))
        {
            HPItem.SetActive(true);
            hpitem = true;
            StartCoroutine(DestroyHpItem());
        }
    }
    public void Heal()
    {
        Debug.Log("Heal~~~~~~~~~~~~~~");
        currHP += 100f;
        AudioSource sound = GetComponent<AudioSource>();
        sound.Play();

    }
}
