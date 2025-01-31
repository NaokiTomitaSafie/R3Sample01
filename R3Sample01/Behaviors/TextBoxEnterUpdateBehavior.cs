using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace R3Sample01.Behaviors
{
    public class TextBoxEnterUpdateBehavior
    {
        public static bool GetUpdateByEnter(DependencyObject obj)
        {
            return (obj.GetValue(UpdateByEnterProperty) as bool?) ?? false;
        }

        public static void SetUpdateByEnter(DependencyObject obj, bool value)
        {
            obj.SetValue(UpdateByEnterProperty, value);
        }

        public static readonly DependencyProperty UpdateByEnterProperty =
            DependencyProperty.RegisterAttached("UpdateByEnter",
                typeof(bool),
                typeof(TextBoxEnterUpdateBehavior),
                new PropertyMetadata(false, (d, e) =>
            {
                if (d is TextBox tb && e.NewValue is bool val)
                {

                    if (val)
                    {
                        tb.PreviewKeyDown += OnPreviewKeyDown;
                    }
                    else
                    {
                        tb.PreviewKeyDown -= OnPreviewKeyDown;
                    }
                }
            }));

        private static void OnPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (sender is TextBox tb
                && e.Key == System.Windows.Input.Key.Enter
                && GetUpdateByEnter(tb))
            {
                var be = BindingOperations.GetBindingExpression(tb, TextBox.TextProperty);
                be?.UpdateSource();
                e.Handled = true;
            }
        }
    }
}
