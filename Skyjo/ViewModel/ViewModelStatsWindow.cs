using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Core;
using Core.RelayCommand;
using Data.Statistiques;
using Skyjo.WindowManager;

namespace Skyjo.ViewModel
{
    public class ViewModelStatsWindow : ViewModelBase
    {
        #region Enum
        public enum ESortOptions
        {
            [Description("Nombre de parties jouées")]
            NumberGamePlayed,

            [Description("Nombre de parties gagnées")]
            NumberGameWon,

            [Description("Dernière partie jouée")]
            LastGamePlayed,

            [Description("Meilleur score")]
            BestScore
        }

        public enum ESortOrders
        {
            [Description("Croissant")]
            Croissant,

            [Description("Décroissant")]
            Decroissant
        }
        #endregion

        #region Fields et Properties
        private readonly NavigationService _navService;

        public string SelectedSortOption { get; set; }
        public List<string> SortOptions { get; }
        public string SelectedSortOrder { get; set; }
        public List<string> SortOrders { get; }
        public List<PlayerStat> PlayerStats { get; private set; }

        public ICommand SortCommand { get; }
        public ICommand CloseCommand { get; }
        #endregion

        #region Constructor
        public ViewModelStatsWindow(NavigationService navService)
        {
            _navService = navService;

            SortCommand = new RelayCommand(Command_SortCommand);
            CloseCommand = new RelayCommand(Command_CloseCommand);

            // Initialisation des options de tri
            SortOptions = Enum.GetValues(typeof(ESortOptions))
                .Cast<ESortOptions>()
                .Select(e => GetEnumDescription(e))
                .ToList();

            SortOrders = Enum.GetValues(typeof(ESortOrders))
                .Cast<ESortOrders>()
                .Select(e => GetEnumDescription(e))
                .ToList();

            SelectedSortOption = SortOptions.First();
            SelectedSortOrder = SortOrders.First();

            PlayerStats = _navService.StatManager.LoadPlayerStats();

            DisplayUpdate();
        }
        #endregion

        #region Private Methods
        private void Command_SortCommand()
        {
            if (PlayerStats == null || !PlayerStats.Any()) return;

            ESortOptions sortOption = (ESortOptions)Enum.Parse(typeof(ESortOptions), SortOptions.IndexOf(SelectedSortOption).ToString());
            bool isAscending = SelectedSortOrder == GetEnumDescription(ESortOrders.Croissant);

            PlayerStats = SortPlayerStats(PlayerStats, sortOption, isAscending);
            DisplayUpdate();
        }

        private List<PlayerStat> SortPlayerStats(List<PlayerStat> stats, ESortOptions option, bool ascending)
        {
            IEnumerable<PlayerStat> sortedStats = option switch
            {
                ESortOptions.NumberGamePlayed => stats.OrderBy(s => s.GamesPlayed),
                ESortOptions.NumberGameWon => stats.OrderBy(s => s.GamesWon),
                ESortOptions.LastGamePlayed => stats.OrderBy(s => s.LastGameDate),
                ESortOptions.BestScore => stats.OrderBy(s => s.BestScore),
                _ => stats
            };

            return ascending ? sortedStats.ToList() : sortedStats.Reverse().ToList();
        }

        private void Command_CloseCommand()
        {
            _navService.NavigateTo("Menu");
        }

        private void DisplayUpdate()
        {
            RaisePropertyChanged(nameof(SelectedSortOption));
            RaisePropertyChanged(nameof(SortOptions));
            RaisePropertyChanged(nameof(SelectedSortOrder));
            RaisePropertyChanged(nameof(SortOrders));
            RaisePropertyChanged(nameof(PlayerStats));
        }

        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
        }
        #endregion
    }
}