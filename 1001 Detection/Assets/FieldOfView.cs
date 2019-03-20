using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)]
    public float ViewAngle;
    public float ViewRadius;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public Mesh ViewMesh;
    public MeshFilter ViewMeshFilter;

    public float MeshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;
    public float MaskCutawayDst;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool hit, Vector3 point, float dst, float angle)
        {
            this.hit = hit;
            this.point = point;
            this.dst = dst;
            this.angle = angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    void Start()
    {
        ViewMesh = new Mesh();
        ViewMesh.name = "View Mesh";
        ViewMeshFilter.mesh = ViewMesh;

        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    private void Update()
    {
        //DrawFieldOfView();
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    public Vector3 DirFromAngle(float angleInDegree, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegree += transform.eulerAngles.y;
        }

        // todo 벡터의 회전공식으로 회전시키기
        return new Vector3(Mathf.Sin(angleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegree * Mathf.Deg2Rad));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, targetMask);

        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // todo 벡터의 내적으로 각도 구하기
            if (Vector3.Angle(transform.forward, dirToTarget) < ViewAngle * 0.5)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(ViewAngle * MeshResolution);
        float stepAngleSize = ViewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - ViewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = CreateViewCastInfo(angle);
            //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * ViewRadius, Color.red);


            // 이 코드가 없어도 작동하지만 레이캐스트의 빈공간이 생긴다.
            // 레이캐스트 해상도를 높이면 어느정도 해결되지만 마냥 높일 수는 없다.
            // 다음 코드로 해상도가 좀 낮아도 빈공간을 최소화할 수 있다.
            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        // 각 포인트를 정점으로 생각한다.
        int vertexCount = viewPoints.Count + 1; // transfome.position을 추가한다.
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.forward * MaskCutawayDst;
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        ViewMesh.Clear();
        ViewMesh.vertices = vertices;
        ViewMesh.triangles = triangles;
        ViewMesh.RecalculateNormals();
    }

    ViewCastInfo CreateViewCastInfo(float angle)
    {
        RaycastHit raycastHit = new RaycastHit();
        Vector3 dir = DirFromAngle(angle, true);
        bool isHit = Physics.Raycast(transform.position, dir, out raycastHit, ViewRadius, obstacleMask);
        if (isHit)
        {
            return new ViewCastInfo(isHit, raycastHit.point, raycastHit.distance, angle);
        }else
        {
            return new ViewCastInfo(isHit, transform.position + dir * ViewRadius, ViewRadius, angle);
        }
    }

    // 레이캐스트의 빈공간을 채워준다.
    // 레이캐스트의 해상도가 좀 낮아도 보완한다.
    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = CreateViewCastInfo(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }
}
