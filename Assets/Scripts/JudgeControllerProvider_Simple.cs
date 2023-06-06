using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[System.Serializable]
public class ControlDevicePosUpdateState
{
    [HideInInspector]
    public float time;
    [HideInInspector]
    public Vector3 lastPos;
    /// <summary>
    /// �ö������ж�ʱ��
    /// </summary>
    public float judgeTime;
    /// <summary>
    /// �ö����жϾ���
    /// </summary>
    public float judgeDistance;
    /// <summary>
    /// �ö����жϷ���
    /// </summary>
    public Vector3 judgeDirection;
    [HideInInspector]
    //�����жϷ���
    public GameObject dirObjTemp;
    /// <summary>
    /// �ö����жϷ�����������Ƕ�
    /// </summary>
    public float judgeAngleOffset;
    [HideInInspector]
    public Action<Transform> actionSuccess;
    //public Action actionFail;
    //public Action actionUpdate;
}

public class JudgeControllerProvider_Simple : MonoBehaviour
{
    public Transform leftHandController;
    public Transform rightHandController;

    public List<ControlDevicePosUpdateState> controlDevicePosUpdateStates_Left;
    public List<ControlDevicePosUpdateState> controlDevicePosUpdateStates_Right;

    private GameObject leftHandHaveThing;
    private GameObject rightHandHaveThing;


    public GameObject a,b;

    public List<GameObject> objPrefabA;
    public List<GameObject> objPrefabB;

    private void Awake()
    {
        SaveLog.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = new GameObject();
        obj.transform.parent = transform;
        obj.transform.localPosition = controlDevicePosUpdateStates_Left[0].judgeDirection;
        controlDevicePosUpdateStates_Left[0].dirObjTemp = obj;

        obj = new GameObject();
        obj.transform.parent = transform;
        obj.transform.localPosition = controlDevicePosUpdateStates_Left[1].judgeDirection;
        controlDevicePosUpdateStates_Left[1].dirObjTemp = obj;

        obj = new GameObject();
        obj.transform.parent = transform;
        obj.transform.localPosition = controlDevicePosUpdateStates_Right[0].judgeDirection;
        controlDevicePosUpdateStates_Right[0].dirObjTemp = obj;

        obj = new GameObject();
        obj.transform.parent = transform;
        obj.transform.localPosition = controlDevicePosUpdateStates_Right[1].judgeDirection;
        controlDevicePosUpdateStates_Right[1].dirObjTemp = obj;


        controlDevicePosUpdateStates_Left[0].actionSuccess += (control) =>
        {
            if (leftHandHaveThing != null)
            {
                Destroy(leftHandHaveThing.gameObject);
                leftHandHaveThing = null;
            }
            leftHandHaveThing = Instantiate(a, control);
            leftHandHaveThing.transform.localPosition = Vector3.zero;
            leftHandHaveThing.transform.localEulerAngles = Vector3.zero;
        };
        controlDevicePosUpdateStates_Left[1].actionSuccess += (control) =>
        {
            if (leftHandHaveThing != null)
            {
                Destroy(leftHandHaveThing.gameObject);
                leftHandHaveThing = null;
            }
            leftHandHaveThing = Instantiate(b, control);
            leftHandHaveThing.transform.localPosition = Vector3.zero;
            leftHandHaveThing.transform.localEulerAngles = Vector3.zero;
        };

        controlDevicePosUpdateStates_Right[0].actionSuccess += (control) =>
        {
            if (rightHandHaveThing != null)
            {
                Destroy(rightHandHaveThing.gameObject);
                rightHandHaveThing = null;
            }
            rightHandHaveThing = Instantiate(a, control);
            rightHandHaveThing.transform.localPosition = Vector3.zero;
            rightHandHaveThing.transform.localEulerAngles = Vector3.zero;
        };
        controlDevicePosUpdateStates_Right[1].actionSuccess += (control) =>
        {
            if (rightHandHaveThing != null)
            {
                Destroy(rightHandHaveThing.gameObject);
                rightHandHaveThing = null;
            }
            rightHandHaveThing = Instantiate(b, control);
            rightHandHaveThing.transform.localPosition = Vector3.zero;
            rightHandHaveThing.transform.localEulerAngles = Vector3.zero;
        };
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0,Camera.main.transform.eulerAngles.y,0);

