﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace ModernUITestSample;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	private readonly ISnackbarService _snackbarService;

	public MainWindow()
	{
		_snackbarService = new SnackbarService();
		

		InitializeComponent();
	}

	private void CardAction_Click(object sender, RoutedEventArgs e)
	{
		_snackbarService.SetSnackbarControl(RootSnackbar);
		_snackbarService.Show("サンプルタイトル", "デイリークエストを達成しました。", SymbolRegular.Alert32, ControlAppearance.Primary);
	}
}
