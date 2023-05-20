using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolowPlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private float _speed = 10.0f;

    private void Update()
    {
        if (_camera == null) {
            SetCamera();
        }
        SetRotate(gameObject, _camera);
    }

    void SetRotate(GameObject toRotate, GameObject camera)
    {
        transform.rotation = Quaternion.Lerp(toRotate.transform.rotation, camera.transform.rotation, _speed * Time.deltaTime);
    }

    public void SetCamera()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
}
