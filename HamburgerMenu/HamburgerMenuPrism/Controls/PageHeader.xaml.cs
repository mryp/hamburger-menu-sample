using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HamburgerMenuPrism.Controls
{
    /// <summary>
    /// コマンド機能付きページのヘッダコントロール
    /// </summary>
    public sealed partial class PageHeader : UserControl
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PageHeader()
        {
            this.InitializeComponent();

            this.Loaded += (s, a) =>
            {
                AppShell.Current.TogglePaneButtonRectChanged += Current_TogglePaneButtonSizeChanged;
                this.titleBar.Margin = new Thickness(AppShell.Current.TogglePaneButtonRect.Right, 0, 0, 0);
            };
        }

        /// <summary>
        /// メニュー部分のサイズ変更通知処理
        /// </summary>
        private void Current_TogglePaneButtonSizeChanged(AppShell sender, Rect e)
        {
            this.titleBar.Margin = new Thickness(e.Right, 0, 0, 0);
        }

        /// <summary>
        /// ヘッダーオブジェクト
        /// </summary>
        public UIElement HeaderContent
        {
            get { return (UIElement)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        /// <summary>
        /// ヘッダーオブジェクトを返すプロパティ
        /// </summary>
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register("HeaderContent", typeof(UIElement), typeof(PageHeader), new PropertyMetadata(DependencyProperty.UnsetValue));

    }
}
