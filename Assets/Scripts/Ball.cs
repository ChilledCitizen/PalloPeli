using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{

    
    public float jumpforce;
    public float dmgKerroin = 5;
    float timer;
    public float hp;
    float jumpCount;
    public float maxHeight;
    private float highPoint;
    private bool goingDown;
    private Rigidbody2D playerRB;
    public float minForce = 1;
    float damage;
    GUIStyle myStyle = new GUIStyle();
    public bool isDed;
    public int sceneNum = 0;
    public Vector2 ballPos;
    public Image hpBar;
    public int maxHP = 100;
    float lastHP;
    public float t;
    public float totaltime;
    [HideInInspector]
    public GameObject controller;
   
    
   // GameObject[] floors;
    
    
    // Use this for initialization
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        //floors = GameObject.FindGameObjectsWithTag("Floor");
        myStyle.normal.textColor = Color.red;
        myStyle.fontSize = 15;
        sceneNum = SceneManager.GetActiveScene().buildIndex;
        maxHP = (int)hp;
        lastHP = hp;
        controller = GameObject.FindWithTag("Controller");

        
        
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        ballPos = transform.position;



        if (Input.GetMouseButton(0) )
        {
            if (jumpforce <= maxHeight)
            {
                jumpforce += Time.deltaTime * 10f;
                Debug.Log("Mouse down force" + jumpforce);
            }
            else
            {
                Debug.Log("Full power");
            }
        }

        if (Input.GetMouseButtonUp(0) && jumpCount == 0)
        {
            jumpCount = 1;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            Vector2 dir = (mousePos - ballPos)*0.25f ;
            dir.y = 1;
            
            

            BallJump(jumpforce, dir);
            

        }

        if (playerRB.velocity.y <= 0 && goingDown == false)
        {
            goingDown = true;
            highPoint = gameObject.transform.position.y;

        }

       /* if (transform.position.y < highPoint)
        {
            foreach (GameObject i in floors)
            {
                i.GetComponent<Collider>().isTrigger = false;
            }
        }*/
        
        if (t < totaltime)
        {
            t += Time.deltaTime;
        }
        else
        {
            t = totaltime;
        }

        hpBar.fillAmount = Mathf.Lerp(lastHP/maxHP, hp/maxHP, t / totaltime);

    }

    void BallJump(float jumpForce, Vector2 dir)
    {
        if (jumpForce < minForce)
        {
            jumpForce = minForce;
        }

        playerRB.AddForce(dir*jumpForce, ForceMode2D.Impulse);
        goingDown = false;
        jumpforce = 0;

      controller.GetComponent<GameController>().AddJump();
        

        /*foreach (GameObject i in floors)
        {
            i.GetComponent<Collider>().isTrigger = true;
        }*/

    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.tag == "Floor")
        {
            if (goingDown = true && highPoint > gameObject.transform.position.y)
            {
                damage = Mathf.Sqrt(dmgKerroin * Mathf.Abs(Physics.gravity.y) * (highPoint - gameObject.transform.position.y));
                TakeDamg(damage);
            }
           
            jumpCount = 0;
            
        }

        if (hit.gameObject.CompareTag("Obs"))
        {
            TakeDamg(hit.gameObject.GetComponent<Obstacle>().damage);
        }

    }

    void TakeDamg(float dmg)
    {
        t = 0;
        lastHP = hp;
        hp -= dmg;
       
        Debug.Log("Damage taken: " + dmg);
        damage = 0.0f;
        
        if (hp <= 0)
        {
            isDed = true;
            Die();
        }
    }

 void Die()
    {
        controller.GetComponent<GameController>().AddDeath();
        SceneManager.LoadScene("GameOver");
        
        
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10,10,200,80), "Jump Force : " + jumpforce.ToString("F1"), myStyle);
        GUI.Label(new Rect(10, 30, 200, 80), "Health : " + hp.ToString("F1"), myStyle);
        GUI.Label(new Rect(10, 50, 200, 80), "Damage : " + damage.ToString("F1"), myStyle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Goal")
        {
            SceneManager.LoadScene(sceneNum+1);
        }
    }



}
