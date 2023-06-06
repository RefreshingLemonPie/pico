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
    /// 该动作的判断时间
    /// </summary>
    public float judgeTime;
    /// <summary>
    /// 该动作判断距离
    /// </summary>
    public float judgeDistance;
    /// <summary>
    /// 该动作判断方向
    /// </summary>
    public Vector3 judgeDirection;
    [HideInInspector]
    //用于判断方向
    public GameObject dirObjTemp;
    /// <summary>
    /// 该动作判断方向允许的误差角度
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

        //判断左手当前移动状态
        //判断当前控制器是否处于被追踪状态
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
                    //对当前坐标进行计算
                    Vector3 dir = leftHandController.position - controlDevicePosUpdateStates_Left[i].lastPos;
                    Vector3 judgeDir = controlDevicePosUpdateStates_Left[i].dirObjTemp.transform.position;
                    if (dir.magnitude < controlDevicePosUpdateStates_Left[i].judgeDistance)
                    {
                        //距离判断不足
                        Debug.Log("左手判断失败，距离不足");
                    }
                    else
                    {
                        if (Vector3.Angle(dir, judgeDir /*controlDevicePosUpdateStates_Left[i].judgeDirection*/) <= controlDevicePosUpdateStates_Left[i].judgeAngleOffset)
                        {
                            //判断成功
                            Debug.Log("左手判断成功");
                            if(controlDevicePosUpdateStates_Left[i].actionSuccess != null)
                                controlDevicePosUpdateStates_Left[i].actionSuccess(leftHandController);


                            Debug.Log("左手 判断索引：" + i);
                            Debug.Log("dir.magnitude：" + dir.magnitude);
                            Debug.Log("dir: " + dir);
                            Debug.Log("DirJudge:" + judgeDir);
                            Debug.Log("Angle:" + Vector3.Angle(dir, judgeDir));
                            Debug.Log("-------------------------------------------");
                            controlDevicePosUpdateStates_Left[i].time = 0.0f;


                            break;
                        }
                        else
                        {
                            //判断失败
                            Debug.Log("左手判断失败，角度超出限制");

                        }
                    }
                    Debug.Log("左手 判断索引：" + i);
                    Debug.Log("dir.magnitude：" + dir.magnitude);
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
        //判断右手当前移动状态
        //判断当前控制器是否处于被追踪状态
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
                    //对当前坐标进行计算
                    Vector3 dir = rightHandController.position - controlDevicePosUpdateStates_Right[i].lastPos;
                    Vector3 judgeDir = controlDevicePosUpdateStates_Right[i].dirObjTemp.transform.position;
                    if (dir.magnitude < controlDevicePosUpdateStates_Right[i].judgeDistance)
                    {
                        //距离判断不足
                        Debug.Log("右手判断失败，距离不足");
                    }
                    else
                    {
                        if (Vector3.Angle(dir, judgeDir/*controlDevicePosUpdateStates_Right[i].judgeDirection*/) <= controlDevicePosUpdateStates_Right[i].judgeAngleOffset)
                        {
                            //判断成功
                            Debug.Log("右手判断成功");
                            if (controlDevicePosUpdateStates_Right[i].actionSuccess != null)
                                controlDevicePosUpdateStates_Right[i].actionSuccess(rightHandController);

                            controlDevicePosUpdateStates_Right[i].time = 0.0f;

                            Debug.Log("右手 判断索引：" + i);
                            Debug.Log("dir.magnitude：" + dir.magnitude);
                            Debug.Log("dir: " + dir);
                            Debug.Log("DirJudge:" + judgeDir);
                            Debug.Log("Angle:" + Vector3.Angle(dir, judgeDir));
                            Debug.Log("-------------------------------------------");
                            break;
                        }
                        else
                        {
                            //判断失败
                            Debug.Log("右手判断失败，角度超出限制");

                        }
                    }
                    controlDevicePosUpdateStates_Right[i].time = 0.0f;

                    Debug.Log("右手 判断索引：" + i);
                    Debug.Log("dir.magnitude：" + dir.magnitude);
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
