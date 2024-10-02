using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] int hp;
    [SerializeField] GameObject hpBar;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject replayButton;
    GameObject currentFloor; 
    int score;
    float scoreTime;
    Animator anim;
    SpriteRenderer render;
    AudioSource deathSound;
    void Start()
    {
        hp = 10;
        score = 1;
        scoreTime = 0f;
        render = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        deathSound = GetComponent<AudioSource>();
        replayButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            render.flipX = false;
            anim.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            render.flipX = true;
            anim.SetBool("Run", true);
        }
        else{
            anim.SetBool("Run", false);
        }
        UpdateScore();
    }
    void OnCollisionEnter2D(Collision2D other) {
        //print("fuck");
        // other.contacts[0].normal 是碰撞到的兩點的法向量
        other.gameObject.GetComponent<AudioSource>().Play();
        if (other.gameObject.tag == "Normal"
         && other.contacts[0].normal == new Vector2(0f, 1f))
        {
            print("normal");
            currentFloor = other.gameObject; 
            modifyHp(1);
            
        }
        else if (other.gameObject.tag == "Nails"
              && other.contacts[0].normal == new Vector2(0f, 1f))
        {
            print("nails");
            currentFloor = other.gameObject; 
            modifyHp(-3);
            anim.SetTrigger("Hurt");
        }
        else if (other.gameObject.tag == "Ceiling")
        {
            print("ceiling");
            currentFloor.GetComponent<BoxCollider2D>().enabled = false; 
            modifyHp(-3);
            anim.SetTrigger("Hurt");
        }
        
    }
    void OnTriggerEnter2D(Collider2D other) {
        // box collider 2d的is trigger要打勾，會直接穿過去不會碰撞
        if (other.gameObject.tag == "DeathLine")
        {
            print("dead");
            Die();
        }
    }
    void modifyHp(int n){
        hp += n;
        if(hp >= 10)
            hp = 10;
        else if(hp <= 0)
        {
            hp = 0;
            Die();
        }
        UpdateHpBar();
    }
    void UpdateHpBar(){
        for(int i=0;i<hpBar.transform.childCount;i++){
            if (hp>i){
                hpBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                hpBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    void UpdateScore(){
        scoreTime += Time.deltaTime;
        if(scoreTime > 2f){
            scoreTime = 0;
            score++;
            scoreText.text = "地下" + score.ToString() + "層";
        }
    }
    void Die(){
        deathSound.Play();
        Time.timeScale = 0f;
        replayButton.SetActive(true);
    }
    public void Replay(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}
