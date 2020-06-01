 /// <summary>
/// 
/// </summary>
using UnityEngine;
using UnitySteer.Behaviors;
using System.Collections;

public class LocomotionPlayer : MonoBehaviour
{

    protected Animator animator;

    private float speed = 0;
    private float direction = 0;
    private Locomotion locomotion = null;
    private Vehicle vehicle;
    private Vector3 relativePosition;
    private Transform leader;
    private float LowDownSpeed = 0.2f;
    private int accelerate = 3;


    // Use this for initialization
    void Awake()
    {
        leader = GameObject.FindGameObjectWithTag("Leader").transform;
        animator = GetComponent<Animator>();
        locomotion = new Locomotion(animator);
        vehicle = GetComponent<Vehicle>();
    }

    //ÿһ֡�����ò�������ϯ�����ƶ�
    public void Move()
    {
        speed = vehicle.Speed;
        if (speed >= 0.01f)
        {
            Vector3 axis = Vector3.Cross(vehicle.Velocity, vehicle.DesiredVelocity);
            direction = Vector3.Angle(vehicle.Velocity, vehicle.DesiredVelocity) / 180.0f * (axis.y < 0 ? -1 : 1);
        }
        locomotion.Do(speed * 6, direction * 180);
    }

    //����һ�ξͿ������ת��targetDirection,���ǿ��ƽű���Ҫ����Ƿ��Ѿ�ת������ٵ���Stop()����ϯͣ����
    public void TurnOnSpot(Vector3 targetDirection)
    {
        Vector3 axis = Vector3.Cross(transform.forward, targetDirection);
        direction = Vector3.Angle(transform.forward, targetDirection) / 180.0f * (axis.y < 0 ? -1 : 1);
        if (Mathf.Abs(direction * 180) > 10)
        {
            locomotion.Do(0.0f, direction * 180);

        }
    }

    public void Stop()
    {
        this.gameObject.GetComponent<SteerForNeighborGroup>().enabled = false;
        this.gameObject.GetComponent<SteerForLeadFollowing>().enabled = false;
        speed = 0.0f;
        direction = 0.0f;
        locomotion.Do(speed * 6, direction * 180);
    }

    public void FollowLeader()
    {
        speed = vehicle.Speed;
        this.gameObject.GetComponent<SteerForNeighborGroup>().enabled = true;
        this.gameObject.GetComponent<SteerForLeadFollowing>().enabled = true;

        //�ȸ��ݾ�����ϯ�����Զ���������ٶȵ����
        if (Vector3.Distance(transform.position, leader.position) < 0.5f)
        {
            speed = LowDownSpeed;
        }
        else if (Vector3.Distance(transform.position, leader.position) > 9f)
        {
            speed *= accelerate;
        }


        if (speed >= 0.01f)
        {
            Vector3 axis = Vector3.Cross(vehicle.Velocity, vehicle.DesiredVelocity);
            direction = Vector3.Angle(vehicle.Velocity, vehicle.DesiredVelocity) / 180.0f * (axis.y < 0 ? -1 : 1);
        }
        //�������˲ʱ��ת�����󲿷�����-20��20֮�䣬ͻȻ����30,40�Ļ��г鴤
        if (direction * 180 > 20 || direction * 180 < -20)
            direction = 0.1f;
        locomotion.Do(speed * 6, direction * 180);
    }



    #region �����ϰ���Ķ���
    public void ShakeHands()
    {
        animator.SetBool("ShakeHands", true);
    }

    public void StopShakeHands()
    {
        animator.SetBool("ShakeHands", false);
    }

    public void WaveHand()
    {
        animator.SetBool("WaveHand", true);
    }

    public void StopWaveHand()
    {
        animator.SetBool("WaveHand", false);
    }

    public void Salute()
    {
        animator.SetBool("Salute", true);
    }

    public void StopSalute()
    {
        animator.SetBool("Salute", false);
    }

    public void ClapHands()
    {
        animator.SetBool("ClapHands", true);
    }
    public void StopClapHands()
    {
        animator.SetBool("ClapHands", false);
    }
    public bool GetBoolWaveHand()
    {
        return animator.GetBool("WaveHand");
    }
#endregion

}
