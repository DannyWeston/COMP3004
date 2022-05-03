namespace Core
{
    public class Simulation
    {
        private delegate void OnWorldUpdateHandler(World world);
        private OnWorldUpdateHandler _worldUpdated;

        private Thread _thread;
        private bool _finished;
        private object _finishedLock = new();

        World _world;

        int _tickRate;

        public Simulation(SimConfig config, IWorldListener listener)
        {
            _world = new World(config.Width, config.Height);
            
            _tickRate = config.TickRate;

            _worldUpdated += listener.OnWorldUpdate;

            // Tell the thread to run the "run" method
            _thread = new Thread(new ThreadStart(Run));
        }

        public void Start()
        {
            // Start simulation thread if not already done
            if (_thread.ThreadState == ThreadState.Unstarted) _thread.Start();
        }

        public void End()
        {
            lock (_finishedLock)
            {
                _finished = true;
            }
        }

        private void Run()
        {
            while (!_finished)
            {
                // Update the world
                _world.Update();

                // Notify listener
                _worldUpdated(_world);

                // Sleep if tickRate enabled
                if (0 < _tickRate) Thread.Sleep(1000 / _tickRate);
            }
        }
    }
}
