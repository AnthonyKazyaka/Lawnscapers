using UnityEngine;
using System.Collections;
using System;

public class FixedSpeedObstacle : MovingObstacle
{
    [SerializeField]
    private float _speed = 2.0f;
    public float Speed { get { return _speed; } set { _speed = value; } }

    public override void Move()
    {
        Vector3 newPosition = (_direction * Speed * (EndTransform.position - StartTransform.position).normalized) * Time.deltaTime + gameObject.transform.position;

        if(_direction == 1)
        {
            if((newPosition- StartTransform.position).magnitude > (EndTransform.position - StartTransform.position).magnitude)
            {
                newPosition = EndTransform.position;
                _direction *= -1;
                IsCurrentlyAtEndPoint = true;
            }
        }
        else if (_direction == -1)
        {
            if ((newPosition - EndTransform.position).magnitude > (EndTransform.position - StartTransform.position).magnitude)
            {
                newPosition = StartTransform.position;
                _direction *= -1;
                IsCurrentlyAtEndPoint = true;
            }
        }

        gameObject.transform.position = newPosition;
    }

}
