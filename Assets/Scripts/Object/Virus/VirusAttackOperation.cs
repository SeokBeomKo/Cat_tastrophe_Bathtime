using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusAttackOperation : MonoBehaviour, IAttackable, IDamageable
{
    [Header("HP")]
    public float HP = 5;
    public ObjectHPbar objectHPbar;
 
    [Header("플레이어 감지 범위")]
    public float radius = 0.1f;
    
    [Header("유한 상태 기계")]
    [SerializeField] public VirusStateMachine virusMachine;

    [Header("모델링 정보")]
    public GameObject model;
    public GameObject explosionVFX;

    public Vector3 PlayerPosition;
    public GameObject ProjectilePrefab;

    public delegate void VirusRespawnHandle();
    public event VirusRespawnHandle OnRespawnTimerStart;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, radius);
    }

    void Update()
    {
        if (null != virusMachine.curState)
        {
            virusMachine.curState.Execute();
        }

    }

    void Start()
    {
        HP = 5;
        objectHPbar.SetHP(HP);
        objectHPbar.CheckHP();
    }

    private void OnEnable()
    {
        model.SetActive(true);       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            Hit(other.gameObject.GetComponentInChildren<IAttackable>().GetDamage());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            HP = 0;
            Check();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player HP--");
            GetDamage();
        }
    }


    private void Hit(float damage)
    {
        objectHPbar.Damage(damage);
        HP = objectHPbar.GetHP();
        Check();
    }

    private void Check()
    {
        if (HP <= 0)
        {
            model.SetActive(false);
            explosionVFX.SetActive(true);

            StartCoroutine(DestroyAfterParticles());
        }
    }

    private IEnumerator DestroyAfterParticles()
    {
        ParticleSystem ps = explosionVFX.GetComponent<ParticleSystem>();

        while (ps != null && ps.IsAlive())
        {
            yield return null;
        }

        explosionVFX.SetActive(false);
        OnRespawnTimerStart?.Invoke();
        transform.gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    public void BeAttacked(float damage)
    {
        Hit(damage);
    }

    public float GetDamage()
    {
        return 5;
    }
}
