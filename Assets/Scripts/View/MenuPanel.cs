using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public SelectModelPanel selectModelPanel;
    public SetPanel setPanel;

    public AudioClip bgClip;

    private void Start()
    {
        AudioManager._instance.PlayMusic(bgClip);
    }


    //�����ʼ��Ϸ
    public void OnStartGameClick() {
        //��ʾѡ��ģʽ�Ľ���
        selectModelPanel.Show();
    }

    //�������
    public void OnSetClick() {
        //��ʾ���õĽ���
        setPanel.Show();
    }

    //����˳���Ϸ
    public void OnExitClick() {
        //�˳���Ϸ
        Application.Quit();
    }      
}                                    