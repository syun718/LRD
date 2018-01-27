using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameContoller : MonoBehaviour
{
    [SerializeField]
    private GameObject[] playerInstance;
    [SerializeField]
    private GameObject[] camInstance;
    public float[,] dango_state = new float[4, 4];
    private float[] state_score = new float[4];
    private float[] goaldango = new float[4];
    public int getflagdango;

    // Use this for initialization
    void Start()
    {
        GenerateCameras();

        dango_state = new float[/*プレイヤー番号*/,/*そのプレイヤーの場所*/]{
            { 1, 1, 1, 0, 0},
            { 2, 1, 1, 0, 0},
            { 3, 1, 1, 0, 0},
            { 4, 1, 1, 0, 0},
            //{順位,現在のラップ数,次のフラグ番号,次のフラグまでの距離,距離のスコア}
        };
        state_score = new float[] { 0, 0, 0, 0 };
        goaldango = new float[] { 0, 0, 0, 0 };
        getflagdango = 4;
    }

    void GenerateCameras()
    {
        for (int i = 0; i < SOSingleton.Instance.pNum; i++)
        {
            playerInstance[i].SetActive(true);
            camInstance[i].SetActive(true);
            var cam = camInstance[i].gameObject.GetComponent<Camera>();
            cam.rect = new Rect(SOSingleton.Instance.cameraPos[i], SOSingleton.Instance.cameraSize);
        }
    }

    private void Update()
    {
        for (int i = 1; i < SOSingleton.Instance.pNum + 1; i++)
        {
			GameObject dango = GameObject.Find("dango_motion" + i);
            GameObject flag = GameObject.Find("flag" + dango_state[i - 1, 2]);

            dango_state[i - 1, 3] = System.Math.Abs(
                                    (System.Math.Abs(flag.transform.position.x) + System.Math.Abs(flag.transform.position.z)) -
                                    (System.Math.Abs(dango.transform.position.x) + System.Math.Abs(dango.transform.position.z)));

            dango_state[i - 1, 4] = (dango_state[i - 1, 1] * 10000) + (dango_state[i - 1, 2] * 1000) - (dango_state[i - 1, 3]);
            state_score[i - 1] = dango_state[i - 1, 4];

			if(dango_state[i - 1, 1] == 3)
			{
                SOSingleton.Instance.pRank = i;

                goaldango[i] = 1;
                dango.gameObject.GetComponent<Player1>().Goal();
                if ((goaldango[0] + goaldango[1] + goaldango[2] + goaldango[3]) >= SOSingleton.Instance.pNum - 1){
                    SceneManager.LoadScene(4);
                }
			}
        }

        Array.Sort(state_score);

        for (int i = SOSingleton.Instance.pNum - 1; i >= 0; i--)
        {
            Debug.Log("1");
            for (int j = 0; j < SOSingleton.Instance.pNum; j++)
            {
                Debug.Log("2");
                if (dango_state[j, 4] == state_score[i])
                {
                    Debug.Log("3");
                    dango_state[j, 0] = i + 1;
                }
            }
        }

        if (getflagdango != 4)
        {
            dango_state[getflagdango, 2]++;
            if (dango_state[getflagdango, 2] > 7)
            {
                dango_state[getflagdango, 2] = 1;
                dango_state[getflagdango, 1]++;
            }
            getflagdango = 4;
        }
    }

    public void Dango1Input()
    {
        getflagdango = 0;
    }
    public void Dango2Input()
    {
        getflagdango = 1;
    }
    public void Dango3Input()
    {
        getflagdango = 2;
    }
    public void Dango4Input()
    {
        getflagdango = 3;
    }
}
