namespace Core
{
    public class Robot
    {
        private float _posX, _posY, _velocity, _rotation;

        public static readonly float Size = 25.0f;

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }
        public float PosX
        {
            get { return _posX; }
            set { _posX = value; }
        }
        public float PosY
        {
            get { return _posY; }
            set { _posY = value; }
        }
        public float Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        public Robot(float posX, float posY)
        {
            _posX = posX;
            _posY = posY;

            _rotation = _velocity = 0.0f;
        }
    }
}
