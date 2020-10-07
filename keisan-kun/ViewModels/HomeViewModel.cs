﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keisan_kun.ViewModels
{
    class HomeViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        #region Commands
        public DelegateCommand<string> GoToTrialSettingCommand { get; private set; }
        #endregion

        public HomeViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            CreateCommands();
        }

        private void CreateCommands()
        {
            GoToTrialSettingCommand = new DelegateCommand<string>(GoToTrialSetting);
        }

        private void GoToTrialSetting(string obj)
        {
            if(obj != null)
            {
                var navParams = new NavigationParameters();
                navParams.Add("mode", obj);
                _regionManager.RequestNavigate("ContentRegion", "TrialSetting", navParams);
            }
        }

        /// <summary>
        /// 画面遷移時にインスタンスを使い回すかどうか
        /// true: 使い回す（コンストラクタ呼ばれない）
        /// false: 使い回さない（コンストラクタ呼ばれる）
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}