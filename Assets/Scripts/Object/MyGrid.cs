using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : View
{

    public Number number;//��������ӵ�����
    
    //�ж��ǲ���������
    public bool IsHaveNumber()
    {
        return number!=null;
    }
    //��ȡ������ӵ�����
    public Number GetNumber()
    {
        return number;
    }
    //��������
    public void SetNumber(Number number)
    {
        this.number = number;
    }
}
