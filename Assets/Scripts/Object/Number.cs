using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Number : AudioManager
{
    private Image bg;
    private Text number_text;

    private MyGrid inGrid;//这个数字所在的格子

    public NumberStatus status;

    private float spawnScaleTime = 1;
    private bool isPlayingSpawnAnim = false;

    private float mergeScaleTime = 1;
    private float mergeScaleTimeBack = 1;
    private bool isPlayingMergeAnim = false;

    private float movePosTime = 1;
    private bool isMoving = false;
    private bool isDestoryOnMoveEnd = false;
    private Vector3 startMovePos, endMovePos;

    public Color[] bg_colors;                       //格子颜色
    public List<int> number_index;                  //格子数字

    public AudioClip mergeClip;


    private void Awake()
    {
        bg = transform.GetComponent<Image>();
        number_text = transform.Find("Text").GetComponent<Text>();
    }


    //初始化
    public void Init(MyGrid myGrid)
    {
        myGrid.SetNumber(this);
        //设置所在的格子
        this.SetGrid(myGrid);
        //给他一个初始化的数字
        this.SetNumber(2);
        status = NumberStatus.Normal;

        //transform.localScale = Vector3.zero;
        PlaySpawnAnim();        //播放动画
    }
    //设置格子
    public void SetGrid(MyGrid myGrid)
    {
        this.inGrid = myGrid;
    }

    //获取格子
    public MyGrid GetGrid( )
    {
        return this.inGrid;
    }

    //设置数字
    public void SetNumber(int number)
    {
        this.number_text.text = number.ToString();
        this.bg.color = this.bg_colors[number_index.IndexOf(number)];
    }

    //获取数字
    public int GetNumber()
    {
        return int.Parse(number_text.text);
    }
        
    //把一个数字移动到某个格子的下面
    public void MoveToGrid(MyGrid myGrid)
    {
        transform.SetParent(myGrid.transform);
        //transform.localPosition = Vector3.zero;
        startMovePos = transform.localPosition;
        //endMovePos = myGrid.transform.position;

        movePosTime = 0;
        isMoving = true;

        this.GetGrid().SetNumber(null);

        //设置格子
        myGrid.SetNumber(this);
        this.SetGrid(myGrid);
    }


    //在移动结束时销毁
    public void DestoryOnMoveEnd(MyGrid myGrid)
    {
        transform.SetParent(myGrid.transform);
        startMovePos = transform.localPosition;

        movePosTime = 0;
        isMoving = true;
        isDestoryOnMoveEnd = true;
    }

    //合并
    public void Merge()
    {

        GamePanel gamePanel =GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();
        gamePanel.AddScore(this.GetNumber());

        int number = this.GetNumber() * 2;
        this.SetNumber(number);
        if(number==2048)
        {
            //游戏胜利
            gamePanel.GameWin();
        }
        status = NumberStatus.NotMerge;
        //播放合并动画
        PlayMergeAnim();
        //播放音效
        AudioManager._instance.PlaySound(mergeClip);
    }

    //判断能不能进行合并
    public bool IsMerge(Number number)
    {
        if (this.GetNumber() == number.GetNumber() && number.status == NumberStatus.Normal)
        {
            return true;
        }
        return false;
    }

    //播放创建动画
    public void PlaySpawnAnim()
    {
        spawnScaleTime = 0;
        isPlayingSpawnAnim = true;
    }

    //播放合并动画
    public void PlayMergeAnim()
    {
        mergeScaleTime = 0;
        mergeScaleTimeBack = 0;
        isPlayingMergeAnim = true;
    }

    public void Update()
    {
        //创建动画

        if (isPlayingSpawnAnim)
        {
            if (spawnScaleTime <= 1)
            {
                spawnScaleTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, spawnScaleTime);
            }
            else
            {
                isPlayingSpawnAnim = false;
            }
        }

        //合并动画

        if (isPlayingMergeAnim)
        {
            if (mergeScaleTime <= 1 && mergeScaleTimeBack == 0)    //变大的过程
            {
                mergeScaleTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, mergeScaleTime);
            }
            if (mergeScaleTime >= 1 && mergeScaleTimeBack <= 1)    //变小的过程
            {
                mergeScaleTimeBack += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, mergeScaleTimeBack);
            }
            if (mergeScaleTime >= 1 && mergeScaleTimeBack >= 1)
            {
                isPlayingMergeAnim = false;
            }
        }

        //移动动画
        if (isMoving)
        {
            movePosTime += Time.deltaTime * 5;
            transform.localPosition = Vector3.Lerp(startMovePos,Vector3.zero, movePosTime);
            //Debug.Log(" movePosTime :" + movePosTime + " pos " + Vector3.Lerp(startMovePos, Vector3.zero, movePosTime));
            if (movePosTime >= 1)
            {
                isMoving = false;
                if (isDestoryOnMoveEnd)
                {
                    GameObject.Destroy(this.gameObject);//destory暂时实现不了 
                }
            }
        }
    }
}
