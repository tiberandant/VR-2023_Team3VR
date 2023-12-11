using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InventoryVR : MonoBehaviour
{
    public GameObject inventory;
    public GameObject anchor;
    bool UIActive;
    private InputDevice rightHandDevice;

    void Start()
    {
        inventory.SetActive(false);
        UIActive = false;
        rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        // ������ ��ġ�� ��ȿ���� Ȯ���ϰ� ��ư �Է��� �˻��մϴ�.
        if (rightHandDevice.isValid)
        {
            rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonPressed);
            if (primaryButtonPressed)
            {
                UIActive = !UIActive;
                inventory.SetActive(UIActive);

                if (UIActive)
                {
                    inventory.transform.position = anchor.transform.position;
                    inventory.transform.eulerAngles = new Vector3(anchor.transform.eulerAngles.x + 15, anchor.transform.eulerAngles.y, 0);
                }
            }
        }
    }
}