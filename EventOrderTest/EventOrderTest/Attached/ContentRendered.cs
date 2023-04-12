using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;

namespace EventOrderTest.Attached;

public class ContentRendered
{
    private static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(ContentRendered),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.None,
                (s, e) =>
                {
                    if (s is Window window)
                    {
                        if (window == null)
                        {
                            Debug.WriteLine($"{typeof(Initialized)}.{CommandProperty}: window is null.");
                            return;
                        }

                        window.ContentRendered += (sender, args) =>
                        {
                            var command = GetCommand(window);

                            command?.Execute(e);
                        };
                    }
                })
            );

    public static ICommand GetCommand(DependencyObject d) => (ICommand)d.GetValue(CommandProperty);

    public static void SetCommand(DependencyObject d, ICommand value) => d.SetValue(CommandProperty, value);
}
