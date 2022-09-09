using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class Player : MonoBehaviour
{
    //--------- Arduino Connect --------------//
    public SerialPort sp = new SerialPort("COM6", 115200);
    public int CMD; //讀取接收Serial.write
    public float speed = 60;


    //--------- [SerializeField] ------------//
    [SerializeField] float moveSpeed = 2f; //player移動速度
    [SerializeField] int HP = 10;//腳色血量
    [SerializeField] GameObject HPbar ;//腳色血量條
    [SerializeField] Text scoretext; //分數計算
    [SerializeField] GameObject bt_Replay; //新增重新開始按鈕
    [SerializeField] GameObject bt_pause; //新增 遊戲開始/暫停 按鈕
    [SerializeField] GameObject bt_music; //新增 音樂開始/暫停 按鈕
    [SerializeField] GameObject bt_replay; //新增 重新開始 按鈕

    //---------GameObject---------//
    GameObject currentball;//偵測腳色與球體碰撞點
    GameObject currentcelling;//偵測腳色與尖刺碰撞點


    //---------宣告物件---------//
    int score;//初始化分數
    float scoretime;//紀錄時間
    bool stop = false; //遊戲開始/暫停
    bool music = false; // 音樂遊戲開始/暫停


    //---------簡化函式---------//
    Animator anim; //簡化 GetComponent<Animator>();
    SpriteRenderer sprite; //簡化 GetComponent<SpriteRenderer>();
    AudioSource my_music;
    

    // ------Start is called before the first frame update-------//
    void Start()
    {
        HP = 8; //初始化血量
        score = 0; //初始分數
        scoretime = 0f; //初始時間
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        my_music = GetComponent<AudioSource>();

        //arduino 連線
        sp.Open();
        sp.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //腳色移動
        // playermove();

        //更新分數
        updatescore();
        
        //連線
        sp_connect();

    }

    void playermove() //腳色移動
    {
        //鍵盤上下左右
        if(Input.GetKey(KeyCode.RightArrow))
        {
            //Debug.Log("RIGHT");
            transform.Translate(moveSpeed*Time.deltaTime, 0, 0);
            sprite.flipX = true;
            sprite.flipY = false;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            //Debug.Log("LEFT");
            transform.Translate(-moveSpeed*Time.deltaTime, 0, 0);
            sprite.flipX = false;
            sprite.flipY = false;
        }
        else if(Input.GetKey(KeyCode.UpArrow))
        {
            //Debug.Log("UP");
            transform.Translate(0,moveSpeed*Time.deltaTime,  0);
            sprite.flipY = false;
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            //Debug.Log("DOWN");
            transform.Translate(0,-moveSpeed*Time.deltaTime,  0);
            sprite.flipY = true;
        }
        else if(CMD == 4)
        {
            //Debug.Log("arduino left");
            transform.Translate(-speed*Time.deltaTime, 0, 0);
            sprite.flipX = false;
            sprite.flipY = false;
        }
        else if(CMD == 6)
        {
            //Debug.Log("arduino RIGHT");
            transform.Translate(speed*Time.deltaTime, 0, 0);
            sprite.flipX = true;
            sprite.flipY = false;
        }
        else if(CMD == 8)
        {
            //Debug.Log("arduino up");
            transform.Translate(0, speed*Time.deltaTime, 0);
            sprite.flipY = false;
        }
        else if(CMD == 2)
        {
            //Debug.Log("arduino down");
            transform.Translate(0, -speed*Time.deltaTime, 0);
            sprite.flipY = true;
        }
    }

    void sp_connect()  //arduino連線
    {
        if (sp.IsOpen)
        {
            try
            {   
                CMD = sp.ReadByte();
                playermove();
            }
            catch(System.Exception)
            {

            }
        }
        else
        {
           playermove(); 
        }
    }

  void OnCollisionEnter2D(Collision2D other) //Player 碰撞偵測
    {
        //紅色寶貝球偵測撞擊
        if(other.gameObject.tag == "r_ball3") 
        {
            //Debug.Log(other.contacts[0].normal);
            //Debug.Log(other.contacts[1].normal);
            Debug.Log("碰到紅色球");
            modifyhp(1);
            currentball = other.gameObject;               
            Destroy(currentball);           
        }

        //撞擊牆壁
        if(other.gameObject.tag == "wall_left" || other.gameObject.tag == "wall_right" || other.gameObject.tag == "wall_on")
        {
            Debug.Log("撞擊牆壁");
        }
        
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "deathline")
        {
            //Debug.Log("已掉下去");
        }
        //撞擊障礙物
        if(other.gameObject.tag == "celling")
        {
            modifyhp(-2);
            anim.SetTrigger("hurt");
            Debug.Log("撞到尖刺");
        }
    }

    void modifyhp(int num) //更新血量
    {
        HP += num;
        if(HP >= 8)
        {
            anim.SetBool("turn_into_1",false);
            HP = 8;
            
        }
        else if(HP<8 && HP >= 5)
        {
            anim.SetBool("turn_into_1",true);
            anim.SetBool("turn_into_2",false);
        }
        else if(HP< 5 && HP >= 1)
        {
            anim.SetBool("turn_into_2",true);
        }
        else if (HP<=0)
        {
            HP = 0;
            die();
        }
        updateHPbar();
    }

    void updateHPbar()  //更新小智
    {
        for(int i = 0; i < HPbar.transform.childCount ; i++)
        {
            if(HP>i)
            {
                HPbar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                HPbar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void updatescore() //更新分數
    {
        scoretime += Time.deltaTime;
        if(scoretime > 1f)
        {
            score++;
            scoretime = 0f;
            scoretext.text = "逃離小智: " + score.ToString() + " 秒 ";
        }
    }

    void die() //死亡偵測
    {
         Time.timeScale = 0f;
         bt_Replay.SetActive(true);
    }

    public void replay() //重新開始
    {
       Time.timeScale = 1f; 
       SceneManager.LoadScene("SampleScene");
    }

    public void pause()
    {
        if(stop == false)
        {
            Time.timeScale = 0f; 
            stop = true;
        }
        else
        {
            Time.timeScale = 1f; 
            stop = false;
        }

    }

    public void play_music()
    {
        if(music == false)
        {
            my_music.Stop();
            music = true;
        }
        else
        {
            my_music.Play();
            music = false;
        }

    }
}
