using Snakefun.Game.Management;
using UnityEngine;
using UnityEngine.UI;

public class MusicSwitcher : MonoBehaviour
{
    [SerializeField] Toggle musicSwitcherToggle;
    [SerializeField] Toggle soundSwitcherToggle;

    private void Start()
    {
        musicSwitcherToggle.isOn = SettingsManager.Instance.MusicOn;
        soundSwitcherToggle.isOn = SettingsManager.Instance.SoundEffectsOn;
    }

    private void Update()
    {
        musicSwitcherToggle.onValueChanged.AddListener(SettingsManager.Instance.SwitchMusic);
        soundSwitcherToggle.onValueChanged.AddListener(SettingsManager.Instance.SwitchSoundEffects);
    }
}
