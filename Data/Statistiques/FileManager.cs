using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Core;

namespace Data.Statistiques
{
    public class FileManager
    {
        #region Fields et Properties
        private readonly string _filePath;

        #endregion

        #region Constructor
        public FileManager(string filePath)
        {
            _filePath = filePath;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Charge les statistiques des joueurs depuis le fichier JSON.
        /// </summary>
        /// <returns>Liste des statistiques des joueurs.</returns>
        public List<PlayerStat> LoadPlayerStats()
        {
            if (!File.Exists(_filePath))
            {
                return new List<PlayerStat>();
            }

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<PlayerStat>>(json) ?? new List<PlayerStat>();
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(Logger.ELevelMessage.Error, $"Erreur lors du chargement des statistiques : {ex.Message}");
                return new List<PlayerStat>();
            }
        }

        /// <summary>
        /// Sauvegarde les statistiques des joueurs dans un fichier JSON.
        /// </summary>
        /// <param name="players">Liste des joueurs.</param>
        public void SavePlayerStats(List<PlayerStat> players)
        {
            try
            {
                string json = JsonSerializer.Serialize(players, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(Logger.ELevelMessage.Error, $"Erreur lors de l'enregistrement des statistiques : {ex.Message}");
            }
        }

        /// <summary>
        /// Crée un joueur s'il n'existe pas déjà.
        /// </summary>
        /// <param name="stat">Statistiques du joueur.</param>
        public void CreatePlayer(PlayerStat stat)
        {
            List<PlayerStat> players = LoadPlayerStats();

            // Vérifie si le joueur existe déjà
            if (players.Any(p => p.Id == stat.Id))
            {
                Logger.Instance.Log(Logger.ELevelMessage.Warning, $"Le joueur {stat.Name} existe déjà");
                return;
            }

            players.Add(stat);
            SavePlayerStats(players);
            Logger.Instance.Log(Logger.ELevelMessage.Info, $"Joueur {stat.Name} ajouté avec succès");
        }

        /// <summary>
        /// Met à jour les statistiques d'un joueur après une partie.
        /// </summary>
        /// <param name="player">Données du joueur.</param>
        /// <param name="haveWin">Indique si le joueur a gagné.</param>
        public void UpdatePlayerStat(PlayersData.Player player, bool haveWin)
        {
            List<PlayerStat> players = LoadPlayerStats();

            PlayerStat playerStat = players.FirstOrDefault(p => p.Name == player.Name);

            if (playerStat == null)
            {
                Logger.Instance.Log(Logger.ELevelMessage.Warning, $"Le joueur {player.Name} n'existe pas encore, création en cours");
                playerStat = new PlayerStat
                {
                    Id = player.PlayerId,
                    Name = player.Name,
                    GamesPlayed = 0,
                    GamesWon = 0,
                    BestScore = int.MaxValue,
                    LastGameDate = DateTime.Now,
                    BestScoreDate = DateTime.Now
                };
                players.Add(playerStat);
            }

            playerStat.GamesPlayed++;
            if (haveWin)
            {
                playerStat.GamesWon++;
            }
            if (player.CardScore < playerStat.BestScore)
            {
                playerStat.BestScore = player.CardScore;
                playerStat.BestScoreDate = DateTime.Now;
            }
            playerStat.LastGameDate = DateTime.Now;

            SavePlayerStats(players);
            Logger.Instance.Log(Logger.ELevelMessage.Warning, $"Statistiques de {player.Name} mises à jour");
        }

        #endregion
    }
}
