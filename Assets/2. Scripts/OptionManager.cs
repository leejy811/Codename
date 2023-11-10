using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public void ExitButton_onMouseClick()
    {
        Application.Quit();
    }

    public void SetResolution(GameObject g)
    {
        string res = g.GetComponent<TextMeshProUGUI>().text;
        //Debug.Log(res);
        if (res == "640x480")
            Screen.SetResolution(640, 480, true);
        else if (res == "1920x1080")
            Screen.SetResolution(1920, 1080, true);
        else if (res == "1366x766")
            Screen.SetResolution(1366, 766, true);

    }

    public void SetMusic(Toggle g)
    {
        bool b = g.isOn;
        if(b) MMSoundManager.Instance.UnmuteMusic();
        else MMSoundManager.Instance.MuteMusic();
    }

    public void SetSFX(Toggle g)
    {
        bool b = g.isOn;
        if (b) {
            MMSoundManager.Instance.UnmuteUI();
            MMSoundManager.Instance.UnmuteSfx(); 
            MMSoundManager.Instance.SetVolumeSfx(1f); // 일시정지 시 기본으로 sfx볼륨은 0이 되버려서.. 1로 다시 설정
        }
        else {
            MMSoundManager.Instance.MuteUI();
            MMSoundManager.Instance.MuteSfx();
        }
        
    }

    public void SetVolume(Slider s)
    {
        float changed = s.value / 5f;
        MMSoundManager.Instance.SetVolumeMaster(changed);
    }

    public void ToggleFullScreen(Toggle g)
    {
        Screen.fullScreen = g.isOn;
    }
}
