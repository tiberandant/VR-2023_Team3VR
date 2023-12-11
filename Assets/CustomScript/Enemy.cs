using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A , B , C , D , E};
    public Type enemyType;
    public float detectionRange = 10f; 
    private SphereCollider detectionCollider; 
    public int maxHealth;
    public int curHealth;
    public Transform target;
    public Transform MissilePos;
    public BoxCollider meleeArea;
    public GameObject bullet;
    public bool isChase;
    public bool isAttack;
    public bool isDead;
    

    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshs;
    public NavMeshAgent nav;
    public Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        detectionCollider = gameObject.AddComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRange;
       

    }

    void ChaseStart()
    {
        if (enemyType == Type.D || enemyType == Type.E)
            return;

        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        if (enemyType == Type.D || enemyType == Type.E || isDead || !isChase)
            return;

        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }


    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targerting()
    {
        if(!isDead && (enemyType != Type.D && enemyType != Type.E))
        {
            float targetRadius = 0f;
            float targetRange = 0f;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.B:
                    targetRadius = 1f;
                    targetRange = 8f;
                    break;
                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
            }

              RaycastHit[] rayHits =
                 Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));
            if (rayHits.Length > 0 && !isAttack)
            {
                // 타겟(플레이어)과의 실제 거리를 확인합니다.
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                // 타겟이 인식 범위 내에 있을 때만 공격합니다.
                if (distanceToTarget <= detectionRange)
                {
                    StartCoroutine(Attack());
                }
            }
        }
        
    }

    IEnumerator Attack()
    {
        if (enemyType == Type.D || enemyType == Type.E)
            yield break;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > detectionRange)
        {
            // 타겟이 공격 범위를 벗어났으므로 공격을 중단합니다.
            isChase = true;
            isAttack = false;
            yield break; // 코루틴을 즉시 종료합니다.
        }

        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch(enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(1f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.B:
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, MissilePos.position, MissilePos.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 20;

                yield return new WaitForSeconds(2f);
                break;
        }

       
        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    void FixedUpdate()
    {
        Targerting();
        FreezeVelocity();   
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && enemyType != Type.D)
        {
            // 플레이어를 감지하면 추적 시작
            target = other.transform;
            ChaseStart();
        }

        if (other.tag =="Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec));
        }

       
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // 플레이어가 범위를 벗어나면 추적 중지
            isChase = false;
            anim.SetBool("isWalk", false);
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        foreach (MeshRenderer mesh in meshs)
            mesh.material.color = Color.red;
        
        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
        else
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;

            gameObject.layer = 12;
            isDead = true;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;

            rigid.AddForce(reactVec * 5, ForceMode.Impulse);

            
            Destroy(gameObject, 4);
        }
    }
}
