using HamburgerMenuApp.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace HamburgerMenuApp.ViewModels
{
    public class BasicPageViewModel : BindableBase
    {
        #region フィールド
        private string m_title;

        private DelegateCommand m_navigateMenuFrameCommand;
        private DelegateCommand m_navigateRootFrameCommand;
        #endregion

        #region プロパティ
        public string Title
        {
            get { return m_title; }
            set { this.SetProperty(ref m_title, value); }
        }

        #endregion

        #region コマンド
        public DelegateCommand NavigateMenuFrameCommand
        {
            get { return m_navigateMenuFrameCommand = m_navigateMenuFrameCommand ?? new DelegateCommand(NavigateMenuFrame); }
        }

        public DelegateCommand NavigateRootFrameCommand
        {
            get { return m_navigateRootFrameCommand = m_navigateRootFrameCommand ?? new DelegateCommand(NavigateRootFrame); }
        }
        #endregion

        #region メソッド
        public BasicPageViewModel()
        {
            this.Title = "ベーシックページ";
        }

        private void NavigateMenuFrame()
        {
            AppShell.Current.AppFrame.Navigate(typeof(BasicDetailPage));
        }

        private void NavigateRootFrame()
        {
            AppShell.Current.Frame.Navigate(typeof(BasicDetailPage));
        }
        #endregion

    }
}
