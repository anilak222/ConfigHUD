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
        public void Initialize(GameObject stances, GameObject stanceslider, GameObject volumeslider, GameObject volumeicon, GameObject sprintbar, GameObject handsbar)
        {
            this._battleStanceGameObject = stances;
            this._battleStanceSliderGameObject = stanceslider;
            this._battleVolumeSliderGameObject = volumeslider;
            this._battleVolumeIconGameObject = volumeicon;
            this._sprintBarGameObject = sprintbar;
            this._energyBarGameObject = handsbar;
            this._battleStanceGameObject.SetActive(ConfigHUDPlugin.stanceVisibility.Value);
            this._battleStanceSliderGameObject.SetActive(ConfigHUDPlugin.heightVisibility.Value);
            this._battleVolumeSliderGameObject.SetActive(ConfigHUDPlugin.volumeVisibility.Value);
            this._battleVolumeIconGameObject.SetActive(ConfigHUDPlugin.volumeVisibility.Value);
            ConfigHUDPlugin.stanceVisibility.SettingChanged += (sender, arguments) =>
            {
                this._battleStanceGameObject.SetActive(ConfigHUDPlugin.stanceVisibility.Value);
            };
            ConfigHUDPlugin.heightVisibility.SettingChanged += (sender, arguments) =>
            {
                this._battleStanceSliderGameObject.SetActive(ConfigHUDPlugin.heightVisibility.Value);
            };
            ConfigHUDPlugin.volumeVisibility.SettingChanged += (sender, arguments) =>
            {
                this._battleVolumeSliderGameObject.SetActive(ConfigHUDPlugin.volumeVisibility.Value);
                this._battleVolumeIconGameObject.SetActive(ConfigHUDPlugin.volumeVisibility.Value);
            };
        }

        public void Initialize2(GameObject ammoPanel, GameObject magnificationPanel, GameObject bodyparts, GameObject bodypartsbg, GameObject effectspanel, GameObject quickgesturespanel)
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
            this._quickGesturesGameObject = quickgesturespanel;
            this._quickGesturesGameObject.SetActive(ConfigHUDPlugin.gesturespanelVisibility.Value);
            ConfigHUDPlugin.gesturespanelVisibility.SettingChanged += (sender, arguments) =>
            {
                this._quickGesturesGameObject.SetActive(ConfigHUDPlugin.gesturespanelVisibility.Value);
            };


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
            if (_battleStanceSliderGameObject != null)
            {
                _heightSliderTransform = _battleStanceSliderGameObject.GetComponent<RectTransform>();
            }
            if (_sprintBarGameObject != null)
            {
                _sprintBarTransform = _sprintBarGameObject.GetComponent<RectTransform>();
            }
            if (_energyBarGameObject != null)
            {
                _energyBarTransform = _energyBarGameObject.GetComponent<RectTransform>();
            }
            if (_battleVolumeSliderGameObject != null)
            {
                _volumeSliderTransform = _battleVolumeSliderGameObject.GetComponent<RectTransform>();
            }
            if (_battleVolumeIconGameObject != null)
            {
                _volumeIconTransform = _battleVolumeIconGameObject.GetComponent<RectTransform>();
                _volumeIconOriginalPosition = _volumeIconTransform.position;
            }
            UpdateUIPositionsAndSizes();
        }

        public void Update()
        {
            UpdateUIPositionsAndSizes();
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
            if (ConfigHUDPlugin.elevationUp.Value.IsPressedIgnoreOthers() && !ConfigHUDPlugin.ammopanelVisibility.Value)
            {
                EnableAmmoPanelTemp(1f);
            }
            if (ConfigHUDPlugin.elevationDown.Value.IsPressedIgnoreOthers() && !ConfigHUDPlugin.ammopanelVisibility.Value)
            {
                EnableAmmoPanelTemp(1f);
            }
        }

        private void UpdateUIPositionsAndSizes()
        {
            if (_heightSliderTransform != null)
            {
                _heightSliderTransform.anchoredPosition = new Vector2(ConfigHUDPlugin.heightSliderPosX.Value, ConfigHUDPlugin.heightSliderPosY.Value);
                _heightSliderTransform.localScale = new Vector3(ConfigHUDPlugin.heightSliderScaleX.Value, ConfigHUDPlugin.heightSliderScaleY.Value, 1f);
            }
            if (_sprintBarTransform != null)
            {
                _sprintBarTransform.anchoredPosition = new Vector2(ConfigHUDPlugin.sprintBarPosX.Value, ConfigHUDPlugin.sprintBarPosY.Value);
                _sprintBarTransform.sizeDelta = new Vector2(ConfigHUDPlugin.sprintBarWidth.Value, ConfigHUDPlugin.sprintBarHeight.Value);
                if(_volumeIconTransform != null)
                {
                    _volumeIconTransform.position = _volumeIconOriginalPosition;
                }
            }
            if (_energyBarTransform != null)
            {
                _energyBarTransform.anchoredPosition = new Vector2(ConfigHUDPlugin.energyBarPosX.Value, ConfigHUDPlugin.energyBarPosY.Value);
                _energyBarTransform.sizeDelta = new Vector2(ConfigHUDPlugin.energyBarWidth.Value, ConfigHUDPlugin.energyBarHeight.Value);
            }
            if (_volumeSliderTransform != null)
            {
                _volumeSliderTransform.anchoredPosition = new Vector2(ConfigHUDPlugin.volumeSliderPosX.Value, ConfigHUDPlugin.volumeSliderPosY.Value);
                _volumeSliderTransform.localScale = new Vector3(ConfigHUDPlugin.volumeSliderScaleX.Value, ConfigHUDPlugin.volumeSliderScaleY.Value, 1f);
            }
            if (_volumeIconTransform != null)
            {
                _volumeIconTransform.anchoredPosition = new Vector2(ConfigHUDPlugin.volumeIconPosX.Value, ConfigHUDPlugin.volumeIconPosY.Value);
                _volumeIconTransform.localScale = new Vector3(ConfigHUDPlugin.volumeIconScaleX.Value, ConfigHUDPlugin.volumeIconScaleY.Value, 1f);
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

        private Player GetLocalPlayerFromWorld()
        {
            GameWorld instance = Singleton<GameWorld>.Instance;
            if (instance == null || instance.MainPlayer == null)
            {
                return null;
            }
            return instance.MainPlayer;
        }

        private GameObject _battleStanceGameObject;

        private GameObject _battleStanceSliderGameObject;

        private GameObject _battleVolumeSliderGameObject;

        private GameObject _battleVolumeIconGameObject;

        private GameObject _sprintBarGameObject;

        private GameObject _energyBarGameObject;

        private GameObject _ammoPanelTextGameObject;

        private GameObject _magnificationPanelGameObject;

        private GameObject _bodypartsPanelGameObject;

        private GameObject _bodypartsBGGameObject;

        private GameObject _quickGesturesGameObject;

        private GameObject _effectsPanelGameObject;

        private RectTransform _effectsPanelRT;

        private RectTransform _heightSliderTransform;

        private RectTransform _sprintBarTransform;

        private RectTransform _energyBarTransform;

        private RectTransform _volumeSliderTransform;

        private RectTransform _volumeIconTransform;

        private Vector2 _volumeIconOriginalPosition;

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
