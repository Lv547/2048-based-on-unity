using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//有亿点重要

public class SelectModelPanel : View
{
    //点击模式选择按钮
    public void OnSelectModelClick(int count)
    {
        // 选择模式
        PlayerPrefs.SetInt(Const.GameModel, count);
        //跳转场景 到 游戏场景
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);//传入值的获得，通过build settings
        SceneManager.LoadSceneAsync(1);
        //asyncOperation.progress进度条功能
    }  
}