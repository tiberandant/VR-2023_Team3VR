using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Bullet
{
    Rigidbody rigid;
    float scaleValue = 0.1f;
    bool isShoot;
    public float shootForce = 50f; // �߻� ��

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
        Shoot(); // �߻� ���� ȣ��
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
        rigid.AddForce(transform.forward * shootForce, ForceMode.Impulse); // �߻� �� ����
    }

    void Update()
    {
        // �ʿ��� ��� ���⿡ �߰� ���� ����
    }
}