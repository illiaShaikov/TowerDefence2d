using UnityEngine;
namespace TowerDefense.Runtime
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float m_speed = default;
        [SerializeField] private Path m_path = default;
        [SerializeField] private int m_health = default;
        [SerializeField] private int m_rewardAmount = default;
        private Vector3 m_start = default;
        private Vector3 m_end = default;
        private float m_interpolate = 0f;
        private int m_pathIndex = 0;
        bool isDead = false;

        Collider2D enemyCollider;
        Animator anim;
        public bool IsDead
        {
            get
            {
                return isDead;
            }
        }
        // Start is called before the first frame update
        private void Start()
        {
            m_start = transform.position;
            m_end = m_path.GetPoint(m_pathIndex).position;
            Manager.Instance.RegisterEnemy(this);
            enemyCollider = GetComponent<Collider2D>();
            anim = GetComponent<Animator>();
        }
        // Update is called once per frame
        private void Update()
        {
            m_interpolate += Time.deltaTime * m_speed / Vector3.Distance(m_start, m_end);
            transform.position = Vector3.Lerp(m_start, m_end, m_interpolate);
            if (m_interpolate >= 1 && isDead == false)
            {
                m_pathIndex++;
                if (m_pathIndex >= m_path.Length)
                {
                    Debug.Log("Moving finished!");
                    enabled = false;
                    //Destroy(gameObject);
                    Manager.Instance.RoundEscaped += 1;
                    Manager.Instance.TotalEscaped += 1;
                    Manager.Instance.UnregisterEnemy(this);
                    Manager.Instance.IsWaveOver();
                    return;
                }
                m_start = transform.position;
                m_end = m_path.GetPoint(m_pathIndex).position;
                m_interpolate = 0;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Projectile")
            {
                Projectile newP = collision.gameObject.GetComponent<Projectile>();
                EnemyHit(newP.AttackDamage);
                Destroy(collision.gameObject);
            }
        }
        public void EnemyHit(int hit)
        {
            if(m_health - hit > 0)
            {
                m_health -= hit;
                anim.Play("Hurt");
            }
            else
            {
                anim.SetTrigger("IsDie");
                Die();
            }           
        }
        public void Die()
        {
            isDead = true;
            enemyCollider.enabled = false;
            Manager.Instance.TotalKilled += 1;
            Manager.Instance.AddMoney(m_rewardAmount);
            Manager.Instance.IsWaveOver();
        }
    }
}


