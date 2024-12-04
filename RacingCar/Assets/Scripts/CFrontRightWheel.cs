using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myeongjin
{
	public class CFrontRightWheel : MonoBehaviour
	{
		private const float CARWEIGHT = 2200.0f;
		private const float MAXSPEED = 30.0f;
		private const float MAXHANDLEANGLE = 40.0f;

		private Rigidbody rigidBody;

		private float radius;
		private float rotatePower = 5f;
		private float velocity = 0;
		private float direction = 0;
		private float angle = 0;
        private float fixedAngle = 0;

		private void Start()
		{
			rigidBody = GetComponent<Rigidbody>();
			rigidBody.drag = 0.7f;
            radius = GetComponent<CapsuleCollider>().radius * transform.localScale.x;
		}
		private void FixedUpdate()
		{
			Rotate();
        }
        private void Rotate()
        {
			transform.Rotate(new Vector3(0, -velocity / (Mathf.PI * radius * 2), 0));
        }
        private void Update()
		{
            VerticalRotate();

            PushAcceleration();
        }
        private void VerticalRotate()
        {
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && angle > -MAXHANDLEANGLE)
                angle -= Time.fixedDeltaTime * rotatePower;
            else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && angle < MAXHANDLEANGLE)
                angle += Time.fixedDeltaTime * rotatePower;
            else
            {
                if (angle > 0.01f)
                    angle -= Time.fixedDeltaTime;
                else if (angle < -0.01f)
                    angle += Time.fixedDeltaTime;
                else
                    angle = 0;
            }
            if (angle < 0)
            {
                transform.localEulerAngles = new Vector3(0, -GetAckermannAngle(angle), 90);
                //Vector3 currentEulerAngles = transform.localRotation.eulerAngles;
                //fixedAngle = -GetAckermannAngle(angle);
                //currentEulerAngles.y = fixedAngle;
                //transform.rotation = Quaternion.Euler(currentEulerAngles);
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, angle, 90);
                //Vector3 currentEulerAngles = transform.localRotation.eulerAngles;
                //currentEulerAngles.y = angle;
                //transform.rotation = Quaternion.Euler(currentEulerAngles);
            }
        }
        float GetAckermannAngle(float angle)
        {
            float R = 4 / Mathf.Tan(Mathf.Abs(angle) * Mathf.Deg2Rad); // 4: 차체 길이 나중에 차체를 인스턴스로 받아서 차체길이 가져오기
            float M = R + 2;    // 2: 좌우 바퀴의 간격
            float ackerman = Mathf.Atan(4 / M) * Mathf.Rad2Deg;

            return ackerman;
        }
        private void PushAcceleration()
        {
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && velocity < MAXSPEED)
                velocity += Time.fixedDeltaTime * 2;
            else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && velocity > -(MAXSPEED / 2))
                velocity -= Time.fixedDeltaTime * 2;
            else
            {
                if (velocity > 0.01f)
                    velocity -= Time.fixedDeltaTime;
                else if (velocity < -0.01f)
                    velocity += Time.fixedDeltaTime * 1.5f;
                else
                    velocity = 0;
            }
        }
        private void OnCollisionStay(Collision collision)
		{
			SetDirection(collision.contacts[0]);

			Move();
		}
        private void Move()
		{
			rigidBody.AddForce(new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * direction), Mathf.Cos(Mathf.Deg2Rad * direction)) * velocity);
        }
		private void SetDirection(ContactPoint contact)
		{
			Vector3 collisionVector3;
			Vector3 worldBottomPosition;

			worldBottomPosition = transform.position;
            worldBottomPosition.x = 0;
            worldBottomPosition.y = -radius;
			worldBottomPosition.z = 0;

            collisionVector3 = contact.point - transform.position;

            direction = GetAngleBetweenVectors(GetDotProduct(worldBottomPosition, collisionVector3), GetVector3Magnitude(worldBottomPosition), GetVector3Magnitude(collisionVector3));
        }
		float GetAngleBetweenVectors(float dotProduct, float magnitude1, float magnitude2)
		{
			return Mathf.Acos(dotProduct / (magnitude1 * magnitude2));
		}

        float GetVector3Magnitude(Vector3 vector)
		{
			return Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2) + Mathf.Pow(vector.z, 2));
		}
		float GetDotProduct(Vector3 vector1, Vector3 vector2)
		{
			return vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z;
		}
	}
}