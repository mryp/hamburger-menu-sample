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
    public class BasicDetailPageViewModel : BindableBase
    {
        #region フィールド
        private string m_title;
        #endregion

        #region プロパティ
        public string Title
        {
            get { return m_title; }
            set { this.SetProperty(ref m_title, value); }
        }

        #endregion
        
        #region メソッド
        public BasicDetailPageViewModel()
        {
            this.Title = "詳細ページ";
        }

        #endregion

    }
}
