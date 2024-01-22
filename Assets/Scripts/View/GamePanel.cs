using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GamePanel : View
{
    #region UI�ؼ�
    public Text text_socre;         //����
    public Text text_best_score;    //��߷�  
    public Button btn_restart;      //���¿�ʼ
    public Button btn_exit;         //�˳���Ϸ

    public WinPanel winPanel;
    public LosePanel losePanel;
    #endregion


    #region ���� ����
    public Transform gridParent;    //���ӵĸ�����

    public Dictionary<int, int> grid_config = new Dictionary<int, int>() { { 4, 80 }, { 5, 65 }, { 6, 52 } };

    private int row;    //��
    private int col;    //��
    public MyGrid[][] grids = null;//���еĸ���
    public List<MyGrid> canCreateNumberGrid = new List<MyGrid>();//���Դ������ֵĸ���


    public GameObject gridPrefab;           //���ӵ�Ԥ����
    public GameObject numberPrefab;         //���ֵ�Ԥ����

    private Vector3 pointerDownPos, pointerUpPos;

    private bool isNeedCreateNumber = false;

    public int currentScore;


    public AudioClip bgClip;

    #endregion


    #region ��Ϸ�߼�

    //��ʼ������
    public void InitGrid()
    {
        //��ȡ���ӵ�����
        int gridNum = PlayerPrefs.GetInt(Const.GameModel, 4);
        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = gridNum;
        gridLayoutGroup.cellSize = new Vector2(grid_config[gridNum], grid_config[gridNum]);

        //��ʼ������
        grids = new MyGrid[gridNum][];

        //��������
        row = gridNum;
        col = gridNum;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //���� i j ����
                if (grids[i] == null)
                {
                    grids[i] = new MyGrid[gridNum];
                }
                grids[i][j] = CreateGrid();
            }
        }
    }

    //��������
    public MyGrid CreateGrid()
    {
        //ʵ�������ӵ�Ԥ����
        GameObject gameObject = GameObject.Instantiate(gridPrefab, gridParent);

        return gameObject.GetComponent<MyGrid>();
    }

    //��������
    public void CreateNumber()
    {
        //�ҵ��������ڵĸ���
        canCreateNumberGrid.Clear();
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (!grids[i][j].IsHaveNumber())//�ж����������û������
                {
                    //���û������
                    canCreateNumberGrid.Add(grids[i][j]);
                }
            }
        }
        if (canCreateNumberGrid.Count == 0)
        {
            return;
        }
        //���һ������
        int index = Random.Range(0, canCreateNumberGrid.Count);

        //�������� �����ַŽ�ȥ
        GameObject gameObj = GameObject.Instantiate(numberPrefab, canCreateNumberGrid[index].transform);
        gameObj.GetComponent<Number>().Init(canCreateNumberGrid[index]);//���г�ʼ��
    }

    //�ж�/ȷ�� �ƶ�����
    public MoveType CaculateMoveType()
    {
        if (Mathf.Abs(pointerUpPos.x - pointerDownPos.x) > Mathf.Abs(pointerDownPos.y - pointerUpPos.y))
        {
            //�����ƶ�
            if (pointerUpPos.x - pointerDownPos.x > 0)
            {
                //�����ƶ�
                Debug.Log("�����ƶ�");
                return MoveType.RIGHT;
            }
            else
            {
                //�����ƶ�
                Debug.Log("�����ƶ�");
                return MoveType.LEFT;
            }
        }
        else
        {
            //�����ƶ�
            if (pointerUpPos.y - pointerDownPos.y > 0)
            {
                //�����ƶ�
                Debug.Log("�����ƶ�");
                return MoveType.UP;
            }
            else
            {
                //�����ƶ�
                Debug.Log("�����ƶ�");
                return MoveType.DOWN;
            }
        }
    }

    //�ƶ���Ľ��
    public void MoveNumber(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.UP:
                for (int j = 0; j < col; j++)
                    for (int i = 1; i < row; i++)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {
                            Number number = grids[i][j].GetNumber();

                            //Debug.Log("���꣺" + i + "," + j);
                            for (int m = i - 1; m >= 0; m--)
                            {
                                Number targetNumber = null;
                                if (grids[m][j].IsHaveNumber())
                                {
                                    targetNumber = grids[m][j].GetNumber();
                                }
                                HandleNumber(number, targetNumber, grids[m][j]);
                                if (targetNumber != null)//��������� ����ѭ��
                                {
                                    break;
                                }
                            }
                        }
                    }
                break;
            case MoveType.DOWN:
                for (int j = 0; j < col; j++)
                {
                    for (int i = row - 2; i >= 0; i--)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {
                            Number number = grids[i][j].GetNumber();

                            //Debug.Log("���꣺" + i + "," + j);
                            for (int m = i + 1; m < row; m++)
                            {
                                Number targetNumber = null;
                                if (grids[m][j].IsHaveNumber())
                                {
                                    targetNumber = grids[m][j].GetNumber();
                                }
                                HandleNumber(number, targetNumber, grids[m][j]);
                                if (targetNumber != null)//��������� ����ѭ��
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case MoveType.LEFT:
                for (int i = 0; i < row; i++)
                    for (int j = 1; j < col; j++)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {
                            Number number = grids[i][j].GetNumber();

                            //Debug.Log("���꣺" + i + "," + j);
                            for (int m = j - 1; m >= 0; m--)
                            {
                                Number targetNumber = null;
                                if (grids[i][m].IsHaveNumber())
                                {
                                    targetNumber = grids[i][m].GetNumber();
                                }
                                HandleNumber(number, targetNumber, grids[i][m]);
                                if (targetNumber != null)//��������� ����ѭ��
                                {
                                    break;
                                }
                            }
                        }
                    }
                break;
            case MoveType.RIGHT:
                for (int i = 0; i < row; i++)
                    for (int j = col - 2; j >= 0; j--)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {
                            Number number = grids[i][j].GetNumber();

                            //Debug.Log("���꣺" + i + "," + j);
                            for (int m = j + 1; m < col; m++)
                            {
                                Number targetNumber = null;
                                if (grids[i][m].IsHaveNumber())
                                {
                                    targetNumber = grids[i][m].GetNumber();
                                }
                                HandleNumber(number, targetNumber, grids[i][m]);
                                if (targetNumber != null)//��������� ����ѭ��
                                {
                                    break;
                                }
                            }
                        }
                    }
                break;
        }
    }

    //��������
    public void HandleNumber(Number current, Number target, MyGrid targetGrid)
    {
        if (target != null)
        {
            //�ж� �ܲ��� �ϲ�
            if (current.IsMerge(target))
            {
                target.Merge();

                //���ٵ�ǰ������
                current.GetGrid().SetNumber(null);
                //GameObject.Destory(current.gameObject);
                current.DestoryOnMoveEnd(target.GetGrid());
                isNeedCreateNumber = true;

            }
        }
        else
        {
            //û������
            current.MoveToGrid(targetGrid);
            isNeedCreateNumber = true;
        }
    }

    //��������״̬
    public void ResetNumberStatus()
    {
        //�������е�����
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (grids[i][j].IsHaveNumber())
                {
                    grids[i][j].GetNumber().status = NumberStatus.Normal;
                }
            }
        }
    }

    //�ж���Ϸ�Ƿ�ʧ��
    public bool IsGameLose()
    {
        //�жϸ����Ƿ�����
        for (int i = 0; i < row; i++)
        { for (int j = 0; j < col; j++)
            {
                if (!grids[i][j].IsHaveNumber())
                {
                    return false;
                }
            }
        }
        //�ж���û�������ܺϲ�
        for (int i = 0; i < row; i+=2)
        {
            for (int j = 0; j < col; j++)
            {
                MyGrid up    =  IsHaveGrid(i - 1, j) ? grids[i - 1][j] : null;
                MyGrid down  =  IsHaveGrid(i + 1, j) ? grids[i + 1][j] : null;
                MyGrid left  =  IsHaveGrid(i, j - 1) ? grids[i][j - 1] : null;
                MyGrid right =  IsHaveGrid(i, j + 1) ? grids[i][j + 1] : null;
                //grids[i][j];
                if (up != null)
                {
                    if(grids[i][j].GetNumber().IsMerge(up.GetNumber()))
                    {
                        return false;
                    }
                }
                if (down != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(down.GetNumber()))
                    {
                        return false;
                    }
                }
                if (left != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(left.GetNumber()))
                    {
                        return false;
                    }
                }
                if (right != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(right.GetNumber()))
                    {
                        return false;
                    }
                }
            }
        }
        return true;//��Ϸʧ��
    }

    //�ж��Ƿ��и��ӣ����ڸ��������ķ�λ�ж�
    public bool IsHaveGrid(int i,int j) {
        if (i >= 0 && j < row && j >= 0 && j < col)
        {
            return true;
        }
        return false;
    }

    #endregion


    #region �ű�����
    private void Awake()
    {
        //��ʼ��������Ϣ
        InitPanelMessage();
        //��ʼ������
        InitGrid();
        //������һ������
        CreateNumber();
    }
    #endregion


    #region �¼�����

    public void OnPointerDown()
    {
        //Debug.Log("���£�" + Input.mousePosition);
        pointerDownPos = Input.mousePosition;

    }

    public void OnPointerUp()
    {
        //Debug.Log("̧��" + Input.mousePosition);
        pointerUpPos = Input.mousePosition;

        if (Vector3.Distance(pointerDownPos, pointerUpPos) < 100)
        {
            Debug.Log("��Ч�Ĳ�����");
            return;
        }

        MoveType moveType = CaculateMoveType();
        Debug.Log("�ƶ����ͣ�" + moveType);
        MoveNumber(moveType);

        //��������
        if (isNeedCreateNumber)
        {
            CreateNumber();
        }

        //���������ֵ�״̬ �ָ�������״̬
        ResetNumberStatus();
        isNeedCreateNumber = false;

        //�ж���Ϸ�Ƿ����
        if (IsGameLose())
        {
            GameLose();
            
        }
    }


    #endregion


    #region �������

    public void InitPanelMessage()
    {
        this.text_best_score.text = PlayerPrefs.GetInt(Const.BestScore,0).ToString();

        //��������
        //AudioManager._instance.PlayMusic(bgClip);
    }
    //������������
    private void Start()
    {
        AudioManager._instance.PlayMusic(bgClip);
    }

    //���ӷ���
    public void AddScore(int score)
    {
        currentScore += score;
        UpdateScore(currentScore);

        //�жϵ�ǰ�����Ƿ�����߷�
        if (currentScore > PlayerPrefs.GetInt(Const.BestScore, 0))
        {
            PlayerPrefs.SetInt(Const.BestScore, currentScore);
            UpdateBestScore(currentScore);
        }
    }
    //���·���
    public void UpdateScore(int score)
    {
        this.text_best_score.text = score.ToString();
    }
    //���÷���
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScore(currentScore);
    }
    //������߷�
    public void UpdateBestScore(int best_score)
    {
        this.text_best_score.text = best_score.ToString();
    }

    #endregion


    #region ��Ϸ����

    public void ExitGame() { }

    //���¿�ʼ
    public void RestartGame() {
        //�������

        //��շ���
        ResetScore();
        //�������
        for(int i=0;i<row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                GameObject.Destroy(grids[i][j].GetNumber().gameObject);//destoryʵ�ֲ���....ƴ����

                grids[i][j].SetNumber(null);
            }
        }

        //����һ������
        CreateNumber();
    }

    public void GameWin()
    {
        Debug.Log("��Ϸʤ��");
        winPanel.Show();
    }

    public void GameLose()
    {
        Debug.Log("��Ϸʧ��");
        losePanel.Show();
    }
    #endregion

}
