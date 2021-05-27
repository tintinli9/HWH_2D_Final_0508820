using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDEAD = false;
    [Header("追蹤範圍"), Range(0, 10)]
    public float rangeTrack = 4;
    [Header("攻擊範圍"), Range(0, 10)]
    public float rangeAttack = 2;
    [Header("移動速度"), Range(0, 15)]
    public float speed = 2;
    [Header("攻擊動畫")]
    public Animator ani;
    [Header("攻擊冷卻時間"), Range(0, 10)]
    public float cdAttack = 3;
    [Header("攻擊力"), Range(0, 100)]
    public float attack = 20;
    [Header("音效來源")]
    public AudioSource aud;
    [Header("攻擊音效")]
    public AudioClip soundAttack;
    [Header("血量")]
    public float hp = 200;
    [Header("血條系統")]
    public HpManager hpManager;

    private Transform player;
    private float timer;
    private float hpMax;

    private void Start()
    {
        player = GameObject.Find("玩家").transform;
        hpMax = hp;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawSphere(transform.position, rangeTrack);

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }

    private void Update()
    {
        Track();
    }

    private void Track()
    {
        if (isDEAD) return;

        float dis = Vector3.Distance(transform.position, player.position);

        if (dis <= rangeAttack)
        {
            Attack();
        }
        else if (dis <= rangeTrack)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    public void Attack()
    {
        timer += Time.deltaTime;

        if (timer >= cdAttack)
        {
            timer = 0;
            aud.PlayOneShot(soundAttack, 0.3f);
            Collider2D hit = Physics2D.OverlapCircle(transform.position, rangeAttack, 1 << 9 );
            hit.GetComponent<Player>().Hit(attack);
            ani.SetTrigger("攻擊");
        }
    }

    public void Hit(float damage)
    {
        hp -= damage;
        hpManager.UpdateHpbar(hp, hpMax);
        StartCoroutine(hpManager.ShowDamage(damage));

        if (hp <= 0) Dead();
    }

    private void Dead()
    {
        hp = 0;
        isDEAD = true;
        Destroy(gameObject, 2);
        ani.SetTrigger("死亡");
    }

}
