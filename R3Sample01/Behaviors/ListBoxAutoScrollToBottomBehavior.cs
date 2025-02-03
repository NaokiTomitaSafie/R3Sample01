using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;

namespace R3Sample01.Behaviors
{
    class ListBoxAutoScrollToBottomBehavior
    {
        public static ScrollViewer? GetScrollToBottomParent(DependencyObject obj)
        {
            return obj.GetValue(ScrollToBottomParentProperty) as ScrollViewer;
        }

        public static void SetScrollToBottomParent(DependencyObject obj, ScrollViewer value)
        {
            obj.SetValue(ScrollToBottomParentProperty, value);
        }

        public static readonly DependencyProperty ScrollToBottomParentProperty =
            DependencyProperty.RegisterAttached("ScrollToBottomParent",
                typeof(ScrollViewer),
                typeof(ListBoxAutoScrollToBottomBehavior),
                new PropertyMetadata(null, (d, e) =>
                {
                    if (d is FrameworkElement fe)
                    {
                        fe.SizeChanged += OnSizeChanged;
                    }
                }));
        private static void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is FrameworkElement fe)
            {
                GetScrollToBottomParent(fe)?.ScrollToBottom();
            }
        }
    }
}
