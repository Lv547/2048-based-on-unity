using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : View
{
    //���¿�ʼ�İ�ť����¼�
    public void OnRestartClick()
    {
        //����GamePanel ������¿�ʼ
        GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().RestartGame();
        //���ص�ǰ����
        Hide();
    }

    //�Ƴ���ť�ĵ���¼�
    public void OnExitClick()
    {
        //�˳����˵�����
        SceneManager.LoadSceneAsync(0);
    }
}
