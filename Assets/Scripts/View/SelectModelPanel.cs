using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//���ڵ���Ҫ

public class SelectModelPanel : View
{
    //���ģʽѡ��ť
    public void OnSelectModelClick(int count)
    {
        // ѡ��ģʽ
        PlayerPrefs.SetInt(Const.GameModel, count);
        //��ת���� �� ��Ϸ����
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);//����ֵ�Ļ�ã�ͨ��build settings
        SceneManager.LoadSceneAsync(1);
        //asyncOperation.progress����������
    }  
}