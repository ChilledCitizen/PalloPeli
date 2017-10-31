using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Data;
using System.IO;
using System;

public class GameController : MonoBehaviour {

    public Scene x;
    public GameObject Player;
    Scene curScene;
    string sceneName;
    public int sceneNum = 0;

    [HideInInspector]
    public string hostname = "localhost";
    [HideInInspector]
    public string db_name = "pallopeli";
    [HideInInspector]
    public string user_id = "root";
    [HideInInspector]
    public string password = "ah4vueoM";
    [HideInInspector]
    public const string savePath = "C:/Users/crkku/Documents/Koulu_24.8.2017/Saves/save1.txt";

    int jumpcount = 0;
    int trycount = 0,deathcount = 0, lastTry;
    bool alreadySaved;

    string get_con_str()
    {
        string constr =
            "Server=" + hostname +
            ";Database=" + db_name +
            ";User ID=" + user_id +
            ";Password=" + password +
            ";Pooling=true";
        return constr;
    }

    // Esimerkkikyselyn tekeminen (tulostus: Debug.Log).
    void Awake()
    {
        
        alreadySaved = false;
        curScene = SceneManager.GetActiveScene();
        sceneName = curScene.name;
        sceneNum = curScene.buildIndex;



        if (!File.Exists(savePath))
            {
                File.Create(savePath);
            }
        if (sceneName != "GameOver")
        {

            TextReader tr = new StreamReader(savePath);

            string linefromfile = tr.ReadLine();

            tr.Close();

            trycount = Int32.Parse(linefromfile);
            lastTry = trycount;
            trycount += 1;

            // Luodaan ja avataan tietokantayhteys.
            MySqlConnection con = new MySqlConnection(get_con_str());
            con.Open();
            Debug.Log("Connection State: " + con.State);

        // Suoritetaan SQL-kysely.
       
            MySqlCommand cmd = new MySqlCommand("insert into pallo values (0, 0," + (sceneNum + 1) + ", " + trycount + ")", con);
            MySqlDataReader reader = cmd.ExecuteReader();

            // Käydään läpi vastaustietueet.
            while (reader.Read())
            {
                // Tulostetaan tietueen kaikki tietueen kentät.
                string line = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    line += reader.GetValue(i);
                    line += " | ";
                }
                Debug.Log(line);
            }
            reader.Close();
            // Suljetaan tietokantayhteys.
            Debug.Log("Closing connection");

            con.Close();
            con.Dispose();
        }

           
        

    }


    

    public void AddJump()
    {
        jumpcount++;

        MySqlConnection con = new MySqlConnection(get_con_str());
        con.Open();
        Debug.Log("Connection State: " + con.State);


        MySqlCommand cmd = new MySqlCommand("update pallo set pomput = " + jumpcount + " where yritys = " + trycount, con);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            // Tulostetaan tietueen kaikki tietueen kentät.
            string line = "";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                line += reader.GetValue(i);
                line += " | ";
            }
            Debug.Log(line);
        }

        // Suljetaan tietokantayhteys.
        Debug.Log("Closing connection");
        reader.Close();
        con.Close();
        con.Dispose();

    }

    public void AddDeath()
    {
        deathcount++;

        MySqlConnection con = new MySqlConnection(get_con_str());
        con.Open();
        Debug.Log("Connection State: " + con.State);


        MySqlCommand cmd = new MySqlCommand("update pallo set kuolemat = " + deathcount + " where yritys = " + trycount, con);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            // Tulostetaan tietueen kaikki tietueen kentät.
            string line = "";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                line += reader.GetValue(i);
                line += " | ";
            }
            Debug.Log(line);
        }

        // Suljetaan tietokantayhteys.
        Debug.Log("Closing connection");
        reader.Close();
        con.Close();
        con.Dispose();
    }

    // Use this for initialization
    void Start () {
        Player = GameObject.FindWithTag("Player");
        /*curScene = SceneManager.GetActiveScene();
        sceneName = curScene.name;
        */
	}
	
	// Update is called once per frame
	void Update () {

        if (sceneName != "GameOver")
        {
            sceneNum = curScene.buildIndex;
        }



        if (sceneName == "GameOver")
        {

            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(sceneNum);
            }

        }

        if (trycount == lastTry)
        {
            trycount++;
        }
        
        if (trycount != lastTry && !alreadySaved)
        {
            TextWriter tw = new StreamWriter(savePath);

            tw.Write(trycount);

            tw.Close();
            alreadySaved = true;
            
        }
        





	}
}
