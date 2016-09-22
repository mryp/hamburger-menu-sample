using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HamburgerMenuPrism.Controls
{
    /// <summary>
    /// ナビゲーションメニューアイテム
    /// </summary>
    class NavMenuItem : INotifyPropertyChanged
    {
        private bool _isSelected;
        private Visibility _selectedVis = Visibility.Collapsed;
        
        /// <summary>
        /// ラベル
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// アイコン
        /// </summary>
        public Symbol Symbol { get; set; }

        /// <summary>
        /// アイコンの文字データ
        /// </summary>
        public char SymbolAsChar
        {
            get
            {
                return (char)this.Symbol;
            }
        }
        
        /// <summary>
        /// 選択しているかどうか
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                SelectedVis = value ? Visibility.Visible : Visibility.Collapsed;
                this.OnPropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// 選択表示を行っているかどうか
        /// </summary>
        public Visibility SelectedVis
        {
            get { return _selectedVis; }
            set
            {
                _selectedVis = value;
                this.OnPropertyChanged("SelectedVis");
            }
        }

        /// <summary>
        /// 表示するページデータ
        /// </summary>
        public Type DestPage { get; set; }

        /// <summary>
        /// ページに渡す引数
        /// </summary>
        public object Arguments { get; set; }

        /// <summary>
        /// プロパティ変更通知イベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// プロパティ変更通知
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
