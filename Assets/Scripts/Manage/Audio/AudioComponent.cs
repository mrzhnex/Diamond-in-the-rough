using UnityEngine;

namespace Assets.Scripts.Manage.Audio
{
    public class AudioComponent : MonoBehaviour
    {
        public AudioSource AudioSource;
        public AudioClip[] BackgroundAudioClips;
        public System.Random Random = new System.Random();
        public void Start()
        {
            AudioSource = GetComponent<AudioSource>();
            AudioSource.volume = Global.Settings.VolumeMusic;
            if (Global.GameStage == GameStage.Menu)
            {
                AudioSource.clip = BackgroundAudioClips[Random.Next(BackgroundAudioClips.Length)];
                AudioSource.Play();
            }
        }
    }
}