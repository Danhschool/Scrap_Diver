using UnityEngine;

[CreateAssetMenu(fileName = "NewPattern", menuName = "ScrapDivers/Spawn Pattern")]
public class SpawnPattern : ScriptableObject
{
    public enum SpawnType
    {
        RandomInCircle,
        FixedPosition,  
        OnWall          
    }

    public enum RotationMode
    {
        Fixed,
        RandomX,
        RandomY,
        RandomZ
    }

    [Header("1. Position info")]
    public SpawnType spawnType = SpawnType.FixedPosition;
    public float padding = 4f;
    public Vector3 positionOffset = Vector3.zero;

    [Tooltip("(Y) Random")]
    public float randomY = 15f;

    [Header("2. Rotation info")]
    public RotationMode rotationMode = RotationMode.Fixed;
    public Vector3 rotationCorrection = Vector3.zero;

    public Vector3 GetCalculatedPosition(float tunnelWidth)
    {
        float radius = (tunnelWidth / 2) - padding;
        Vector3 finalPos = Vector3.zero;

        switch (spawnType)
        {
            case SpawnType.RandomInCircle:
                Vector2 circle = Random.insideUnitCircle * radius;

                float randY = Random.Range(-randomY, randomY);
                finalPos = new Vector3(circle.x, randY, circle.y);
                break;

            case SpawnType.FixedPosition:
                finalPos = Vector3.zero;
                break;

            case SpawnType.OnWall:
                finalPos = new Vector3(-radius, 0, 0);
                break;
        }

        return finalPos + positionOffset;
    }

    public Quaternion GetCalculatedRotation()
    {
        Quaternion correction = Quaternion.Euler(rotationCorrection);
        Quaternion randomRot = Quaternion.identity;

        switch (rotationMode)
        {
            case RotationMode.RandomX: randomRot = Quaternion.Euler(Random.Range(0f, 360f), 0, 0); break;
            case RotationMode.RandomY: randomRot = Quaternion.Euler(0, Random.Range(0f, 360f), 0); break;
            case RotationMode.RandomZ: randomRot = Quaternion.Euler(0, 0, Random.Range(0f, 360f)); break;
        }
        return correction * randomRot;
    }
}