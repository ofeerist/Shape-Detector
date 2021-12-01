using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ShapeDetector : MonoBehaviour
    {
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private float _rangeForSquareAngle;
        [SerializeField] private LineRenderer _lineRenderer;

        [Space] [SerializeField] private float _maxDistanceForSquare;
        [Space] [SerializeField] private float _maxDistanceForTriangle;
        [Space] [SerializeField] private float _maxDistanceForCircle;
        [Space] [SerializeField] private float _minDistanceForZ;
        
        private int _squareCount;
        private int _sharpCount;
        private int _obtuseCount;
        private void Start()
        {
            _inputHandler.OnAdd += (current, previous) =>
            {
                _lineRenderer.positionCount = _inputHandler.Points.Count;
                _lineRenderer.SetPositions(_inputHandler.Points.ToArray());
                
                var cos = Vector3.Dot(previous, current) / (previous.magnitude * current.magnitude);
                var angle = Mathf.Acos(cos) * Mathf.Rad2Deg;
                if (angle < 90 + _rangeForSquareAngle && angle > 90 - _rangeForSquareAngle)
                {
                    print("SquareAngle! " + angle);
                    _squareCount++;
                }
                else if (angle > 90 + _rangeForSquareAngle)
                {
                    print("SharpAngle! " + (angle - 90));
                    _sharpCount++;
                }
                else if (angle < 90 - _rangeForSquareAngle)
                {
                    print("ObtuseAngle! " + (angle + 90));
                    _obtuseCount++;
                }
                
                if (_squareCount == 3 && Vector3.Distance(_inputHandler.Points[0],
                    _inputHandler.Points[_inputHandler.Points.Count - 1]) < _maxDistanceForSquare)
                {
                    print("It is Square!");
                } 
                
                else if (_sharpCount == 2 && Vector3.Distance(_inputHandler.Points[0],
                    _inputHandler.Points[_inputHandler.Points.Count - 1]) < _maxDistanceForTriangle)
                {
                    print("It is Triangle!");
                } 
                
                else if (_obtuseCount >= 5 && Vector3.Distance(_inputHandler.Points[0],
                    _inputHandler.Points[_inputHandler.Points.Count - 1]) < _maxDistanceForCircle)
                {
                    print("It is Circle!");
                }
                
                else if (_sharpCount == 2 && Vector3.Distance(_inputHandler.Points[0],
                    _inputHandler.Points[_inputHandler.Points.Count - 1]) > _minDistanceForZ)
                {
                    print("It is definitely Z!");
                }
            };

            _inputHandler.OnClear += () =>
            {
                _lineRenderer.positionCount = 0;
                _squareCount = 0;
                _sharpCount = 0;
                _obtuseCount = 0;
            };
        }
    }
}