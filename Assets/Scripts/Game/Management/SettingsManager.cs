using Snakefun.Game.Control;
using UnityEngine;

namespace Snakefun.Game.Management
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance;

        public bool MusicOn { get; set; }
        public bool SoundEffectsOn { get; set; }

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource soundEffectSource;

        [SerializeField] AudioSource eatClip;
        [SerializeField] AudioSource dieClip;

        SnakeController snakeController;
        GameObject snake;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Instance.LoadSettings();
        }

        private void Update()
        {
            musicSource.mute = !Instance.MusicOn;
            soundEffectSource.mute = !Instance.musicSource;

            snake = GameObject.FindWithTag("Player");

            if (snake != null)
            {
                snakeController = snake.GetComponent<SnakeController>();
            }

            if (snakeController != null)
            {
                GameObject.FindWithTag("Player").GetComponent<SnakeController>().OnEat += PlayEatSound;
                GameObject.FindWithTag("Player").GetComponent<SnakeController>().OnDie += PlayDieSound;
            }
        }

        public void SwitchMusic(bool musicOn)
        {
            MusicOn = musicOn;

            if (Instance.MusicOn)
            {
                musicSource.volume = 0.1f;
                musicSource.Play();
            }

            SaveSettings();
        }
        public void SwitchSoundEffects(bool soundEffectsOn)
        {
            SoundEffectsOn = soundEffectsOn;

            if (Instance.SoundEffectsOn)
            {
                soundEffectSource.Play();
            }

            SaveSettings();
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetInt("MusicOn", MusicOn ? 1 : 0);
            PlayerPrefs.SetInt("SoundEffectsOn", SoundEffectsOn ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void LoadSettings()
        {
            MusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
            SoundEffectsOn = PlayerPrefs.GetInt("SoundEffectsOn", 1) == 1;
        }

        public void PlayEatSound()
        {
            if (SoundEffectsOn)
            {
                eatClip.Play();
            }
        }
        public void PlayDieSound()
        {
            if (SoundEffectsOn)
            {
                dieClip.Play();
            }
        }
    }

}