using System.Windows;
using System.Windows.Controls;

namespace R3Sample01.Behaviors
{
    public class ListBoxScrollIntoSelectedItemBehavior
    {
        public static bool GetScrollIntoSelectedItem(DependencyObject obj)
        {
            return (obj.GetValue(ScrollIntoSelectedItemProperty) as bool?) ?? false;
        }

        public static void SetScrollIntoSelectedItem(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollIntoSelectedItemProperty, value);
        }

        public static readonly DependencyProperty ScrollIntoSelectedItemProperty =
            DependencyProperty.RegisterAttached("ScrollIntoSelectedItem",
                typeof(bool),
                typeof(ListBoxScrollIntoSelectedItemBehavior),
                new PropertyMetadata(false, (d, e) =>
                {
                    if (d is ListBox lb && e.NewValue is bool val)
                    {

                        if (val)
                        {
                            lb.SelectionChanged += OnSelectionChanged;
                        }
                        else
                        {
                            lb.SelectionChanged -= OnSelectionChanged;
                        }
                    }
                }));

        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox lb
                && lb.SelectedItem != null
                && GetScrollIntoSelectedItem(lb))
            {
                lb.ScrollIntoView(lb.SelectedItem);
            }
        }
    }
}
