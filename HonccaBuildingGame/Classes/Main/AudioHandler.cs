using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.Main
{
    class AudioHandler
    {
        public Dictionary<string, Audio> Sounds = new Dictionary<string, Audio>()
        {
            {
                "JumpSound",

                new Audio()
                {
                    FileName = "Audio/jumpSound"
                }
            }
        };

        public AudioHandler()
        {
            Dictionary<string, Audio> fakeAudio = Sounds;

            foreach (var sound in fakeAudio)
                Sounds[sound.Key].LoadTexture();
        }

        public SoundEffect GetAudio(string audioName)
        {
            if (Sounds.ContainsKey(audioName))
            {
                if (Sounds[audioName].Sound == null)
                    throw new Exception($"{audioName} doesn't exist in the Content folder.");

                return Sounds[audioName].Sound;
            }

            throw new Exception($"{audioName} doesn't exist in the dictionary.");
        }

        public void PlaySound(string audioName, float volume = 0.5f)
        {
            SoundEffect soundEffect = GetAudio(audioName);

            soundEffect.Play(volume, 0, 0);
        }

        public string GetAudioNameFromSoundEffect(SoundEffect audio)
        {
            foreach (var currentSprite in Sounds)
            {
                if (currentSprite.Value.Sound == audio)
                {
                    return currentSprite.Key;
                }
            }

            return string.Empty;
        }
    }
    class Audio
    {
        public string FileName;

        public SoundEffect Sound;

        public void LoadTexture()
        {
            Sound = MainGame.Instance.Content.Load<SoundEffect>(FileName);
        }
    }
}
