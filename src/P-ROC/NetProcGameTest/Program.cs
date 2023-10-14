using NetPinProc.Domain;
using NetPinProc.Domain.PinProc;
using System;
using System.ComponentModel;

namespace PinprocTest
{
    class Program
    {
        const bool SIMULATED = true;

        public static StarterGame.StarterGame game;
        private static BackgroundWorker worker;
        private static ConsoleLogger logger;
		private static bool wallStatus;
		private static bool flasherStatus;
		private static bool stepLampStatus;
		private static int currentLamp;

        static void Main(string[] args)
        {
			currentLamp = 0;
			wallStatus = false;
			flasherStatus = false;
			stepLampStatus = false;
            System.Threading.Thread.CurrentThread.Name = "Console Thread";
            logger = new ConsoleLogger() { LogLevel = LogLevel.Verbose };
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = false;

            worker.DoWork += new DoWorkEventHandler(worker_DoWork);

            worker.RunWorkerAsync();

            string line = Console.ReadLine();
            while (line != "q" && line != "quit" && line != "exit")
            {
                if (line == "e")
                {
                    game.EnableFlippers(true);
                }
                if (line == "d")
                {
                    game.EnableFlippers(true);
                }
                if (line == "c")
                {
                    game.Coils["divertorFlasher"].Pulse(50);
                }
                if (line == "r")
                {
                    game.PROC.Reset(1);
                }
				if (line == "f") {
					
					flasherStatus = !flasherStatus;
					//if (flasherStatus) {
					//	Console.WriteLine ("Spinning flashers on");
					//	game.spinning_flashers_on ();
					//} else {
					//	Console.WriteLine ("Spinning flashers off");
					//	game.spinning_flashers_off ();
					//}

					game.RunFlasherRoutine ();
				}

				// Step through lamps
				if (line == "s") {
					/*
					if (stepLampStatus)
						game.stop_stepping_lamps ();
					else
						game.step_lamps ();
					*/
					stepLampStatus = !stepLampStatus;
				}

				if (line == "w") {
					wallStatus = !wallStatus;

					if (wallStatus) {
						Console.WriteLine ("Wall scoops up");
						game.left_wall_up ();
						game.right_wall_up ();
					} else {
						Console.WriteLine ("Wall scoops down");
						game.left_wall_down ();
						game.right_wall_down ();
					}
					//game.test_servo ();
				}

				if (line == "l") {
					Console.WriteLine ("Flashing lamp " + currentLamp.ToString());
					game.flash_lamp ((byte)currentLamp);
					currentLamp++;
					if (currentLamp == 50)
						currentLamp = 0;
				}

                line = Console.ReadLine();
            }

        }

        static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                System.Threading.Thread.CurrentThread.Name = "p-roc thread";
                game = new StarterGame.StarterGame(logger, SIMULATED);
                game.setup();
                game.PROC.DmdDraw(new byte[128]);
                game.RunLoop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("FATAL ERROR: Could not load P-ROC device.");
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }



        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            game.EndRunLoop();

            game = null;
        }

        static string Matrix2String(byte[] matrix)
        {
            if (matrix.Length != 4096) return "";
            string result = "";
            //public static byte DMDFrameGetDot(ref DMDFrame Frame, int x, int y) { return Frame.buffer[y * Frame.size.Width + x]; }
            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    result += matrix[y * 128 + x];
                }
                result += "\n";
            }
            return result;
        }
    }
}
