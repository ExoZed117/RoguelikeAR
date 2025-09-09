using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class EnvironmentGenerator : MonoBehaviour
{
    void Start()
    {
        Generate();
    }

    [Header("Prefabs")]
    public GameObject[] prefabs;      // >10 prefabs
    public int instancesPerPrefab = 3; // cuántas veces se genera cada objeto

    [Header("Area")]
    public Vector2 areaSize = new Vector2(50f, 50f);
    public float minY = 0f;
    public LayerMask groundMask;

    [Header("Placement rules")]
    public float minDistance = 2f;       // distancia mínima entre objetos
    public bool alignToNormal = true;
    public Vector2 scaleRange = new Vector2(1f, 1f);

    [Header("Cleanup")]
    public bool clearBeforeGenerate = true;
    public string generatedParentName = "GeneratedObjects";

    private List<Vector3> occupiedPositions = new List<Vector3>();

    public void Generate()
    {
        if (prefabs == null || prefabs.Length == 0) return;

        // Crear parent
        Transform parent = transform.Find(generatedParentName);
        if (parent == null)
        {
            GameObject p = new GameObject(generatedParentName);
            p.transform.SetParent(transform);
            parent = p.transform;
        }
        else if (clearBeforeGenerate)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
                DestroyImmediate(parent.GetChild(i).gameObject);
            occupiedPositions.Clear();
        }

        int totalSpawned = 0;

        foreach (GameObject prefab in prefabs)
        {
            int spawnedForPrefab = 0;
            int attempts = 0;
            while (spawnedForPrefab < instancesPerPrefab && attempts < 100)
            {
                attempts++;
                Vector3 candidate = RandomPointInArea();

                // raycast al suelo
                RaycastHit hit;
                Vector3 rayStart = new Vector3(candidate.x, minY + 200f, candidate.z);
                if (Physics.Raycast(rayStart, Vector3.down, out hit, 400f, groundMask))
                {
                    Vector3 finalPos = hit.point;

                    // comprobar distancia mínima
                    if (!IsTooClose(finalPos))
                    {
                        GameObject instance = Instantiate(prefab);
                        instance.transform.position = finalPos;

                        // escala aleatoria
                        float s = Random.Range(scaleRange.x, scaleRange.y);
                        instance.transform.localScale *= s;

                        // rotación
                        if (alignToNormal)
                        {
                            Quaternion q = Quaternion.FromToRotation(Vector3.up, hit.normal);
                            float yaw = Random.Range(0f, 360f);
                            instance.transform.rotation = q * Quaternion.Euler(0, yaw, 0);
                        }
                        else
                        {
                            instance.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                        }

                        instance.transform.SetParent(parent, true);
                        occupiedPositions.Add(finalPos);

                        Debug.Log($"Spawned {prefab.name} at {finalPos}");
                        spawnedForPrefab++;
                        totalSpawned++;
                    }
                }
            }

            if (spawnedForPrefab < instancesPerPrefab)
            {
                Debug.LogWarning($"Could not place all {instancesPerPrefab} instances of {prefab.name}");
            }
        }

        Debug.Log($"EnvironmentGenerator: spawned {totalSpawned} objects in total.");
    }

    Vector3 RandomPointInArea()
    {
        float halfX = areaSize.x * 0.5f;
        float halfZ = areaSize.y * 0.5f;
        float x = transform.position.x + Random.Range(-halfX, halfX);
        float z = transform.position.z + Random.Range(-halfZ, halfZ);
        return new Vector3(x, 0f, z);
    }

    bool IsTooClose(Vector3 pos)
    {
        float sqMin = minDistance * minDistance;
        foreach (Vector3 p in occupiedPositions)
        {
            if ((p - pos).sqrMagnitude < sqMin) return true;
        }
        return false;
    }
}