        //�ж����ֵ�ǰ�ƶ�״̬
        //�жϵ�ǰ�������Ƿ��ڱ�׷��״̬
        if (true)
        {
            for(int i = 0; i < controlDevicePosUpdateStates_Left.Count; i++)
            {
                if (controlDevicePosUpdateStates_Left[i].time == 0.0f)
                {
                    controlDevicePosUpdateStates_Left[i].lastPos = leftHandController.position;
                    controlDevicePosUpdateStates_Left[i].time += Time.deltaTime;
                }
                else if (controlDevicePosUpdateStates_Left[i].time >= controlDevicePosUpdateStates_Left[i].judgeTime)
                {
                    //�Ե�ǰ������м���
                    Vector3 dir = leftHandController.position - controlDevicePosUpdateStates_Left[i].lastPos;
                    Vector3 judgeDir = controlDevicePosUpdateStates_Left[i].dirObjTemp.transform.position;
                    if (dir.magnitude < controlDevicePosUpdateStates_Left[i].judgeDistance)
                    {
                        //�����жϲ���
                        Debug.Log("�����ж�ʧ�ܣ����벻��");
                    }
                    else
                    {
                        if (Vector3.Angle(dir, judgeDir /*controlDevicePosUpdateStates_Left[i].judgeDirection*/) <= controlDevicePosUpdateStates_Left[i].judgeAngleOffset)
                        {
                            //�жϳɹ�
                            Debug.Log("�����жϳɹ�");
                            if(controlDevicePosUpdateStates_Left[i].actionSuccess != null)
                                controlDevicePosUpdateStates_Left[i].actionSuccess(leftHandController);


                            Debug.Log("���� �ж�������" + i);
                            Debug.Log("dir.magnitude��" + dir.magnitude);
                            Debug.Log("dir: " + dir);
                            Debug.Log("DirJudge:" + judgeDir);
                            Debug.Log("Angle:" + Vector3.Angle(dir, judgeDir));
                            Debug.Log("-------------------------------------------");
                            controlDevicePosUpdateStates_Left[i].time = 0.0f;


                            break;
                        }
                        else
                        {
                            //�ж�ʧ��
                            Debug.Log("�����ж�ʧ�ܣ��Ƕȳ�������");

                        }
                    }
                    Debug.Log("���� �ж�������" + i);
                    Debug.Log("dir.magnitude��" + dir.magnitude);
                    Debug.Log("dir: " + dir);
                    Debug.Log("DirJudge:" + judgeDir);
                    Debug.Log("Angle:" + Vector3.Angle(dir, judgeDir));
                    Debug.Log("-------------------------------------------");

                    controlDevicePosUpdateStates_Left[i].time = 0.0f;
                }
                else
                {
                    controlDevicePosUpdateStates_Left[i].time += Time.deltaTime;
                }
            }
        }
        //�ж����ֵ�ǰ�ƶ�״̬
        //�жϵ�ǰ�������Ƿ��ڱ�׷��״̬
        if (true)
        {
            for (int i = 0; i < controlDevicePosUpdateStates_Right.Count; i++)
            {
                if (controlDevicePosUpdateStates_Right[i].time == 0.0f)
                {
                    controlDevicePosUpdateStates_Right[i].lastPos = rightHandController.position;
                    controlDevicePosUpdateStates_Right[i].time += Time.deltaTime;
                }
                else if (controlDevicePosUpdateStates_Right[i].time >= controlDevicePosUpdateStates_Right[i].judgeTime)
                {
                    //�Ե�ǰ������м���
                    Vector3 dir = rightHandController.position - controlDevicePosUpdateStates_Right[i].lastPos;
                    Vector3 judgeDir = controlDevicePosUpdateStates_Right[i].dirObjTemp.transform.position;
                    if (dir.magnitude < controlDevicePosUpdateStates_Right[i].judgeDistance)
                    {
                        //�����жϲ���
                        Debug.Log("�����ж�ʧ�ܣ����벻��");
                    }
                    else
                    {
                        if (Vector3.Angle(dir, judgeDir/*controlDevicePosUpdateStates_Right[i].judgeDirection*/) <= controlDevicePosUpdateStates_Right[i].judgeAngleOffset)
                        {
                            //�жϳɹ�
                            Debug.Log("�����жϳɹ�");
                            if (controlDevicePosUpdateStates_Right[i].actionSuccess != null)
                                controlDevicePosUpdateStates_Right[i].actionSuccess(rightHandController);

                            controlDevicePosUpdateStates_Right[i].time = 0.0f;

                            Debug.Log("���� �ж�������" + i);
                            Debug.Log("dir.magnitude��" + dir.magnitude);
                            Debug.Log("dir: " + dir);
                            Debug.Log("DirJudge:" + judgeDir);
                            Debug.Log("Angle:" + Vector3.Angle(dir, judgeDir));
                            Debug.Log("-------------------------------------------");
                            break;
                        }
                        else
                        {
                            //�ж�ʧ��
                            Debug.Log("�����ж�ʧ�ܣ��Ƕȳ�������");

                        }
                    }
                    controlDevicePosUpdateStates_Right[i].time = 0.0f;

                    Debug.Log("���� �ж�������" + i);
                    Debug.Log("dir.magnitude��" + dir.magnitude);
                    Debug.Log("dir: " + dir);
                    Debug.Log("DirJudge:" + judgeDir);
                    Debug.Log("Angle:" + Vector3.Angle(dir, judgeDir));
                    Debug.Log("-------------------------------------------");
                }
                else
                {
                    controlDevicePosUpdateStates_Right[i].time += Time.deltaTime;
                }
            }
        }
    }
}
