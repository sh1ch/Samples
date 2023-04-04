using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;

namespace ContentRenderedSample;

/// <summary>
/// <see cref="ContentRendered"/> クラスは、<see cref="Window.ContentRendered"/> イベントを <see cref="UserControl"/> クラスで利用するための添付プロパティを定義するクラスです。
/// </summary>
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
                    if (s is UserControl control)
                    {
                        if (control == null)
                        {
                            Debug.WriteLine($"{typeof(ContentRendered)}.{CommandProperty}: control is null.");
                            return;
                        }

                        var window = Window.GetWindow(control);

                        if (window == null)
                        {
                            Debug.WriteLine($"{typeof(ContentRendered)}.{CommandProperty}: parent window is null.");
                            return;
                        }

                        window.ContentRendered += (sender, args) =>
                        {
                            var command = GetCommand(control);

                            command?.Execute(e);
                        };
                    }
                })
            );

    public static ICommand GetCommand(DependencyObject d)
    {
        return (ICommand)d.GetValue(CommandProperty);
    }

    public static void SetCommand(DependencyObject d, ICommand value)
    {
        d.SetValue(CommandProperty, value);
    }
}

