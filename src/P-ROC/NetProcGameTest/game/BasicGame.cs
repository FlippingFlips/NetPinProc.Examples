using NetPinProc.Dmd;
using NetPinProc.Domain;
using NetPinProc.Domain.PinProc;
using NetPinProc.Game;

namespace NetProcGameTest.game
{
    /// <summary>
    /// BasicGame is a subclass of GameController that includes and configures various useful
    /// helper classes to provide:
    ///     - DmdDisplayController
    /// </summary>
    /// 
    public class BasicGame : GameController
    {
        public DmdDisplayController dmd = null;
        public ScoreDisplay score_display = null;

		public BasicGame(MachineType machine_type, ILogger logger, bool simulated = false)
            : base(machine_type, logger, simulated)
        {
            FontManager manager = new FontManager(@"fonts/");
            if (machine_type == MachineType.WPCAlphanumeric)
            {
                // Create alphanumeric display
            }
            else
            {
                this.dmd = new DmdDisplayController(this, 128, 32, manager.FontName("Font07x5.dmd"));
            }

            this.score_display = new ScoreDisplay(this, 50);

            // The below code is for showing frames on the desktop
            //if (this.dmd != null) this.dmd.frame_handlers.Add(new DMDFrameHandler(this.set_last_frame));

            // Set up key map configs
        }

        /// <summary>
        /// Updates the DMD via (see <see cref="DmdDisplayController"/>)
        /// </summary>
        public override void DmdEvent() => dmd?.Update();

        public override void GameStarted()
        {
            score_display.Layer.Enabled = true;
            base.GameStarted();
        }

        /// <summary>
        /// Reset all core functionality and Add the ScoreDisplay _mode to the _mode queue
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this.Modes.Add(this.score_display);
        }
        public void Score(int points)
        {
            var p = this.CurrentPlayer();
            p.Score += points;
        }

        public override void Tick()
        {
            base.Tick();
            //this.show_last_frame();
        }
    }
}
