namespace Terminal_TI {

    public enum Directions {
        Up,
        Down,
        Left,
        Right
    };

    class Setings {

        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Speed { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }
        public static bool GameOver { get; set; }
        public static Directions directions { get; set; }

        public Setings() {
            Width = 16;
            Height = 16;
            Speed = 12;
            Score = 0;
            Points = 100;
            GameOver = false;
            directions = Directions.Right;
        }

    }
}
