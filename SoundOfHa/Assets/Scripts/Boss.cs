using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public enum BossStateEnum
    {
        Cry = 0,
        Indifferent = 1,
        Flustered = 2,
        Happy = 3
    }
    public GameObject[] Enemytypes;
    public GameObject[] Destroyers;
    public GameObject Bells;
    public GameObject FinishLine;
    public GameObject Meteor;
    public List<Transform> ArmySpawnPoints;
    [HideInInspector]
    public BossStateEnum BossState { get; private set; } = BossStateEnum.Cry;

    public SpriteRenderer[] RedEyeRenederers;
    public SpriteRenderer[] GreenEyeRenederers;
    public SpriteRenderer[] TealEyeRenederers;

    public Sprite[] RedEyes;
    public Sprite[] GreenEyes;
    public Sprite[] TealEyes;

    private GameObject m_player;
    private Animator m_animator;
    private List<GameObject> m_army = new List<GameObject>();
    private float m_meteorfirerate = 7f;
    private float m_lastShootTime = 0f;
    private BossStateEnum m_previousState = BossStateEnum.Cry;

    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_animator = GetComponent<Animator>();

        StateBossCry();
        m_previousState = BossStateEnum.Cry;
    }

    void Update()
    {
        DoBossAttacks();

        if (!IsArmyAlive())
        {
            var nextState = (int)BossState + 1;
            if(nextState < 4)
            {
                BossState = (BossStateEnum)nextState;
            }
        }

        if (BossState == m_previousState)
            return;

        switch(BossState)
        {
            case BossStateEnum.Cry:
                break;

            case BossStateEnum.Indifferent:
                StateBossIndifferent();
                m_previousState = BossStateEnum.Indifferent;
                m_lastShootTime = Time.time;
                break;

            case BossStateEnum.Flustered:
                StateBossFlustered();
                m_meteorfirerate = 5f;
                m_lastShootTime = Time.time;
                m_previousState = BossStateEnum.Flustered;
                ActivateDestroyers();
                break;

            case BossStateEnum.Happy:
                StateBossHappy();
                m_previousState = BossStateEnum.Happy;
                break;
        }

        Instantiate(Bells);
    }

    private void DoBossAttacks()
    {
        switch (BossState)
        {
            case BossStateEnum.Cry:
                break;

            case BossStateEnum.Indifferent:
                if (Time.time - m_lastShootTime > m_meteorfirerate)
                {
                    FireMeteor();
                }
                break;

            case BossStateEnum.Flustered:
                if (Time.time - m_lastShootTime > m_meteorfirerate)
                {
                    FireMeteor();
                }
                break;

            case BossStateEnum.Happy:
                break;
        }
    }

    private void ActivateDestroyers()
    {
        foreach(GameObject destroyer in Destroyers)
        {
            destroyer.SetActive(true);
        }
    }

    private void FireMeteor()
    {
        m_lastShootTime = Time.time;
        Instantiate(Meteor, new Vector3(m_player.transform.position.x, 200, m_player.transform.position.z), Quaternion.identity);
    }

    void StateBossCry()
    {
        BossState = BossStateEnum.Cry;
        SpawnArmy(10);
    }

    void StateBossIndifferent()
    {
        BossState = BossStateEnum.Indifferent;

        if (m_animator)
            m_animator.SetTrigger("Stage2");
        
        foreach(SpriteRenderer sr in RedEyeRenederers)
            sr.sprite = RedEyes[1];

        foreach(SpriteRenderer sr in GreenEyeRenederers)
            sr.sprite = GreenEyes[1];

        foreach(SpriteRenderer sr in TealEyeRenederers)
            sr.sprite = TealEyes[1];

        SpawnArmy(30);
    }

    void StateBossFlustered()
    {
        BossState = BossStateEnum.Flustered;

        if (m_animator)
            m_animator.SetTrigger("Stage3");

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
            
        
        SpawnArmy(50);
    }

    void StateBossHappy()
    {
        BossState = BossStateEnum.Happy;

        if (m_animator)
            m_animator.SetTrigger("Stage4");

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
        m_previousState = BossStateEnum.Happy;
        FinishLine.SetActive(true);
    }

    public bool IsArmyAlive()
    {
        bool result = true;

        for (int i = 0; i < m_army.Count; i++)
        {
            if (m_army[i] == null)
            {
                m_army.RemoveAt(i);
            }
        }
        if (m_army.Count == 0)
        {
           result = false;
        }

        return result;
    }

    private void SpawnArmy(int count)
    {
        m_player.GetComponent<CharacterController>().Move(Vector3.zero - m_player.transform.position);

        for(int i = 0; i < count; i++)
        {
            //Get random index of spawnPoints list
            int randIndex = (int)Mathf.Round(Random.Range(0, ArmySpawnPoints.Count -1));

            float randX = Random.Range(-20, 20);
            float randZ = Random.Range(-20, 20);

            GameObject gm = Instantiate(Enemytypes[Random.Range(0, Enemytypes.Length)], 
                                        ArmySpawnPoints[randIndex].position + new Vector3(randX, 0, randZ), 
                                        Quaternion.identity);
                
            m_army.Add(gm);
            gm.GetComponent<Enemy>().Aggro();
        }
    }
}