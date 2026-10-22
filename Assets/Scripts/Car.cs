using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed, speedBackwards, torque, torqueBackwards;
    private Manager manager;
    private WheelJoint2D[] wheelJoints;
    private JointMotor2D motor;

    private void Start()
    {
        manager = GameObject.Find("Canvas").GetComponent<Manager>();
        wheelJoints = GetComponents<WheelJoint2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Killzone") && manager.IsPlaying())
        {
            manager.GameOver(PlayerPrefs.GetInt("Language") == 1 ? "Du darfst nicht ins Gebäude fahren!" : "You are not allowed to drive into the building!");
        }
    }

    private void FixedUpdate()
    {
        int inputSpeed = manager.GetGeschwindigkeit();

        if (inputSpeed != 0)
        {
            if (inputSpeed > 0)
            {
                motor.motorSpeed = speed * -inputSpeed;
                motor.maxMotorTorque = torque;
            }
            else
            {
                motor.motorSpeed = speedBackwards * -inputSpeed;
                motor.maxMotorTorque = torqueBackwards;
            }

            for (int i = 0; i < wheelJoints.Length; i++)
            {
                wheelJoints[i].useMotor = true;
                wheelJoints[i].motor = motor;
            }
        }
        else
        {
            for (int i = 0; i < wheelJoints.Length; i++)
            {
                wheelJoints[i].useMotor = false;
            }
        }
    }
}
