using NetPinProc.Domain;
using NetPinProc.Game.Sqlite;

namespace NetPinProc.Data.GameWithDatabase
{
    /// <summary>
    /// Runs this program which will create a new <see cref="NetProcDataGameController"/> <para/>
    /// Adjust parameters when creating a game below. Change to simulated or delete database on run. <para/>
    /// When game is created POWERED_ON_TIMES should increment and this is printed to the screen, each run should show value incremented from the database
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("NetProcGame with database....");

            //delete database everytime
            //var _game = new NetProcDataGameController(Domain.PinProc.MachineType.PDB, true, new ConsoleLogger(), true);

            //don't delete the database, run real board
            var _game = new NetProcDataGameController(Domain.PinProc.MachineType.PDB, false, new ConsoleLogger(), false);

            // Game data is saved when this program quits and when a game quits.
            // At the end of every game it's also saved. In a real game the game won't just quit like it does here
            //_game.SaveData();

            //when the game is run this is value is incremented so this should be different
            _game.Logger.Log($"POWERED_ON_TIMES={_game.GetAudit("POWERED_ON_TIMES")}");

            //quit game and save database
            _game.Quit();

            Console.ReadLine();
        }
    }
}