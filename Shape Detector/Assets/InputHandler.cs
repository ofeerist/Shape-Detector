using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private float _interval;
    [SerializeField] private float _minValueToExtend;
    [SerializeField] private Camera _camera;
    public List<Vector3> Points = new List<Vector3>();

    public delegate void Added(Vector2 current, Vector2 previous);

    public event Added OnAdd;
    public event Action OnClear;
    private void Start()
    {
        Observable.Interval(TimeSpan.FromSeconds(_interval)).Subscribe(x =>
        {
            if (!Input.GetMouseButton(0)) return;
            
            var point = _camera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            if (Points.Count > 0 && Vector3.Distance(Points[Points.Count - 1], point) < _minValueToExtend) return;
            
            Points.Add(point);
            OnAdd?.Invoke(Points.Count <= 2 ? Vector3.zero : point - Points[Points.Count - 2],
                Points.Count <= 3 ? Vector3.zero : Points[Points.Count - 2] - Points[Points.Count - 3]);
            
        }).AddTo(this);
        
        Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)).Subscribe(x =>
        {
            Points.Clear();
            OnClear?.Invoke();
        });
    }
}
