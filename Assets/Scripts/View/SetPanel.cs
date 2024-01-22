using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : View
{
    public Slider slider_sound;
    public Slider slider_music;

    //�رհ�ť
    public void OnBtnCloseClick() {
        //��ǰ��������
        Hide();
    }
    
    //��Ч
    public void OnSoundValueChange(float f) {

        //�޸���Ч�Ĵ�С
        AudioManager._instance.OnSoundVolumeChange(f);

        //���浱ǰ���޸�
        PlayerPrefs.SetFloat(Const.Sound, f);
    }

    //����
    public void OnMusicValueChange(float f) {

        //�޸����ֵĴ�С
        AudioManager._instance.OnMusicVolumeChange(f);
        //���浱ǰ���޸�
        PlayerPrefs.SetFloat(Const.Music, f);
    }


    public override void Show()
    {
        base.Show();
        //�Խ�����г�ʼ��
        slider_sound.value = PlayerPrefs.GetFloat(Const.Sound,0);
        slider_music.value = PlayerPrefs.GetFloat(Const.Music,0);
    }

  
}
