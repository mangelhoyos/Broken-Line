using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendering : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public Transform followTransform;
    public GameObject colliderGameObject;

    private List<Vector3> linePositions;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        linePositions = new List<Vector3>();

        AssignColliderToObject();
        
        StartCoroutine(CreateLine());
    }
    
    private MeshCollider meshCollider;
    private Mesh mesh;
    private void AssignColliderToObject()
    {
        meshCollider = colliderGameObject.AddComponent<MeshCollider>();
        mesh = new Mesh();
    }

    IEnumerator CreateLine()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateColliders();
        linePositions.Add(followTransform.localPosition);
        _lineRenderer.positionCount = linePositions.Count;
        _lineRenderer.SetPositions(linePositions.ToArray());
        StartCoroutine(CreateLine());
    }

    void UpdateColliders()
    {
        if (LineMovement.Instance.initialized)
        {
            _lineRenderer.BakeMesh(mesh, true);
            meshCollider.sharedMesh = mesh;
        }
    }
    
}
