using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _follow;
    [SerializeField] private float _time;
    [SerializeField] protected Vector2 _offset;

    // Update is called once per frame
    void Update()
    {
        var vec = Vector3.Lerp(transform.position, (Vector2)_follow.position + _offset, Time.deltaTime/_time);
        vec.z = transform.position.z;
        transform.position = vec;
    }
}
