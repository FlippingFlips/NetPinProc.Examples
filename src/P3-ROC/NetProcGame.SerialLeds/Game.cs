using NetPinProc.Domain;
using NetPinProc.Domain.PinProc;
using NetPinProc.Game;

namespace NetProcGame.SerialLeds
{
    internal class Game : GameController
    {
        public Game(MachineType machineType, ILogger logger, bool Simulated = false) :
            base(machineType, logger, Simulated) { }

        internal void Setup()
        {
            LoadConfig(@"machine.json");
        }        
    }
}
