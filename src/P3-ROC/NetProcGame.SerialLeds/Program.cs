using NetPinProc.Domain;
using NetPinProc.Domain.PinProc;
using NetPinProc.Game.Sqlite;
using NetProcGame.SerialLeds;

bool _configFromDb = true;

Console.WriteLine("PROC SerialLed Tests");
CancellationTokenSource source = new CancellationTokenSource();

try
{
    MachineConfiguration mc = null;

    //create rgb PDB P3-ROC game. Set true for simulated
    var game = new Game(MachineType.PDB, new ConsoleLogger(), false);

    if (_configFromDb)
    {
        //create context for database and create machine config
        using var ctx = new NetProcDbContext();
        ctx.InitializeDatabase(true, false);
        mc = ctx.GetMachineConfiguration();
    }
    else //run setup machine from machine.json
    {
        game.LoadConfig("./machine.json");
    }
    
    //setup init game
    game.SetUp();

    //setup machine items from the machine config
    game.LoadConfig(mc);

    //listen for cancel keypress and end run loop
    Console.CancelKeyPress += (sender, eventArgs) =>
    {
        Console.WriteLine("ctrl+C triggered");
        source.Cancel();
        eventArgs.Cancel = true;
        game.EndRunLoop();
    };

    //run scripted rgb leds
    ScriptedLEDS(game);

    //var serialChain = game.SerialLeds["SerialChain"];
    //serialChain.WriteColor(255);

    var stepper = game.Steppers["Stepper"];
    stepper.Move(-2500);

    //stepper.Move(0);

    //run game loop
    await GameLoop(game);

    //close console
    Console.WriteLine("netprocgame closing...");
    game.EndRunLoop(); //run in endloop in case we're not cleaned up
    await Task.Delay(500);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

/// <summary>Runs a script on every led in machine.json</summary>
/// <param name="game"></param>
void ScriptedLEDS(Game game) 
{ 

}

async static Task GameLoop(Game game, byte delay = 0, CancellationTokenSource cancellationToken = default)
    => await Task.Run(() => { game.RunLoop(delay, cancellationToken); });