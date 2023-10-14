using NetPinProc.Domain;
using NetPinProc.Game.Sqlite;

namespace NetPinProc.Data.GameWithDatabase
{
    /// <summary>
    /// Runs this program which will run a game, create a database and increment the times the machine powered up
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("NetProcGame with database....");

            //delete database everytime
            //var _game = new NetProcDataGameController(Domain.PinProc.MachineType.PDB, true, new ConsoleLogger(), true);

            //don't delete database
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