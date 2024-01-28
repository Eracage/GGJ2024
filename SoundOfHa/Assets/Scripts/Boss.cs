using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    enum BossState
    {
        Cry = 0,
        Indifferent = 1,
        Flustered = 2,
        Happy = 3
    }
    bool shouldSpawnArmy = true;
    public GameObject[] enemytypes;

    public GameObject bells;

    public GameObject meteor;
    public Transform armySpawnPoint;
    float meteorfirerate = 7f;
    float lastShootTime = 0f;


    public GameObject[] destroyers;
    List<GameObject> army = new List<GameObject>();

    BossState bossState = BossState.Cry;
    BossState lastState = BossState.Cry;

    public GameObject finishLine;
    public Transform player;
    SpriteRenderer spriteRenderer;

    public SpriteRenderer[] RedEyeRenederers;
    public SpriteRenderer[] GreenEyeRenederers;
    public SpriteRenderer[] TealEyeRenederers;

    public Sprite[] RedEyes;
    public Sprite[] GreenEyes;
    public Sprite[] TealEyes;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()
    {
        switch(bossState)
        {
            case BossState.Cry:
                BossCry();
                break;
            case BossState.Indifferent:
                BossIndifferent();
                break;
            case BossState.Flustered:
                BossFlustered();
                break;
            case BossState.Happy:
                BossHappy();
                break;
        }
    }

    void BossCry()
    {
        if(shouldSpawnArmy)
        {
            SpawnArmy(2,10);
            shouldSpawnArmy = false;
        }
        for(int i = 0; i<army.Count; i++)
        {
            if(army[i] == null)
            {
                army.RemoveAt(i);
            }
        }
        if(army.Count == 0)
        {
            bossState = BossState.Indifferent;
            shouldSpawnArmy = true;
        }

    }

    void BossIndifferent()
    {
        if(lastState != BossState.Indifferent)
        {
            Instantiate(bells);
            foreach(SpriteRenderer sr in RedEyeRenederers)
            {
                sr.sprite = RedEyes[1];
            }
            foreach(SpriteRenderer sr in GreenEyeRenederers)
            {
                sr.sprite = GreenEyes[1];
            }
            foreach(SpriteRenderer sr in TealEyeRenederers)
            {
                sr.sprite = TealEyes[1];
            }
            lastState = BossState.Indifferent;
            lastShootTime = Time.time;
        }

        if(shouldSpawnArmy)
        {
            SpawnArmy(3,30);
            shouldSpawnArmy = false;
        }
        for(int i = 0; i<army.Count; i++)
        {
            if(army[i] == null)
            {
                army.RemoveAt(i);
            }
        }
        if(army.Count == 0)
        {
            bossState = BossState.Flustered;
            shouldSpawnArmy = true;
        }

        if(Time.time - lastShootTime > meteorfirerate)
        {
            lastShootTime = Time.time;
            Instantiate(meteor, new Vector3(player.position.x,200,player.position.z), Quaternion.identity);
        }

    }

    void BossFlustered()
    {
        if(lastState != BossState.Flustered)
        {
            foreach(SpriteRenderer sr in RedEyeRenederers)
            {
                sr.sprite = RedEyes[2];
            }
            foreach(SpriteRenderer sr in GreenEyeRenederers)
            {
                sr.sprite = GreenEyes[2];
            }
            foreach(SpriteRenderer sr in TealEyeRenederers)
            {
                sr.sprite = TealEyes[2];
            }
            lastState = BossState.Flustered;
            lastShootTime = Time.time;
            meteorfirerate = 5f;
            
            foreach(GameObject destroyer in destroyers)
            {
                destroyer.SetActive(true);
            }
        }

        
        if(shouldSpawnArmy)
        {
            SpawnArmy(4,50);
            shouldSpawnArmy = false;
        }
        for(int i = 0; i<army.Count; i++)
        {
            if(army[i] == null)
            {
                army.RemoveAt(i);
            }
        }
        if(army.Count == 0)
        {
            bossState = BossState.Flustered;
            shouldSpawnArmy = true;
        }

        if(Time.time - lastShootTime > meteorfirerate)
        {
            lastShootTime = Time.time;
            Instantiate(meteor, new Vector3(player.position.x,200,player.position.z), Quaternion.identity);
        }
    }

    void BossHappy()
    {
        if(lastState != BossState.Happy)
        {
            foreach(SpriteRenderer sr in RedEyeRenederers)
            {
                sr.sprite = RedEyes[3];
            }
            foreach(SpriteRenderer sr in GreenEyeRenederers)
            {
                sr.sprite = GreenEyes[3];
            }
            foreach(SpriteRenderer sr in TealEyeRenederers)
            {
                sr.sprite = TealEyes[3];
            }
            lastState = BossState.Happy;
            finishLine.SetActive(true);
        }
    }


    void SpawnArmy(int rowcount, int columncount)
    {
        for(int rows = 0; rows < rowcount; rows++)
        {
            for(int columns = 0; columns < columncount; columns++)
            {
                GameObject gm = Instantiate(enemytypes[Random.Range(0, enemytypes.Length)], armySpawnPoint.position + new Vector3((columns+1)*3, 0, (rows+1)*3), Quaternion.identity);
                army.Add(gm);
                gm.GetComponent<Enemy>().Aggro();
            }
        }
    }
}