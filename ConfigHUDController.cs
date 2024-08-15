using System;
using System.Collections;
using Comfort.Common;
using EFT;
using EFT.CameraControl;
using EFT.UI;
using GPUInstancer;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace ConfigHUD
{
    internal class ConfigHUDController : MonoBehaviour
    {

        public void Initialize(Transform stances, Transform stanceslider, Transform volumeslider, Transform volumeicon)
        {
            this._battleStanceTransform = stances.gameObject;
            this._battleStanceSliderTransform = stanceslider.gameObject;
            this._battleVolumeSliderTransform = volumeslider.gameObject;
            this._battleVolumeIconTransform = volumeicon.gameObject;
            this._battleStanceTransform.SetActive(ConfigHUDPlugin.stanceVisibility.Value);
            this._battleStanceSliderTransform.SetActive(ConfigHUDPlugin.stanceVisibility.Value);
            this._battleVolumeSliderTransform.SetActive(ConfigHUDPlugin.volumeVisibility.Value);
            this._battleVolumeIconTransform.SetActive(ConfigHUDPlugin.volumeVisibility.Value);
            ConfigHUDPlugin.stanceVisibility.SettingChanged += (sender, arguments) =>
            {
                this._battleStanceTransform.SetActive(ConfigHUDPlugin.stanceVisibility.Value);
                this._battleStanceSliderTransform.SetActive(ConfigHUDPlugin.stanceVisibility.Value);
            };
            ConfigHUDPlugin.volumeVisibility.SettingChanged += (sender, arguments) =>
            {
                this._battleVolumeSliderTransform.SetActive(ConfigHUDPlugin.volumeVisibility.Value);
                this._battleVolumeIconTransform.SetActive(ConfigHUDPlugin.volumeVisibility.Value);
            };
        }

        public void Initialize2(Transform firemodetext)
        {
            this._firemodeTextGameObject = firemodetext;
            this._firemodeTextGameObject.gameObject.SetActive(ConfigHUDPlugin.ammotextVisibility.Value);
            ConfigHUDPlugin.ammotextVisibility.SettingChanged += (sender, arguments) =>
            {
                this._firemodeTextGameObject.gameObject.SetActive(ConfigHUDPlugin.ammotextVisibility.Value);
            };
        }

        public void Update()
        {
            if (ConfigHUDPlugin.checkAmmo.Value.IsDown())
            {
                ToggleFireModeText();
            }
        }

        private void ToggleStanceHUD()
        {
            if (this.GetLocalPlayerFromWorld() == null)
            {
                return;
            }
            ConfigHUDPlugin.stanceVisibility.Value = !ConfigHUDPlugin.stanceVisibility.Value;
            this._battleStanceTransform.transform.gameObject.SetActive(!ConfigHUDPlugin.stanceVisibility.Value);
            this._battleStanceSliderTransform.transform.gameObject.SetActive(!ConfigHUDPlugin.stanceVisibility.Value);
            this._battleStanceTransform.SetActive(ConfigHUDPlugin.stanceVisibility.Value);
            this._battleStanceSliderTransform.SetActive(ConfigHUDPlugin.stanceVisibility.Value);
        }
        private void ToggleVolumeHUD()
        {
            if (this.GetLocalPlayerFromWorld() == null)
            {
                return;
            }
            ConfigHUDPlugin.volumeVisibility.Value = !ConfigHUDPlugin.volumeVisibility.Value;
            this._battleVolumeSliderTransform.transform.gameObject.SetActive(ConfigHUDPlugin.volumeVisibility.Value);
            this._battleVolumeIconTransform.transform.gameObject.SetActive(ConfigHUDPlugin.volumeVisibility.Value);
            this._battleVolumeSliderTransform.SetActive(ConfigHUDPlugin.volumeVisibility.Value);
            this._battleVolumeIconTransform.SetActive(ConfigHUDPlugin.volumeVisibility.Value);
        }

        private void ToggleFireModeText()
        {
            if (this.GetLocalPlayerFromWorld() == null)
            {
                return;
            }
            ConfigHUDPlugin.ammotextVisibility.Value = !ConfigHUDPlugin.ammotextVisibility.Value;
            this._firemodeTextGameObject.gameObject.SetActive(ConfigHUDPlugin.ammotextVisibility.Value);
            this._firemodeTextGameObject.gameObject.SetActive(ConfigHUDPlugin.ammotextVisibility.Value);
        }

        private Player GetLocalPlayerFromWorld()
        {
            GameWorld instance = Singleton<GameWorld>.Instance;
            if (instance == null || instance.MainPlayer == null)
            {
                return null;
            }
            return instance.MainPlayer;
        }

        private GameObject _battleStanceTransform;

        private GameObject _battleStanceSliderTransform;

        private GameObject _battleVolumeSliderTransform;

        private GameObject _battleVolumeIconTransform;

        private Transform _firemodeTextGameObject;
    }
}
