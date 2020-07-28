using System.Collections;
using System.Collections.Generic;
using TowerDefense.Runtime;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField] float timeBeetweenAttack;
    [SerializeField] float attackRadius;
    [SerializeField] Projectile projectile;
    Enemy targetEnemy = null;
    float attackCounter;
    bool isAttack = false;
    void Start()
    {
        
    }
    void Update()
    {
        attackCounter -= Time.deltaTime;
        if(targetEnemy == null || targetEnemy.IsDead)
        {
            Enemy nearestenemy = GetNearestEnemy();
            if(nearestenemy != null && Vector2.Distance(transform.localPosition, nearestenemy.transform.localPosition) <= attackRadius)
            {
                targetEnemy = nearestenemy;
            }
        }
        else
        {
            if(attackCounter <=0)
            {
                isAttack = true;
                attackCounter = timeBeetweenAttack;
            }
            else
            {
                isAttack = false;
            }
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
            {
                targetEnemy = null;
            }
        }
        
    }
    public void FixedUpdate()
    {
        if(isAttack == true)
        {
            Attack();
        }
    }
    public void Attack()
    {
        isAttack = false;
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.localPosition = transform.localPosition;
        if(targetEnemy == null)
        {
            Destroy(newProjectile);
        }
        else
        {
            //move projectile to enemy
            StartCoroutine(MoveProjectile(newProjectile));
        }
    }
    IEnumerator MoveProjectile(Projectile projectile)
    {
        while(GetTargetDistance(targetEnemy) > 0.20f && projectile != null && targetEnemy != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }
        if(projectile !=null || targetEnemy == null)
        {
            Destroy(projectile);
        }
    }
    private float GetTargetDistance(Enemy thisEnemy)
    {
        if(thisEnemy == null)
        {
            thisEnemy = GetNearestEnemy();
            if(thisEnemy == null)
            {
                return 0f;
            }
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }
    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach(Enemy enemy in Manager.Instance.EnemyList)
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
    private Enemy GetNearestEnemy()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach(Enemy enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }
}
