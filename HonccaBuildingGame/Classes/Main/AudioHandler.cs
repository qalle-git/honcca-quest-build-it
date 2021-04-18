using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

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
            },
            {
                "STEP_1",

                new Audio()
                {
                    FileName = "Audio/stepSound_1"
                }
            },
            {
                "STEP_2",

                new Audio()
                {
                    FileName = "Audio/stepSound_2"
                }
            },
            {
                "STEP_3",

                new Audio()
                {
                    FileName = "Audio/stepSound_3"
                }
            },
            {
                "STEP_4",

                new Audio()
                {
                    FileName = "Audio/stepSound_4"
                }
            },
            {
                "DIALOGUE",

                new Audio()
                {
                    FileName = "Audio/dialogue"
                }
            },
            {
                "PICKUP_ITEM",

                new Audio()
                {
                    FileName = "Audio/pickupItem"
                }
            },
            {
                "PLACE_BLOCK",

                new Audio()
                {
                    FileName = "Audio/placeBlock"
                }
            }
        };

        public AudioHandler()
        {
            Dictionary<string, Audio> fakeAudio = Sounds;

            foreach (var sound in fakeAudio)
                Sounds[sound.Key].LoadAudio();
        }

        /// <summary>
        /// Get a SoundEffect object of the audio specified.
        /// </summary>
        /// <param name="audioName">What audio you want</param>
        /// <returns>A SoundEffect object.</returns>
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

        /// <summary>
        /// Play a soundeffect.
        /// </summary>
        /// <param name="audioName">The name of the SoundEffect</param>
        /// <param name="volume">The volume in float 0-1 default 0.5</param>
        public void PlaySound(string audioName, float volume = 0.5f)
        {
            SoundEffect soundEffect = GetAudio(audioName);

            soundEffect.Play(volume, 0, 0);
        }

        /// <summary>
        /// Retrieve the audio name by a soundeffect object.
        /// </summary>
        /// <param name="audio">SoundEffect object.</param>
        /// <returns>A string that returns the audioName</returns>
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

        public void LoadAudio()
        {
            Sound = MainGame.Instance.Content.Load<SoundEffect>(FileName);
        }
    }
}
