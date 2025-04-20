using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class HingeJointAutoClose : MonoBehaviour
{
    public float autoCloseSpeed = 5f;     // 关闭速度
    public float closeThreshold = 1f;     // 多少度以内认为关闭
    public float autoCloseDelay = 10f;    // 自动关闭延迟时间（秒）

    private HingeJoint hinge;
    private JointSpring hingeSpring;
    private float initialAngle;
    private float lastOpenedTime;
    private bool isClosing = false;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        hingeSpring = hinge.spring;
        initialAngle = hinge.angle;
        lastOpenedTime = Time.time;
    }

    void Update()
    {
        float currentAngle = hinge.angle;

        // 如果门已偏离初始角度，记录最后一次开启时间
        if (Mathf.Abs(currentAngle - initialAngle) > closeThreshold)
        {
            lastOpenedTime = Time.time;
            isClosing = false;
        }

        // 20 秒后启动关闭
        if (!isClosing && Time.time - lastOpenedTime >= autoCloseDelay)
        {
            isClosing = true;
        }

        if (isClosing)
        {
            hinge.useSpring = true;
            hingeSpring.targetPosition = initialAngle;
            hingeSpring.spring = autoCloseSpeed;
            hingeSpring.damper = 1f;
            hinge.spring = hingeSpring;

            // 如果已接近目标角度，就停止关闭
            if (Mathf.Abs(currentAngle - initialAngle) < closeThreshold)
            {
                hinge.useSpring = false;
                isClosing = false;
            }
        }
    }
}
