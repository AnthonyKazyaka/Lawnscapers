using UnityEngine;
using System.Collections;
using System;

public class LerpSpeedObstacle : MovingObstacle
{
    [SerializeField]
    private float _lerpCoefficient = 2.0f;
    public float LerpCoefficient { get { return _lerpCoefficient; } set { _lerpCoefficient = value; } }

    [SerializeField]
    private float _endPointSnapDistance = 0.05f;
    public float EndPointSnapDistance { get { return _endPointSnapDistance; } set { _endPointSnapDistance = value; } }

    public override void Move()
    {
        Vector3 newPosition = Vector3.zero;
        //Fix this. Don't need to do this calculation because we're going to Lerp anyways

        if (_direction == 1)
        {
            newPosition = Vector3.Lerp(gameObject.transform.position, EndTransform.position, Time.deltaTime * LerpCoefficient);

            if(Vector3.Distance(newPosition, EndTransform.position) <= EndPointSnapDistance)
            {
                newPosition = EndTransform.position;
                _direction *= -1;
                IsCurrentlyAtEndPoint = true;
            }
        }
        else if (_direction == -1)
        {
            newPosition = Vector3.Lerp(gameObject.transform.position, StartTransform.position, Time.deltaTime * LerpCoefficient);

            if (Vector3.Distance(newPosition, StartTransform.position) <= EndPointSnapDistance)
            {
                newPosition = StartTransform.position;
                _direction *= -1;
                IsCurrentlyAtEndPoint = true;
            }
        }

        if (!IsCurrentlyAtEndPoint)
        {
            gameObject.transform.position = newPosition;
        }
    }
}
