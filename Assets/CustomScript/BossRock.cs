using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Bullet
{
    Rigidbody rigid;
    float scaleValue = 0.1f;
    bool isShoot;
    public float shootForce = 50f; // 발사 힘

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }

    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
        Shoot(); // 발사 로직 호출
    }

    IEnumerator GainPower()
    {
        while (!isShoot)
        {
            scaleValue += 0.005f;
            transform.localScale = Vector3.one * scaleValue;
            yield return null;
        }
    }

    void Shoot()
    {
        rigid.AddForce(transform.forward * shootForce, ForceMode.Impulse); // 발사 힘 적용
    }

    void Update()
    {
        // 필요한 경우 여기에 추가 로직 구현
    }
}