﻿using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using CountersPlus.Custom;
using CountersPlus.UI;
using CountersPlus.UI.FlowCoordinators;
using CountersPlus.UI.SettingGroups;
using CountersPlus.UI.ViewControllers;
using CountersPlus.UI.ViewControllers.Editing;
using CountersPlus.Utils;
using HMUI;
using SiraUtil;
using System;
using UnityEngine;
using Zenject;

namespace CountersPlus.Installers
{
    public class MenuUIInstaller : MonoInstaller
    {
        private MenuButton menuButton;

        // Using Zenject for UI lets goooooooooooo
        public override void InstallBindings()
        {
            // CanvasUtility for UI
            Container.Bind<CanvasUtility>().AsSingle();
            Container.Bind<MockCounter>().AsSingle();

            BindSettingsGroup<MainSettingsGroup>();
            BindSettingsGroup<CountersSettingsGroup>();
            BindSettingsGroup<HUDsSettingsGroup>();

            foreach (CustomCounter customCounter in Plugin.LoadedCustomCounters.Values)
            {
                if (customCounter.BSML != null && customCounter.BSML.HasType)
                {
                    Type hostType = customCounter.BSML.HostType;
                    if (hostType.BaseType == typeof(MonoBehaviour))
                    {
                        Container.Bind(hostType).WithId(customCounter.Name).FromComponentOnRoot().AsCached();
                    }
                    else
                    {
                        Container.Bind(hostType).WithId(customCounter.Name).AsCached();
                    }
                }
            }

            BindViewController<CountersPlusCreditsViewController>();
            BindViewController<CountersPlusMainScreenNavigationController>();
            BindViewController<CountersPlusBlankViewController>();
            BindViewController<CountersPlusSettingSectionSelectionViewController>();
            BindViewController<CountersPlusHorizontalSettingsListViewController>();
            BindViewController<CountersPlusCounterEditViewController>();
            BindViewController<CountersPlusMainSettingsEditViewController>();
            BindViewController<CountersPlusHUDListViewController>();
            BindViewController<CountersPlusHUDEditViewController>();

            Container.BindFlowCoordinator<CountersPlusSettingsFlowCoordinator>();
            Container.BindInterfacesAndSelfTo<MenuButtonManager>().AsSingle().NonLazy();
        }

        private void BindViewController<T>() where T : ViewController
        {
            Container.BindViewController<T>();
        }

        private void BindSettingsGroup<T>() where T : SettingsGroup
        {
            Container.Bind<SettingsGroup>().To<T>().AsCached().NonLazy();
        }
    }
}
