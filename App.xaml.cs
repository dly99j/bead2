using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using bead2.Model;
using bead2.View;
using bead2.ViewModel;
using bead2.Persistence;
using System.Windows.Threading;
using System.ComponentModel;

namespace bead2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        public EventHandler<GamePlayer> PlayerChanged;
        private GameModel _model;
        private GameViewModel _viewModel;
        private MainWindow _view;
        private DispatcherTimer _timer;
        private DispatcherTimer _refreshTimer;
        private Boolean isPaused;
        private GameDifficulty _gameLevel;

        #endregion

        #region Constructors

        /// <summary>
        /// Alkalmazás példányosítása.
        /// </summary>
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object sender, StartupEventArgs e)
        {

            // modell létrehozása
            _model = new GameModel(new GameDataAccess());
            _model.Over += new EventHandler<GameEventArgs>(Model_GameOver);

            // nézemodell létrehozása
            _viewModel = new GameViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_RestartGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LevelOne += new EventHandler(LevelOne);
            _viewModel.LevelTwo += new EventHandler(LevelTwo);
            _viewModel.LevelThree += new EventHandler(LevelThree);
            _viewModel.LoadGame += new EventHandler(ViewModel_RestartGame);
            _viewModel.KeyW += new EventHandler(ViewModel_WKey);
            _viewModel.KeyS += new EventHandler(ViewModel_SKey);
            _viewModel.KeyD += new EventHandler(ViewModel_DKey);
            _viewModel.KeyA += new EventHandler(ViewModel_AKey);
            _viewModel.KeyP += new EventHandler(ViewModel_PauseGame);

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();

            // időzítő létrehozása
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);

            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromMilliseconds(1);
            _refreshTimer.Tick += new EventHandler(RefreshTimer_Tick);


            SetupGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                _model.AdvanceTime();
                _viewModel.RefreshTable();
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                _model.RefreshTable();
                _viewModel.RefreshTable();
            }
        }

        #endregion

        #region View event handlers

        /// <summary>
        /// Nézet bezárásának eseménykezelője.
        /// </summary>
        private void View_Closing(object sender, CancelEventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;

            _timer.Stop();
            _refreshTimer.Stop();

            if (MessageBox.Show("Are you sure you want to exit?", "Sneaking Out the Game", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást

                if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                    _timer.Start();
            }
        }
        #endregion

        #region ViewModel event handlers

        private void ViewModel_WKey(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                _model.PlayerStep(GameDirection.Up);
                _model.RefreshTable();
                _viewModel.RefreshTable();
            }
        }

        private void ViewModel_SKey(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                _model.PlayerStep(GameDirection.Down);
                _model.RefreshTable();
                _viewModel.RefreshTable();
            }
        }

        private void ViewModel_DKey(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                _model.PlayerStep(GameDirection.Right);
                _model.RefreshTable();
                _viewModel.RefreshTable();
            }
        }

        private void ViewModel_AKey(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                _model.PlayerStep(GameDirection.Left);
                _model.RefreshTable();
                _viewModel.RefreshTable();
            }
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void ViewModel_StartGame(object sender, System.EventArgs e, GameDifficulty gameLevel, String fileName)
        {
            _gameLevel = gameLevel;

            try
            {
                await _model.LoadGameAsync(fileName);
                _viewModel.CreateNewTable();
            }
            catch (GameDataException)
            {
                MessageBox.Show("Hiba a betöltés közben!", "MaciLaci", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _timer.Start();
            _refreshTimer.Start();
            isPaused = false;
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        //private async void ViewModel_SaveGame(object sender, EventArgs e)
        //{
        //    Boolean restartTimer = _timer.IsEnabled;

        //    _timer.Stop();

        //    try
        //    {
        //        SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
        //        saveFileDialog.Title = "SneakingOut table loading";
        //        saveFileDialog.Filter = "SneakingOut table|*.txt";
        //        if (saveFileDialog.ShowDialog() == true)
        //        {
        //            try
        //            {
        //                // játéktábla mentése
        //                await _model.SaveGameAsync(saveFileDialog.FileName);
        //            }
        //            catch (SneakingOutDataException)
        //            {
        //                MessageBox.Show("Saving failed!" + Environment.NewLine + "The path is incorrect or the file can't be written!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Saving failed!", "SneakingOut the Game", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }

        //    if (restartTimer) // ha szükséges, elindítjuk az időzítőt
        //        _timer.Start();
        //}

        /// <summary>
        /// Játékból való kilépés eseménykezelője.
        /// </summary>
        private void ViewModel_ExitGame(object sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }

        private void LevelOne(object sender, System.EventArgs e)
        {
            ViewModel_StartGame(sender, e, GameDifficulty.Easy, @"..\..\..\table1.txt");
        }

        private void LevelTwo(object sender, System.EventArgs e)
        {
            ViewModel_StartGame(sender, e, GameDifficulty.Medium, @"..\..\..\table2.txt");
        }

        private void LevelThree(object sender, System.EventArgs e)
        {
            ViewModel_StartGame(sender, e, GameDifficulty.Hard, @"..\..\..\table3.txt");
        }


        /// <summary>
        /// Játék megallitasanak eseménykezelője.
        /// </summary>
        private void ViewModel_PauseGame(object sender, System.EventArgs e)
        {
            if (!isPaused)
            {
                isPaused = true;
                _timer.Stop();
                _refreshTimer.Stop();
            }
            else
            {
                isPaused = false;
                _timer.Start();
                _refreshTimer.Start();
            }
        }

        private void ViewModel_RestartGame(object sender, System.EventArgs e)
        {
            switch (_gameLevel)
            {
                case GameDifficulty.Easy:
                    LevelOne(sender, e);
                    break;
                case GameDifficulty.Medium:
                    LevelTwo(sender, e);
                    break;
                case GameDifficulty.Hard:
                    LevelThree(sender, e);
                    break;
            }
        }

        #endregion

        #region Model event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object sender, GameEventArgs e)
        {
            _timer.Stop();
            _refreshTimer.Stop();

            if (e.IsWon) // győzelemtől függő üzenet megjelenítése
            {
                MessageBox.Show("Gratu, nyertél",
                                "MaciLaci",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("Szar vagy",
                                "MaciLaci",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
        }

        public async void SetupGame()
        {
            _gameLevel = GameDifficulty.Easy;
            try
            {
                await _model.LoadGameAsync(@"..\..\..\table1.txt");
                _viewModel = new GameViewModel(_model);
                _viewModel.CreateNewTable();
            }
            catch (GameDataException)
            {
                MessageBox.Show("Hiba a betöltés közben!", "MaciLaci", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _timer.Start();
            _refreshTimer.Start();
            isPaused = false;
        }
        #endregion
    }
}

