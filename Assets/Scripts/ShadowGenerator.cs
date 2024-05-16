using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dogabeey
{
    [ExecuteAlways]
    public class ShadowGenerator : MonoBehaviour
    {
        public Collider2D selfCollider;
        [Tooltip("Light source's transform.")]
        public Transform lightSource;
        public Transform[] cornerPoints;
        [Tooltip("Distance of shadow.")]
        public float shadowDistance;
        [Tooltip("Shadow sprite")]
        public MeshFilter shadowMesh;
        public PolygonCollider2D shadowCollider;
        public float shadowZOffset = 0.1f;


        Vector3 lastKnownCornerPos1;
        Vector3 lastKnownCornerPos2;
        Vector3 lastKnownCornerPos3;
        Vector3 lastKnownCornerPos4;
        Vector3 lastKnownLightPosition;
        Vector3 lastKnownPosition;
        Vector3 lastKnownAngles;
        Vector3[] vertices;
        Mesh mesh;

        // Calculate the angle between a line that consists of an origin and end point, and a point.
        float CalculateSlope(Vector2 origin, Vector2 end, Vector2 point)
        {
            // Calculate the direction vector from origin to end
            Vector2 direction = end - origin;

            // Calculate the direction vector from origin to point
            Vector2 pointDirection = point - origin;

            // Calculate the angle between the two vectors
            float angle = Vector2.SignedAngle(direction, pointDirection);

            // Return the angle
            return angle;
        }
        // Based on transform postion as origin, light source as direction, and corner points as points, calculate the point with minimum & maximum determinant and return these as out parameter.
        void CalculateMinMaxDeterminant(out Vector2 minDeterminantPoint, out Vector2 maxDeterminantPoint)
        {
            Vector2 origin = lightSource.position;
            Vector2 direction = transform.position;

            Vector2[] points = cornerPoints.ToVector2Array();

            Vector2 minPoint = points[0];
            Vector2 maxPoint = points[0];

            float minDeterminant = CalculateSlope(origin, direction, points[0]);
            float maxDeterminant = CalculateSlope(origin, direction, points[0]);

            for (int i = 1; i < points.Length; i++)
            {
                float determinant = CalculateSlope(origin, direction, points[i]);

                if (determinant < minDeterminant)
                {
                    minDeterminant = determinant;
                    minPoint = points[i];
                }

                if (determinant > maxDeterminant)
                {
                    maxDeterminant = determinant;
                    maxPoint = points[i];
                }
            }

            minDeterminantPoint = minPoint;
            maxDeterminantPoint = maxPoint;
        }

        // Based on a line that consists of an origin and end point; calculate an extended end point that is originated from this line.
        Vector2 ExtendLine(Vector2 A, Vector2 B, float X)
        {
            // Calculate the direction vector from A to B
            Vector2 direction = B - A;

            // Normalize the direction vector to get a unit vector
            Vector2 unitDirection = direction.normalized;

            // Scale the unit vector by X
            Vector2 scaledVector = unitDirection * X;

            // Calculate the new end point C
            Vector2 C = B + scaledVector;

            // Return the new end point
            return C;
        }

        // Calculate the shadow's corner points based on the minimum & maximum determinant points, and the shadow distance.
        Vector2[] CalculateShadowcornerPoints(Vector2 minDeterminantPoint, Vector2 maxDeterminantPoint)
        {
            Vector2[] points = new Vector2[4];

            points[0] = minDeterminantPoint;
            points[1] = maxDeterminantPoint;
            points[2] = ExtendLine(lightSource.position, maxDeterminantPoint, shadowDistance);
            points[3] = ExtendLine(lightSource.position, minDeterminantPoint, shadowDistance);

            return points;
        }
        Mesh CreateMesh(Vector2[] points, Transform transform)
        {
            DestroyImmediate(mesh);

            mesh = new Mesh();

            // Update here: Create vertices for both top and bottom faces
            vertices = new Vector3[points.Length * 2]; // Doubled for top and bottom faces
            for (int i = 0; i < points.Length; i++)
            {
                // Top face vertices
                vertices[i] = transform.InverseTransformPoint(new Vector3(points[i].x, points[i].y, transform.position.z));
                // Bottom face vertices (extruded downwards by shadowZOffset)
                vertices[i + points.Length] = transform.InverseTransformPoint(new Vector3(points[i].x, points[i].y, transform.position.z - shadowZOffset));
            }

            // Update here: Create triangles for both faces and the sides
            var triangles = new List<int>();

            // Top face
            triangles.AddRange(new int[] { 0, 1, 2, 2, 3, 0 });

            // Bottom face
            int offset = points.Length;
            triangles.AddRange(new int[] { offset + 0, offset + 2, offset + 1, offset + 2, offset + 0, offset + 3 });

            // Side faces
            for (int i = 0; i < points.Length; i++)
            {
                int next = (i + 1) % points.Length;
                triangles.AddRange(new int[] { i, next, offset + i });
                triangles.AddRange(new int[] { next, offset + next, offset + i });
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
        private void Start()
        {
        }

        private void CalculateZPos()
        {
            // Set shadow mesh's z position to zPos
            transform.position = new Vector3(transform.position.x, transform.position.y, lightSource.transform.position.z);
            shadowMesh.transform.position = transform.position + (Vector3.forward * 0.05f);
            shadowMesh.transform.localScale = Vector3.one * 0.99f;
        }

        
        [ExecuteAlways]
        private void Update()
        {
            //Check if object's last position, angles or light s ource's position changed. If it did, generate shadow.
            if (lastKnownPosition != transform.position
                || lastKnownAngles != transform.eulerAngles
                || lastKnownLightPosition != lightSource.transform.position
                || lastKnownCornerPos1 != cornerPoints[0].position
                || lastKnownCornerPos2 != cornerPoints[1].position
                || lastKnownCornerPos3 != cornerPoints[2].position
                || lastKnownCornerPos4 != cornerPoints[3].position
                )
            {
                GenerateShadow();
                CalculateZPos();
            }

            //Update last known positions
            lastKnownPosition = transform.position;
            lastKnownAngles = transform.eulerAngles;
            lastKnownLightPosition = lightSource.transform.position;
            lastKnownCornerPos1 = cornerPoints[0].position;
            lastKnownCornerPos2 = cornerPoints[1].position;
            lastKnownCornerPos3 = cornerPoints[2].position;
            lastKnownCornerPos4 = cornerPoints[3].position;
        }
        private void GenerateShadow()
        {
            Vector2 minDeterminantPoint, maxDeterminantPoint;
            CalculateMinMaxDeterminant(out minDeterminantPoint, out maxDeterminantPoint);

            Vector2[] points = CalculateShadowcornerPoints(minDeterminantPoint, maxDeterminantPoint);
            shadowMesh.mesh = CreateMesh(points, transform);

                // Assuming shadowMesh.sharedMesh.vertices is correctly transformed and represents the 2D shape:
                Vector2[] colliderPoints = new Vector2[shadowMesh.mesh.vertexCount / 2]; // Assuming the bottom face corresponds to the 2D shape
                for (int i = 0; i < colliderPoints.Length; i++)
                {
                    Vector3 vertex = shadowMesh.mesh.vertices[i]; // Adjust index based on your mesh's vertex order
                    colliderPoints[i] = new Vector2(vertex.x, vertex.y);
                }
                shadowCollider.points = colliderPoints;
        }
    }
}