using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BodyPartSensor : MonoBehaviour
{
    [SerializeField] protected bool isLimb;
    [SerializeField] protected GameObject mesh;
    [SerializeField] protected Collider skel;

    [SerializeField] protected float startAngle;
    public float limitAngle;
    protected Vector3 initialRotation;

    protected Transform m_transform;

    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] protected Player player;

    [Header("On hit")]
    protected Color hitColor = Color.white;
    protected float hitTime = 1f ;
    protected Renderer partRenderer;
    protected Material instanceMaterial;
    protected Color originalColor;
    protected Coroutine currentCoroutine;

    protected virtual void Start()
    {
        player = GetComponentInParent<Player>();

        m_transform = transform;

        initialRotation = m_transform.localEulerAngles;

        partRenderer = mesh.GetComponent<Renderer>();
        instanceMaterial = partRenderer.material;
        instanceMaterial.EnableKeyword("_EMISSION");
        originalColor = instanceMaterial.GetColor("_Emission");

        hitColor = Color.white * 5f;

    }
    public virtual void RotateLimb_2()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Obstacle")) return;

        if (player == null)
            player = GetComponentInParent<Player>();

        if (player.limb == null)            
            return;

        OnHit();
        player.limb.GetHit(isLimb, mesh, skel);
    }

    protected virtual void OnHit()
    {
        if(instanceMaterial == null) return;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(HitEffect());
    }

    public IEnumerator HitEffect()
    {
        instanceMaterial.SetColor("_Emission", hitColor);

        CameraManager.instance.Shake();

        yield return new WaitForSeconds(hitTime);

        instanceMaterial.SetColor("_Emission", originalColor);

        currentCoroutine = null;
    }


    public void RotateArm( float _startAngle)
    {
        if (player == null) return;

        float targetZ = _startAngle;
        Direction currentDir = player.movement.CheckDirection();

        if (currentDir == Direction.left) targetZ = limitAngle;
        else if (currentDir == Direction.right) targetZ = -limitAngle;

        float currentZ = transform.localEulerAngles.z;
        float newDir = Mathf.LerpAngle(currentZ, targetZ, rotationSpeed * Time.deltaTime);

        Vector3 currentRotation = transform.localEulerAngles;

        transform.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, newDir);

    }
    public void RotateLeg(float _startAngle)
    {
        if (player == null) return;

        float targetY = _startAngle;
        Direction currentDir = player.movement.CheckDirection();

        if (currentDir == Direction.left) targetY = limitAngle;
        else if (currentDir == Direction.right) targetY = -limitAngle;

        float currentY = transform.localEulerAngles.y;
        float newDir = Mathf.LerpAngle(currentY, targetY, rotationSpeed * Time.deltaTime);

        Vector3 currentRotation = m_transform.localEulerAngles;

        transform.localRotation = Quaternion.Euler(currentRotation.x, newDir, currentRotation.z);

    }

}

