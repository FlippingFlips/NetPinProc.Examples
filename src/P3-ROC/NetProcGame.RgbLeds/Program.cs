using NetPinProc.Domain;
using NetPinProc.Domain.Pdb;
using NetPinProc.Domain.PinProc;
using NetPinProc.Game.Sqlite;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetProcGame.RgbLeds
{
    internal class Program
    {
        static CancellationTokenSource source = new CancellationTokenSource();
        static async Task Main(string[] args)
        {
            try
            {
                //create context for database and create machine config
                using var ctx = new NetProcDbContext();
                ctx.InitializeDatabase(true, false);
                var mc = ctx.GetMachineConfiguration();

                //create rgb PDB P3-ROC game. Set true for simulated
                var game = new Game(MachineType.PDB, null, false);

                //run setup machine from machine.json
                //game.LoadConfig("./machine.json");

                game.SetUp();

                //run setup from config from database
                game.LoadConfig((MachineConfiguration)mc);

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

                //var led = new PRLEDRGB()
                //{
                //    pRedLED = new PRLED() { boardAddr = 0, LEDIndex = 12 },
                //    pBlueLED = new PRLED() { boardAddr = 0, LEDIndex = 13 },
                //    pGreenLED = new PRLED() { boardAddr = 0, LEDIndex = 14 }
                //};
                //PinProc.PRLEDRGBColor(proc.ProcHandle, ref led, 0xFF00);

                //run game loop
                await GameLoop(game);

                //close console
                Console.WriteLine("netprocgame closing...");
                game.EndRunLoop(); //run in case not cleaned up                
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        async static Task GameLoop(Game game)
        {
            await Task.Run(() =>
            {
                game.RunLoop();
            });
        }

        /// <summary>
        /// Runs a script on every led in machine.json
        /// </summary>
        /// <param name="game"></param>
        private static void ScriptedLEDS(Game game)
        {
            //create led script to cycle colors
            var script = new LEDScript[3]
            {
                    new LEDScript(){ Colour = new uint[] { 0xFF, 0, 0}, Duration = 1000, FadeTime = 2000},
                    new LEDScript(){ Colour = new uint[] { 0, 0xFF, 0}, Duration = 1000, FadeTime = 2000},
                    new LEDScript(){ Colour = new uint[] { 0, 0, 0xFF}, Duration = 1000, FadeTime = 2000}
            };

            //apply script to each led in the machine.json
            for (int i = 1; i < 6; i++)
            {
                game.LEDS["LED" + i].Script(script);
            }

            //game.Steppers["TestStepper"].Move(-1567);


            //game.LEDS["LED1"].Polarity = true;
            //game.LEDS["LED1"].ChangeColor(new uint[] { 0xFF, 0x0, 0x0 });

            //game.LEDS["LED1"].ChangeColor(new uint[] { 255, 255, 255 });

            //PdLeds.PDLEDS[0].WriteStep(1, 1000, 30);

            //servo.WriteServoPulseCount(300);            
        }
    }
}
