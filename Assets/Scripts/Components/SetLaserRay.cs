using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLaserRay : DraggedObject
{
    [SerializeField]
    private GameObject coordinateSphere;
    [SerializeField]
    private Transform startOfRayPoint;
    [SerializeField]
    private Transform[] positionsOfSphere;

    private LineRenderer lineRenderer;
    private float timeOut = 6f;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        lineRenderer.enabled = true;
        
        coordinateSphere.SetActive(true);
        coordinateSphere.transform.position = positionsOfSphere[0].position;
        SetRay();

        StartCoroutine(MoveRay());
    }

    private void SetRay()
    {
        lineRenderer.SetPosition(0, startOfRayPoint.position);
        lineRenderer.SetPosition(1, coordinateSphere.transform.position);
    }

    IEnumerator MoveRay()
    {
        yield return new WaitForSeconds(timeOut);

        for (int i = 1; i < 3; i++)
        {
            coordinateSphere.transform.position = positionsOfSphere[i].position;
            SetRay();
            yield return new WaitForSeconds(timeOut);

        }

        lineRenderer.enabled = false;
        coordinateSphere.SetActive(false);
        if (trainingDropManager.gameObject.activeSelf)
            trainingDropManager.EndActionsWithDraggedObj();
        else
            checkDropManager.EndActionsWithDraggedObj();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
