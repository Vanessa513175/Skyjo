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
using System.Windows.Shapes;
using Data.PlayersData;
using Skyjo.ViewModel;
using Skyjo.WindowManager;

namespace Skyjo.Window
{
    /// <summary>
    /// Logique d'interaction pour ViewChoosePlayers.xaml
    /// </summary>
    public partial class ViewChoosePlayers : System.Windows.Window
    {
        public ViewChoosePlayers(NavigationService navService)
        {
            InitializeComponent();
            var viewModel = new ViewModelChoosePlayer(navService);
            DataContext = viewModel;
        }
        private void KnownPlayersListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && item.DataContext is Player selectedPlayer)
            {
                var viewModel = DataContext as ViewModelChoosePlayer;
                viewModel?.AddKnownPlayerToSelected(selectedPlayer);
            }
        }

    }
}
