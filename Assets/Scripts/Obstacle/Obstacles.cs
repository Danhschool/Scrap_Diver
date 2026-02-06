using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public Vector3 position;
    public Vector3 rotation;

    private GamePlayManager gamePlayManager;

    //[SerializeField] private float destroyHeight = 50f;
    private float timeToDestroy = 4f;

    // Start is called before the first frame update
    protected virtual void OnEnable()
    {
        Invoke("Destroy", timeToDestroy);
        gamePlayManager = GamePlayManager.instance;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Up();
    }

    protected virtual void OnDisable()
    {
        CancelInvoke();
    }

    public virtual void SetPosition(Vector3 _position)
    {
        transform.position = _position;
    }
    public virtual void SetRotation(Vector3 _rotation)
    {
        transform.eulerAngles = _rotation;
    }
    private void Up()
    {
        transform.Translate(Vector3.up * gamePlayManager.GameSpeed * Time.deltaTime, Space.World);

    }

    private void Destroy()
    {
        ObjectPool.instance.ReturnObject(gameObject);
    }
}
