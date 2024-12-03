using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Myeongjin
{
	public class CTire : MonoBehaviour
	{
		[SerializeField] private GameObject frontLeftTire;
		[SerializeField] private GameObject frontRightTire;

		private Vector3 newForward;
		private Vector3 oldPosition;
		private Vector3 curPosition;
		private Vector3 forwardV;

        private Rigidbody bodyRigidbody;

		private int curDirection;
		private float curAngle = 0f;

		private const float moveSpeed = 20f;
		private const float MaxAngle = 40f;
		private const int rotateSpeed = 20;
		private const int MaxForwardSpeed = 30;
		private const int MaxRearSpeed = 15;

		private void Start()
		{
			oldPosition = transform.position;
			bodyRigidbody = GetComponent<Rigidbody>();
		}
        private void FixedUpdate()
        {
            Move();
        }
        private void Update()
		{
			SetDirection();

			InitValue();
		}
		private void SetDirection()
		{
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
				curDirection = 1;
			else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
				curDirection = -1;
			else
				curDirection = 0;

			if (Input.GetKey(KeyCode.LeftArrow) && (frontLeftTire.transform.localRotation.eulerAngles.y > 360 - MaxAngle || frontLeftTire.transform.localRotation.eulerAngles.y < MaxAngle))
			{
				curAngle -= Time.fixedDeltaTime * rotateSpeed;
				Vector3 tireRotation = new Vector3(0, -Time.fixedDeltaTime * rotateSpeed, 0);
				frontLeftTire.transform.rotation = Quaternion.Euler(frontLeftTire.transform.rotation.eulerAngles + tireRotation);
                frontRightTire.transform.rotation = Quaternion.Euler(frontRightTire.transform.rotation.eulerAngles + tireRotation);
            }
			if (Input.GetKey(KeyCode.RightArrow) && (frontLeftTire.transform.localRotation.eulerAngles.y < MaxAngle || frontLeftTire.transform.localRotation.eulerAngles.y > 360 - MaxAngle))
            {
                curAngle += Time.fixedDeltaTime * rotateSpeed;
                Vector3 tireRotation = new Vector3(0, Time.fixedDeltaTime * rotateSpeed, 0);
                frontLeftTire.transform.rotation = Quaternion.Euler(frontLeftTire.transform.rotation.eulerAngles + tireRotation);
                frontRightTire.transform.rotation = Quaternion.Euler(frontRightTire.transform.rotation.eulerAngles + tireRotation);
            }
		}
		private void Move()
        {
			if (bodyRigidbody.velocity.magnitude < MaxForwardSpeed)
			{
				forwardV = new Vector3(Mathf.Cos(Mathf.Deg2Rad * -curAngle), 0, Mathf.Sin(Mathf.Deg2Rad * -curAngle));
				bodyRigidbody.AddForce(forwardV * curDirection * moveSpeed);
			}

			if (bodyRigidbody.velocity.magnitude > 1f)
			{
				Vector3 direction = bodyRigidbody.velocity;
				direction.y = 0;

				Vector3 forward = Vector3.Slerp(transform.forward, direction, rotateSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction));

				transform.LookAt(transform.position + forward);

				//transform.Rotate(0, curAngle * rotateSpeed / 10 * Time.deltaTime * curDirection, 0);
			}
        }
		private void InitValue()
		{
			AligningTorque();

            curPosition = transform.position;

			oldPosition = curPosition;
		}
        private void AligningTorque()
        {
			Vector3 tireRotation;

            if (Mathf.Abs(curAngle) < 0.01f)
			{
				curAngle = 0;
				frontLeftTire.transform.localRotation = Quaternion.Euler(0, 0, 90);
                frontRightTire.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
			if (curAngle < 0)
			{
				curAngle += Time.fixedDeltaTime;
                tireRotation = new Vector3(0, Time.fixedDeltaTime, 0);
                frontLeftTire.transform.rotation = Quaternion.Euler(frontLeftTire.transform.rotation.eulerAngles + tireRotation);
                frontRightTire.transform.rotation = Quaternion.Euler(frontRightTire.transform.rotation.eulerAngles + tireRotation);
            }
			if (curAngle > 0)
            {
                curAngle -= Time.fixedDeltaTime;
                tireRotation = new Vector3(0, -Time.fixedDeltaTime, 0);
                frontLeftTire.transform.rotation = Quaternion.Euler(frontLeftTire.transform.rotation.eulerAngles + tireRotation);
                frontRightTire.transform.rotation = Quaternion.Euler(frontRightTire.transform.rotation.eulerAngles + tireRotation);
            }
        }
    }
}