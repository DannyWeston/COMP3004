namespace Core
{
    public class World
    {
        public readonly int Width, Height;

        private Robot _robot;

        public World(int width, int height)
        {
            Width = width;
            Height = height;

            // Make a new robot
            _robot = new(width / 2, height / 2)
            {
                Velocity = 1.0f,
            };
        }

        public Robot GetRobot()
        {
            return _robot;
        }

        public void Update()
        {
            // Update Robot's position
            float newX = _robot.PosX + (float)Math.Cos(_robot.Rotation) * _robot.Velocity;
            float newY = _robot.PosY + (float)Math.Sin(_robot.Rotation) * _robot.Velocity;

            // If coordinates exceed boundary, update accordingly
            if (newX < 1) newX = 1;
            else if (newX > Width - 2) newX = Width - 2;
            if (newY < 1) newY = 1;
            else if (newY > Height - 2) newY = Height - 2;

            _robot.PosX = newX;
            _robot.PosY = newY;

            // Increment robot's rotation so it circles for now
            _robot.Rotation += (float)Math.PI / 18;
        }
    }
}
