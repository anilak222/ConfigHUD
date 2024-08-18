using System;
using System.Collections;
using System.Linq;
using BepInEx.Configuration;
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

        public void Initialize2(GameObject ammoPanel, GameObject magnificationPanel, GameObject bodyparts, GameObject bodypartsbg, GameObject effectspanel)
        {
            this._ammoPanelTextGameObject = ammoPanel;
            this._ammoPanelTextGameObject.SetActive(ConfigHUDPlugin.ammopanelVisibility.Value);
            ConfigHUDPlugin.ammopanelVisibility.SettingChanged += (sender, arguments) =>
            {
                this._ammoPanelTextGameObject.SetActive(ConfigHUDPlugin.ammopanelVisibility.Value);
            };
            this._magnificationPanelGameObject = magnificationPanel;
            this._magnificationPanelGameObject.SetActive(ConfigHUDPlugin.magnificationpanelVisibility.Value);
            ConfigHUDPlugin.magnificationpanelVisibility.SettingChanged += (sender, arguments) =>
            {
                this._magnificationPanelGameObject.SetActive(ConfigHUDPlugin.magnificationpanelVisibility.Value);
            };
            this._bodypartsPanelGameObject = bodyparts;
            this._bodypartsBGGameObject = bodypartsbg;
            this._effectsPanelGameObject = effectspanel;
            this._effectsPanelRT = effectspanel.GetComponent<RectTransform>();

            if (_bodypartsPanelGameObject != null && _bodypartsBGGameObject != null)
            {
                this._bodypartsPanelGameObject.SetActive(ConfigHUDPlugin.bodypartsVisibility.Value);
                this._bodypartsBGGameObject.SetActive(ConfigHUDPlugin.bodypartsVisibility.Value);
                ConfigHUDPlugin.bodypartsVisibility.SettingChanged += (sender, arguments) =>
                {
                    this._bodypartsPanelGameObject.SetActive(ConfigHUDPlugin.bodypartsVisibility.Value);
                    this._bodypartsBGGameObject.SetActive(ConfigHUDPlugin.bodypartsVisibility.Value);
                    if (ConfigHUDPlugin.bodypartsVisibility.Value == false)
                    {
                        this._effectsPanelRT.anchoredPosition = this._newEPAnchoredPosition;
                    }
                    else
                    {
                        this._effectsPanelRT.anchoredPosition = this._originalAnchoredPosition;
                    }
                };
            }
        }

        public void Start()
        {
            if (_bodypartsPanelGameObject != null && _bodypartsBGGameObject != null)
            {
                if (ConfigHUDPlugin.bodypartsVisibility.Value == false)
                {
                    this._effectsPanelRT.anchoredPosition = this._newEPAnchoredPosition;
                }
                else
                {
                    this._effectsPanelRT.anchoredPosition = this._originalAnchoredPosition;
                }
            }
        }

        public void Update()
        {
            if (ConfigHUDPlugin.checkAmmo.Value.IsPressedIgnoreOthers() && !ConfigHUDPlugin.ammopanelVisibility.Value)
            {
                EnableAmmoPanelTemp(2f);
            }
            if (ConfigHUDPlugin.checkFireMode.Value.IsPressedIgnoreOthers() && !ConfigHUDPlugin.ammopanelVisibility.Value)
            {
                EnableAmmoPanelTemp(2f);
            }
            if (ConfigHUDPlugin.switchFireMode.Value.IsPressedIgnoreOthers() && !ConfigHUDPlugin.ammopanelVisibility.Value)
            {
                EnableAmmoPanelTemp(2f);
            }
            if (ConfigHUDPlugin.aimDownSight1.Value.IsDownIgnoreOthers() && !ConfigHUDPlugin.ammopanelVisibility.Value && !ConfigHUDPlugin.magnificationpanelVisibility.Value)
            {
                Player localPlayerFromWorld = this.GetLocalPlayerFromWorld();
                if (_ammoPanelTextGameObject != null && _magnificationPanelGameObject != null)
                {
                    ConfigHUDPlugin.ammopanelVisibility.Value = !ConfigHUDPlugin.ammopanelVisibility.Value;
                    ConfigHUDPlugin.magnificationpanelVisibility.Value = !ConfigHUDPlugin.magnificationpanelVisibility.Value;
                }
                if (localPlayerFromWorld == null)
                {
                    ConfigHUDPlugin.ammopanelVisibility.Value = false;
                    ConfigHUDPlugin.magnificationpanelVisibility.Value = false;
                    return;
                }
            }
            else if(ConfigHUDPlugin.aimDownSight1.Value.IsUpIgnoreOthers())
            {
                if (_ammoPanelTextGameObject != null && _magnificationPanelGameObject != null)
                {
                    ConfigHUDPlugin.ammopanelVisibility.Value = !ConfigHUDPlugin.ammopanelVisibility.Value;
                    ConfigHUDPlugin.magnificationpanelVisibility.Value = !ConfigHUDPlugin.magnificationpanelVisibility.Value;
                }
            }
            if (ConfigHUDPlugin.aimDownSight2.Value.IsDownIgnoreOthers())
            {
                if (_ammoPanelTextGameObject != null && _magnificationPanelGameObject != null)
                {
                    ConfigHUDPlugin.ammopanelVisibility.Value = !ConfigHUDPlugin.ammopanelVisibility.Value;
                    ConfigHUDPlugin.magnificationpanelVisibility.Value = !ConfigHUDPlugin.magnificationpanelVisibility.Value;
                }
            }
        }

        private void EnableAmmoPanelTemp(float duration)
        {
            if (_ammoPanelTextGameObject != null)
            {
                ConfigHUDPlugin.ammopanelVisibility.Value = true;
                this._ammoPanelTextGameObject.SetActive(ConfigHUDPlugin.ammopanelVisibility.Value);
                StartCoroutine(HideAmmoPanel(duration));
            }
        }

        private IEnumerator HideAmmoPanel(float delay)
        {
            if (_ammoPanelTextGameObject != null)
            {
                yield return new WaitForSeconds(delay);
                ConfigHUDPlugin.ammopanelVisibility.Value = false;
                this._ammoPanelTextGameObject.SetActive(ConfigHUDPlugin.ammopanelVisibility.Value);
            }
        }

        private bool IsWorldDestroyed()
        {
            GameWorld instance = Singleton<GameWorld>.Instance;
            return instance == null;
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

        private GameObject _ammoPanelTextGameObject;

        private GameObject _magnificationPanelGameObject;

        private GameObject _bodypartsPanelGameObject;

        private GameObject _bodypartsBGGameObject;

        private GameObject _effectsPanelGameObject;

        private RectTransform _effectsPanelRT;

        private Vector2 _newEPAnchoredPosition = new Vector2(10, -8);

        private Vector2 _originalAnchoredPosition = new Vector2(146, -8);
    }

    internal static class ShortcutExtensions
    {
        public static bool IsDownIgnoreOthers(this KeyboardShortcut shortcut)
        {
            return Input.GetKeyDown(shortcut.MainKey) && shortcut.Modifiers.All(Input.GetKey);
        }

        public static bool IsUpIgnoreOthers(this KeyboardShortcut shortcut)
        {
            return Input.GetKeyUp(shortcut.MainKey) && shortcut.Modifiers.All(Input.GetKey);
        }

        public static bool IsPressedIgnoreOthers(this KeyboardShortcut shortcut)
        {
            return Input.GetKey(shortcut.MainKey) && shortcut.Modifiers.All(Input.GetKey);
        }
    }
}
