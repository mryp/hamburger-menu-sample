using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace HamburgerMenuApp.Controls
{
    /// <summary>
    /// ナビゲーションメニューアイテムを一覧表示するリストビューコントロール
    /// </summary>
    class NavMenuListView : ListView
    {
        private SplitView splitViewHost;

        /// <summary>
        /// メニューアイテム選択イベント
        /// </summary>
        public event EventHandler<ListViewItem> ItemInvoked;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NavMenuListView()
        {
            this.SelectionMode = ListViewSelectionMode.Single;
            this.SingleSelectionFollowsFocus = false;
            this.IsItemClickEnabled = true;
            this.ItemClick += ItemClickedHandler;
            
            this.Loaded += (s, a) =>
            {
                var parent = VisualTreeHelper.GetParent(this);
                while (parent != null && !(parent is SplitView))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }

                if (parent != null)
                {
                    this.splitViewHost = parent as SplitView;
                    
                    splitViewHost.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (sender, args) =>
                    {
                        this.OnPaneToggled();
                    });
                    splitViewHost.RegisterPropertyChangedCallback(SplitView.DisplayModeProperty, (sender, args) =>
                    {
                        this.OnPaneToggled();
                    });
                    
                    this.OnPaneToggled();
                }
            };
        }

        /// <summary>
        /// コントロールテンプレート適用時イベント
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Remove the entrance animation on the item containers.
            for (int i = 0; i < this.ItemContainerTransitions.Count; i++)
            {
                if (this.ItemContainerTransitions[i] is EntranceThemeTransition)
                {
                    this.ItemContainerTransitions.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 指定したリストアイテムを選択状態にする
        /// </summary>
        /// <param name="item"></param>
        public void SetSelectedItem(ListViewItem item)
        {
            int index = -1;
            if (item != null)
            {
                index = this.IndexFromContainer(item);
            }

            for (int i = 0; i < this.Items.Count; i++)
            {
                var lvi = (ListViewItem)this.ContainerFromIndex(i);
                if (lvi != null)
                {
                    if (i != index)
                    {
                        lvi.IsSelected = false;
                    }
                    else if (i == index)
                    {
                        lvi.IsSelected = true;
                    }
                }
            }
        }

        /// <summary>
        /// キーイベント処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            var focusedItem = FocusManager.GetFocusedElement();

            switch (e.Key)
            {
                case VirtualKey.Up:
                    this.TryMoveFocus(FocusNavigationDirection.Up);
                    e.Handled = true;
                    break;

                case VirtualKey.Down:
                    this.TryMoveFocus(FocusNavigationDirection.Down);
                    e.Handled = true;
                    break;

                case VirtualKey.Space:
                case VirtualKey.Enter:
                    // Fire our event using the item with current keyboard focus
                    this.InvokeItem(focusedItem);
                    e.Handled = true;
                    break;

                default:
                    base.OnKeyDown(e);
                    break;
            }
        }

        /// <summary>
        /// This method is a work-around until the bug in FocusManager.TryMoveFocus is fixed.
        /// </summary>
        /// <param name="direction"></param>
        private void TryMoveFocus(FocusNavigationDirection direction)
        {
            if (direction == FocusNavigationDirection.Next || direction == FocusNavigationDirection.Previous)
            {
                FocusManager.TryMoveFocus(direction);
            }
            else
            {
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                if (control != null)
                {
                    control.Focus(FocusState.Programmatic);
                }
            }
        }

        /// <summary>
        /// メニューアイテムをクリックしたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemClickedHandler(object sender, ItemClickEventArgs e)
        {
            // Triggered when the item is selected using something other than a keyboard
            var item = this.ContainerFromItem(e.ClickedItem);
            this.InvokeItem(item);
        }

        /// <summary>
        /// 指定したアイテムを選択状態にする
        /// </summary>
        /// <param name="focusedItem"></param>
        private void InvokeItem(object focusedItem)
        {
            this.SetSelectedItem(focusedItem as ListViewItem);
            this.ItemInvoked?.Invoke(this, focusedItem as ListViewItem);

            if (this.splitViewHost.IsPaneOpen && (
                this.splitViewHost.DisplayMode == SplitViewDisplayMode.CompactOverlay ||
                this.splitViewHost.DisplayMode == SplitViewDisplayMode.Overlay))
            {
                this.splitViewHost.IsPaneOpen = false;
            }

            if (focusedItem is ListViewItem)
            {
                ((ListViewItem)focusedItem).Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        /// 画面サイズ変更時等により表示状態をセットしなおす
        /// </summary>
        private void OnPaneToggled()
        {
            if (this.splitViewHost.IsPaneOpen)
            {
                this.ItemsPanelRoot.ClearValue(FrameworkElement.WidthProperty);
                this.ItemsPanelRoot.ClearValue(FrameworkElement.HorizontalAlignmentProperty);
            }
            else if (this.splitViewHost.DisplayMode == SplitViewDisplayMode.CompactInline ||
                this.splitViewHost.DisplayMode == SplitViewDisplayMode.CompactOverlay)
            {
                this.ItemsPanelRoot.SetValue(FrameworkElement.WidthProperty, this.splitViewHost.CompactPaneLength);
                this.ItemsPanelRoot.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            }
        }
    }
}