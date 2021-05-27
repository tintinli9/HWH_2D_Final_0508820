using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int lv = 1;
    [Range(0, 15)]
    public float speed = 10.5f;
    public bool isDEAD = false;
    public string cNAME = "主角";
    [Header("虛擬搖桿")]
    public FixedJoystick joystick;
    [Header("變形元件")]
    public Transform tra;
    [Header("動畫元件")]
    public Animator ani;
    [Header("偵測範圍")]
    public float rangeAttack = 1.5f;
    [Header("音效來源")]
    public AudioSource aud;
    [Header("攻擊音效")]
    public AudioClip soundAttack;
    [Header("攻擊力"), Range(0, 100)]
    public float attack = 20;
    [Header("血量")]
    public float hp = 200;
    [Header("血條系統")]
    public HpManager hpManager;

    private float hpMax;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }

    private void Move()
    {
        if (isDEAD) return;

        float h = joystick.Horizontal;

        tra.Translate(h * speed * Time.deltaTime, 0, 0);

        ani.SetFloat("水平", h);
    }

    public void Attack()
    {
        if (isDEAD) return;

        aud.PlayOneShot(soundAttack, 0.3f);

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, rangeAttack, -transform.up, 0, 1 << 8);
        if (hit && hit.collider.tag == "敵人") hit.collider.GetComponent<Enemy>().Hit(attack);
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
        Invoke("Replay",2);
        ani.SetTrigger("死亡");
    }

    private void Replay()
    {
        SceneManager.LoadScene("遊戲場景");
    }

    private void Start()
    {
        hpMax = hp;
    }

    private void Update()
    {
        Move();
    }

    public void Test()
    {
        ani.SetTrigger("攻擊");
    }
}

