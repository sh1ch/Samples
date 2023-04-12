using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;

namespace EventOrderTest.Attached;


public class Initialized
{
    private static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(Initialized),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.None,
                (s, e) =>
                {
                    if (s is FrameworkElement control)
                    {
                        if (control == null)
                        {
                            Debug.WriteLine($"{typeof(Initialized)}.{CommandProperty}: control is null.");
                            return;
                        }

                        // CAUTION: Initialized は new したあとすぐに発生するので、添付プロパティでは補足できません
                        control.Initialized += (sender, args) =>
                        {
                            var command = GetCommand(control);

                            command?.Execute(e);
                        };
                    }
                })
            );

    public static ICommand GetCommand(DependencyObject d) => (ICommand)d.GetValue(CommandProperty);

    public static void SetCommand(DependencyObject d, ICommand value) => d.SetValue(CommandProperty, value);
}
