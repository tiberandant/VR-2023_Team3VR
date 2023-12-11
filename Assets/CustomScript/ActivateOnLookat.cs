using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnLookat : MonoBehaviour
{
    public List<GameObject> linkedItems;

    public new Camera camera;
    public Behaviour target;

    public float thresholdAngle = 30f;
    public float thresholdDuration = 2f;

    private bool isLooking = false;
    private float showingTime;

    private void Awake()
    {
        target.enabled = false;
    }

    private void Update()
    {
        var dir = target.transform.position - camera.transform.position;
        var angle = Vector3.Angle(camera.transform.forward.normalized, dir);

        if(angle <= thresholdAngle)
        {
            if(!isLooking)
            {
                isLooking = true;
                showingTime = Time.time + thresholdDuration;
            }
            else
            {
                if(!target.enabled &&
                    Time.time > showingTime)
                {
                    target.enabled = true;
                }
            }
        }
        else
        {
            if(isLooking)
            {
                isLooking = false;
                target.enabled = false;
            }
        }

        if (!this.enabled)
        {
            foreach (var item in linkedItems)
            {
                if (item.activeSelf)
                {
                    item.SetActive(false);
                }
            }
        }

        // UI 요소가 활성화되면 모든 연결된 아이템도 활성화
        if (this.enabled)
        {
            foreach (var item in linkedItems)
            {
                if (!item.activeSelf)
                {
                    item.SetActive(true);
                }
            }
        }
    }

}
