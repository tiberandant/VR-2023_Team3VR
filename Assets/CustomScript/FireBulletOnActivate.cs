using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletOnActivate : MonoBehaviour
{
    public enum Type { pistol , Blitch};
    public Type type;
    public int damage;
    public float rate;
    public float bulletLifetime;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(Firebullet);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3);
        }

    }

    public void Firebullet(ActivateEventArgs arg)
    {
        switch (type)
        {
            case Type.pistol:
                FirePistolBullet();
                break;

            case Type.Blitch:
                FireBlitchBullet();
                break;
        }
    }

    private void FirePistolBullet()
    {
        // �ǽ��� Ÿ�� ������ �Ѿ� �߻� ����
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 40; 

        // �ǽ����� ź�� ���� ����
        EjectBulletCase();
    }

    private void FireBlitchBullet()
    {
        int bulletCount = 10;
        for (int i = 0; i < bulletCount; i++)
        {
            // źȯ�� �߻� ���⿡ ������ ������ �߰�
            Quaternion randomRotation = Quaternion.Euler(Random.Range(-8, 8), Random.Range(-8, 8), 0);
            GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation * randomRotation);
            Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = instantBullet.transform.forward * 80;

            Destroy(instantBullet, bulletLifetime);
        }


        // ����ġ�� ź�� ���� ����
        EjectBulletCase();

        
    }

    private void EjectBulletCase()
    {
        // ź�� ���� ����
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }


}