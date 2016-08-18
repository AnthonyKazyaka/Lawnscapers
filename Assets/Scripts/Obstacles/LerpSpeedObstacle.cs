using UnityEngine;
using System.Collections;
using System;

public class LerpSpeedObstacle : MovingObstacle
{
    [SerializeField]
    private float _lerpCoefficient = 2.0f;
    public float LerpCoefficient { get { return _lerpCoefficient; } set { _lerpCoefficient = value; } }

    public override void Move()
    {
        Vector3 newPosition; = (_direction * (EndTransform.position - StartTransform.position).normalized) * Time.deltaTime + gameObject.transform.position;
        //Fix this. Don't need to do this calculation because we're going to Lerp anyways

        if (_direction == 1)
        {
            if ((newPosition - StartTransform.position).magnitude > (EndTransform.position - StartTransform.position).magnitude)
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

        if (!IsCurrentlyAtEndPoint)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, newPosition, Time.deltaTime * LerpCoefficient);
        }
    }
}
