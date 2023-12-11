using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;
    public Transform missilePortC;
    public Transform missilePortD;
    public Transform BigShotPos;
    public bool isLook;

    Vector3 lookVec;
    Vector3 tauntVec;
    

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

    void Update()
    {
       if (isDead)
        {
            StopAllCoroutines();
            return;
        }
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
            nav.SetDestination(tauntVec);
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 5);
        switch (ranAction)
        {
            case 0:
            case 1:
                StartCoroutine(MissileShot());
                break;
            case 2:
            case 3:
                StartCoroutine(RockShot());
                break;
            case 4:
                StartCoroutine(Taunt());

                break;
        }
    }

    IEnumerator MissileShot()
    {
        if (Vector3.Distance(target.position, transform.position) > detectionRange)
        {
            // 타겟이 인식 범위 밖에 있으면 다른 행동을 생각합니다.
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Think());
            yield break;
        }


        float angle = 45f; // 발사 각도

        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.1f);

        GameObject instantMissileA = Instantiate(missile, missilePortA.position, missilePortA.rotation);
        SetMissileVelocity(instantMissileA, target, angle);

        yield return new WaitForSeconds(0.2f);

        GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        SetMissileVelocity(instantMissileB, target, angle);

        yield return new WaitForSeconds(0.3f);

        GameObject instantMissileC = Instantiate(missile, missilePortC.position, missilePortC.rotation);
        SetMissileVelocity(instantMissileC, target, angle);

        yield return new WaitForSeconds(0.4f);

        GameObject instantMissileD = Instantiate(missile, missilePortD.position, missilePortD.rotation);
        SetMissileVelocity(instantMissileD, target, angle);

        yield return new WaitForSeconds(2f);
        StartCoroutine(Think());
    }

    void SetMissileVelocity(GameObject missile, Transform target, float angle)
    {
        Rigidbody missileRigidbody = missile.GetComponent<Rigidbody>();
        if (missileRigidbody == null)
        {
            missileRigidbody = missile.AddComponent<Rigidbody>();
        }
        missileRigidbody.velocity = CalculateLaunchVelocity(missile, target, angle);
    }

    Vector3 CalculateLaunchVelocity(GameObject missile, Transform target, float angle)
    {
        if (target == null)
        {
            Debug.LogError("Target is null");
            return Vector3.zero;
        }

        Vector3 direction = target.position - missile.transform.position;
        float height = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float a = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += height / Mathf.Tan(a);

        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * direction.normalized;
    }

    IEnumerator RockShot()
{
        if (Vector3.Distance(target.position, transform.position) > detectionRange)
        {
            // 타겟이 인식 범위 밖에 있으면 다른 행동을 생각합니다.
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Think());
            yield break;
        }


        isLook = false;
    anim.SetTrigger("doBigShot");

    // 타겟을 향한 발사 위치의 회전 설정
    BigShotPos.rotation = Quaternion.LookRotation(target.position - BigShotPos.position);

    GameObject instantBigShot = Instantiate(bullet, BigShotPos.position, BigShotPos.rotation);
    Rigidbody rigidBullet = instantBigShot.GetComponent<Rigidbody>();
    
    yield return new WaitForSeconds(3f);

    isLook = true;
    StartCoroutine(Think());
}

    IEnumerator Taunt()
    {
        if (Vector3.Distance(target.position, transform.position) > detectionRange)
        {
            // 타겟이 인식 범위 밖에 있으면 다른 행동을 생각합니다.
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Think());
            yield break;
        }

        tauntVec = target.position + lookVec;

        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doTaunt");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;
        StartCoroutine(Think());
    }
}
