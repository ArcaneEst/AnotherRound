using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AnotherRound
{
    class MainForm : Form
    {
        Timer timer = new Timer();
        public Field Field { get; set; }
        public Controller Controller = new Controller();
        public Image Image;
        public int CurrentLevel;

        public void EndGame(string reason)
        {
            if (reason == "Dead")
            {
                TryRestart("Player is dead. Another round?");
            }
            else if (reason == "Win")
            {
                TryRestart("You win! Another round?");
            }
            else throw new ArgumentException();
        }

        private void TryRestart(string message)
        {
            var dialogResult = MessageBox.Show(message,
                    "Conformation", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                RestartGame();
            }
            else
                Close();
        }

        public void RestartGame()
        {
            timer = new Timer();
            Field = new Field(CurrentLevel);
            Controller = new Controller();
            StartGame();
        }

        public void StartGame()
        {
            Field.EndGameEvent += EndGame;
        }


        //Работа с формой
        #region
        /// <summary>
        /// Делает дела при загрузке классовой формы.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            Text = "Another Round?";
            WindowState = FormWindowState.Maximized;
            StartGame();
        }

        /// <summary>
        /// Основной метод формы. Работает, пока работает форма.
        /// </summary>
        public MainForm(int level)
        {
            Field = new Field(level);
            CurrentLevel = level;

            Image = new Bitmap(Field.FieldSize.Width, Field.FieldSize.Height, PixelFormat.Format32bppArgb);

            timer.Interval = 10;
            timer.Tick += TimerTick;

            timer.Start();
        }

        /// <summary>
        /// Сборный метод для тика таймера игры.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void TimerTick(object sender, EventArgs args)
        {
            ExecuteContrloller();
            Invalidate();
        }
        #endregion

        //Отрисовка поля
        #region
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.FillRectangle(Brushes.SandyBrown, ClientRectangle);

            var tableImage = new Bitmap(300, 300, PixelFormat.Format32bppArgb);
            var table = Graphics.FromImage(tableImage);
            GenerateInfoTable(table);
            e.Graphics.DrawImage(tableImage, 20, 50);

            var g = Graphics.FromImage(Image);
            DrawField(g);

            e.Graphics.DrawImage(Image, 
                ((ClientRectangle.Width - Image.Width) - 50), 
                (ClientRectangle.Height - Image.Height) / 2);
        }

        private void GenerateInfoTable(Graphics g)
        {
            var text = Field.ObjectsVault.GenerateInfoTable();
            g.FillRectangle(Brushes.White, 20, 50, 220, 60 + 30 * text.Length);
            DrawArcs(g, text.Length);

            for (var i = 0; i < text.Length; i++)
            {
                g.DrawString(text[i], new Font("Arial", 20), Brushes.Black, 50, 80 + 30 * i);
            }
        }

        private void DrawArcs(Graphics g, int textCount)
        {
            g.DrawArc(new Pen(Color.SandyBrown, 8), 15, 45, 40, 40, 180, 90);
            g.DrawArc(new Pen(Color.SandyBrown, 8), 205, 45, 40, 40, 270, 90);

            g.DrawArc(new Pen(Color.SandyBrown, 8), 15, 75 + 30 * textCount, 40, 40, 180, -90);
            g.DrawArc(new Pen(Color.SandyBrown, 8), 205, 75 + 30 * textCount, 40, 40, 90, -90);
        }

        /// <summary>
        /// Заполняет игровое поле объектами.
        /// </summary>
        /// <param name="g">Заполняемый объект графики.</param>
        private void DrawField(Graphics g)
        {
            g.FillRectangle(Brushes.White, ClientRectangle);
            DrawObstacles(g);
            DrawPlayer(g);
            DrawProjectiles(g);
        }

        /// <summary>
        /// Прорисовывает все пули.
        /// </summary>
        /// <param name="g">Заполняемый объект графики.</param>
        private void DrawProjectiles(Graphics g)
        {
            foreach (var proj in Field.Projectails.Projectails)
                g.FillCentredEllipse(Brushes.Green, proj.Location, proj.Size);
        }

        /// <summary>
        /// Рисует игрока.
        /// </summary>
        /// <param name="g">Заполняемый объект графики.</param>
        private void DrawPlayer(Graphics g)
        {
            if (Field.Player.IsCanBeHited)
                g.FillCentredEllipse(Brushes.Blue, Field.Player.Location, Field.Player.Size);
            else
                g.FillCentredEllipse(Brushes.Yellow, Field.Player.Location, Field.Player.Size);
        }
        /// <summary>
        /// Рисует препятствия.
        /// </summary>
        /// <param name="g">Заполняемый объект графики.</param>
        private void DrawObstacles(Graphics g)
        {
            foreach (var obstacle in Field.ObjectsVault.GetAllObstacles())
            {
                var obstacleBrush = Brushes.Brown;
                var removableBrush = Brushes.Red;
                var enemyBrush = Brushes.Black;
                var objectiveBrush = Brushes.YellowGreen;

                if (obstacle is Enemy)
                    g.FillCentredEllipse(enemyBrush, obstacle.Location, obstacle.Size);
                else if (obstacle is Objective)
                    g.FillCenterRectangle(objectiveBrush, obstacle.Location, obstacle.Size);
                else if (obstacle is CircleRemovable)
                    g.FillCentredEllipse(removableBrush, obstacle.Location, obstacle.Size);
                else if (obstacle is SquareRemovable)
                    g.FillCenterRectangle(removableBrush, obstacle.Location, obstacle.Size);
                else if (obstacle is ISquare)
                    g.FillCenterRectangle(obstacleBrush, obstacle.Location, obstacle.Size);
                else if (obstacle is ICircle)
                    g.FillCentredEllipse(obstacleBrush, obstacle.Location, obstacle.Size);
            }
        }
        #endregion

        //Работа с контроллером (вводом игрока)
        #region
        /// <summary>
        /// Вызывает класс контроллера для обработки введенных команд.
        /// </summary>
        private void ExecuteContrloller()
        {
            Controller.ExecuteContrloller(Field);
        }

        /// <summary>
        /// Ивенты при нажатии клавиши.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Controller.HandleKey(e.KeyCode, true);
        }

        /// <summary>
        /// Ивенты при подъеме клавиши.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            Controller.HandleKey(e.KeyCode, false);
        }
        #endregion
    }

    //Методы расширения
    #region
    public static class GrapficsExtenshion
    {
        /// <summary>
        /// Рисует на форме овал с центром location размера size кистью brush.
        /// </summary>
        /// <param name="graphics">Заполняемый объект графики.</param>
        /// <param name="brush">Кисть из Brushes</param>
        /// <param name="location">Вектор-координаты центра овала.</param>
        /// <param name="size">Размеры овала по осям.</param>
        public static void FillCentredEllipse(this Graphics graphics, Brush brush, Vector location, Size size)
        {
            graphics.FillEllipse(brush, 
                location.X - size.Width / 2,
                location.Y - size.Height / 2,
                size.Width, size.Height);
        }

        /// <summary>
        /// Рисует на форме прямоугольник с центром location размера size кистью brush.
        /// </summary>
        /// <param name="graphics">Заполняемый объект графики.</param>
        /// <param name="brush">Кисть из Brushes</param>
        /// <param name="location">Вектор-координаты центра прямоугольника</param>
        /// <param name="size">Размер прямоугольника.</param>
        public static void FillCenterRectangle(this Graphics graphics, Brush brush, Vector location, Size size)
        {
            graphics.FillRectangle(brush,
                location.X - size.Width / 2,
                location.Y - size.Height / 2,
                size.Width, size.Height);
        }
    }
    #endregion
}