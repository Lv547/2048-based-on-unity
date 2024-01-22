using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Number : AudioManager
{
    private Image bg;
    private Text number_text;

    private MyGrid inGrid;//����������ڵĸ���

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

    public Color[] bg_colors;                       //������ɫ
    public List<int> number_index;                  //��������

    public AudioClip mergeClip;


    private void Awake()
    {
        bg = transform.GetComponent<Image>();
        number_text = transform.Find("Text").GetComponent<Text>();
    }


    //��ʼ��
    public void Init(MyGrid myGrid)
    {
        myGrid.SetNumber(this);
        //�������ڵĸ���
        this.SetGrid(myGrid);
        //����һ����ʼ��������
        this.SetNumber(2);
        status = NumberStatus.Normal;

        //transform.localScale = Vector3.zero;
        PlaySpawnAnim();        //���Ŷ���
    }
    //���ø���
    public void SetGrid(MyGrid myGrid)
    {
        this.inGrid = myGrid;
    }

    //��ȡ����
    public MyGrid GetGrid( )
    {
        return this.inGrid;
    }

    //��������
    public void SetNumber(int number)
    {
        this.number_text.text = number.ToString();
        this.bg.color = this.bg_colors[number_index.IndexOf(number)];
    }

    //��ȡ����
    public int GetNumber()
    {
        return int.Parse(number_text.text);
    }
        
    //��һ�������ƶ���ĳ�����ӵ�����
    public void MoveToGrid(MyGrid myGrid)
    {
        transform.SetParent(myGrid.transform);
        //transform.localPosition = Vector3.zero;
        startMovePos = transform.localPosition;
        //endMovePos = myGrid.transform.position;

        movePosTime = 0;
        isMoving = true;

        this.GetGrid().SetNumber(null);

        //���ø���
        myGrid.SetNumber(this);
        this.SetGrid(myGrid);
    }


    //���ƶ�����ʱ����
    public void DestoryOnMoveEnd(MyGrid myGrid)
    {
        transform.SetParent(myGrid.transform);
        startMovePos = transform.localPosition;

        movePosTime = 0;
        isMoving = true;
        isDestoryOnMoveEnd = true;
    }

    //�ϲ�
    public void Merge()
    {

        GamePanel gamePanel =GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();
        gamePanel.AddScore(this.GetNumber());

        int number = this.GetNumber() * 2;
        this.SetNumber(number);
        if(number==2048)
        {
            //��Ϸʤ��
            gamePanel.GameWin();
        }
        status = NumberStatus.NotMerge;
        //���źϲ�����
        PlayMergeAnim();
        //������Ч
        AudioManager._instance.PlaySound(mergeClip);
    }

    //�ж��ܲ��ܽ��кϲ�
    public bool IsMerge(Number number)
    {
        if (this.GetNumber() == number.GetNumber() && number.status == NumberStatus.Normal)
        {
            return true;
        }
        return false;
    }

    //���Ŵ�������
    public void PlaySpawnAnim()
    {
        spawnScaleTime = 0;
        isPlayingSpawnAnim = true;
    }

    //���źϲ�����
    public void PlayMergeAnim()
    {
        mergeScaleTime = 0;
        mergeScaleTimeBack = 0;
        isPlayingMergeAnim = true;
    }

    public void Update()
    {
        //��������

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

        //�ϲ�����

        if (isPlayingMergeAnim)
        {
            if (mergeScaleTime <= 1 && mergeScaleTimeBack == 0)    //���Ĺ���
            {
                mergeScaleTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, mergeScaleTime);
            }
            if (mergeScaleTime >= 1 && mergeScaleTimeBack <= 1)    //��С�Ĺ���
            {
                mergeScaleTimeBack += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, mergeScaleTimeBack);
            }
            if (mergeScaleTime >= 1 && mergeScaleTimeBack >= 1)
            {
                isPlayingMergeAnim = false;
            }
        }

        //�ƶ�����
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
                    GameObject.Destroy(this.gameObject);//destory��ʱʵ�ֲ��� 
                }
            }
        }
    }
}
