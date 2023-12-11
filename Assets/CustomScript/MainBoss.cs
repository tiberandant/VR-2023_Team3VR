using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainBoss : Enemy
{ 
    public bool isJumpAttack = false;
    public bool isSlash1 = false;
    public bool isSlash2 = false;
    public Transform EBullet;
    public bool isLook;



    Vector3 lookVec;
    Vector3 jumpattackVec;
    Vector3 slash1Vec;
    Vector3 slash2Vec;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        nav.isStopped = true;
        StartCoroutine(Think());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }

        if (isLook)
        {
            lookVec = (target.position - transform.position).normalized;
            transform.LookAt(target.position);
        }
        else
        {
            if (isJumpAttack)
                nav.SetDestination(jumpattackVec);
            else if (isSlash1)
                nav.SetDestination(slash1Vec);
            else if (isSlash2)
                nav.SetDestination(slash2Vec);
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 6);
        switch (ranAction)
        {
            case 0:
            case 1:
            case 2:
                StartCoroutine(Shot());
                break;
            case 3:
                StartCoroutine(JumpShot());
                break;
            case 4:
                StartCoroutine(JumpAttack());
                break;
            case 5:
                StartCoroutine(Slash1());
                break;
            case 6:
                StartCoroutine(Slash2());
                break;
        }

    }

    IEnumerator Shot()
    {
        if (Vector3.Distance(target.position, transform.position) > detectionRange)
        {
            // 타겟이 인식 범위 밖에 있으면 다른 행동을 생각합니다.
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Think());
            yield break;
        }

        isLook = false;
        anim.SetTrigger("doShotMain");

        for (int i = 0; i < 3; i++) 
        {
            GameObject instantEshot = Instantiate(bullet, EBullet.position, EBullet.rotation);
            Rigidbody rigidBullet = instantEshot.GetComponent<Rigidbody>();
            rigidBullet.velocity = transform.forward * 30;

            yield return new WaitForSeconds(0.1f); 
        }

        isLook = true;
        StartCoroutine(Think());
    }

    IEnumerator JumpShot()
    {

        
        if (Vector3.Distance(target.position, transform.position) > detectionRange)
        {
            // 타겟이 인식 범위 밖에 있으면 다른 행동을 생각합니다.
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Think());
            yield break;
        }

        isLook = false;
        anim.SetTrigger("doJumpShot");

        for (int i = 0; i < 3; i++)
        {
            GameObject instantEshot = Instantiate(bullet, EBullet.position, EBullet.rotation);
            Rigidbody rigidBullet = instantEshot.GetComponent<Rigidbody>();
            rigidBullet.velocity = transform.forward * 30; // 총알은 타겟 방향으로 발사

            yield return new WaitForSeconds(0.1f);
        }

        isLook = true;
        StartCoroutine(Think());
    }

    IEnumerator JumpAttack()
    {
        
        if (Vector3.Distance(target.position, transform.position) > detectionRange)
        {
            // 타겟이 인식 범위 밖에 있으면 다른 행동을 생각합니다.
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Think());
            yield break;
        }

        isJumpAttack = true;

        jumpattackVec = target.position + lookVec;

        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doJumpAttack");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;

        isJumpAttack = false;
        StartCoroutine(Think());
    }
    IEnumerator Slash1()
    {
        

        if (Vector3.Distance(target.position, transform.position) > detectionRange)
        {
            // 타겟이 인식 범위 밖에 있으면 다른 행동을 생각합니다.
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Think());
            yield break;
        }

        isSlash1 = true;

        slash1Vec = target.position + lookVec;

        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doSlash1");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;

        isSlash1 = false;
        StartCoroutine(Think());
    }
    IEnumerator Slash2()
    {
       
        if (Vector3.Distance(target.position, transform.position) > detectionRange)
        {
            // 타겟이 인식 범위 밖에 있으면 다른 행동을 생각합니다.
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Think());
            yield break;
        }

        isSlash2 = true;

        slash2Vec = target.position + lookVec;

        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doSlash2");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;
        isSlash2 = false;
        StartCoroutine(Think());
    }

}
