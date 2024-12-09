namespace hw
{
    public partial class Form1 : Form
    {


        Bitmap imgX = new Bitmap("x.jpg");

        Bitmap imgO = new Bitmap("o.jpg");

        Random random = new Random();

        bool[] moves;

        private Button[] buttons;

        bool compMove = false;

        private readonly int[][] winningCombinations = new int[][]
{
    new int[] { 0, 1, 2 }, // Горизонтальные
    new int[] { 3, 4, 5 },
    new int[] { 6, 7, 8 },
    new int[] { 0, 3, 6 }, // Вертикальные
    new int[] { 1, 4, 7 },
    new int[] { 2, 5, 8 },
    new int[] { 0, 4, 8 }, // Диагональные
    new int[] { 2, 4, 6 }
};
        public Form1()
        {
            InitializeComponent();
            buttons = new Button[] { button1, button2, button3, button4, button5, button6, button7, button8, button9 };
            moves = new bool[] { false, false, false, false, false, false, false, false, false };

        }



        private void button1_Click(object sender, EventArgs e)
        {

            Button clicked = sender as Button;
            int randomIndex;

            if (clicked == null) { return; }





            if (!compMove) // Ход игрока
            {
                MakePlayerMove(clicked);
                if (!CheckWinner(imgX) && !CheckDraw())
                {
                    compMove = true; // Передача хода компьютеру
                    MakeComputerMove();
                }
            }


        }

        private void button10_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Enabled = true;
            }
            ResetGame();

        }

        private bool CheckWinner(Bitmap symbol)
        {
            foreach (var combination in winningCombinations)
            {
                if (buttons[combination[0]].Image == symbol &&
                    buttons[combination[1]].Image == symbol &&
                    buttons[combination[2]].Image == symbol)
                {
                    return true;
                }

            }


            return false;
        }
        private bool CheckDraw()
        {
            foreach (bool move in moves)
            {
                if (!move) return false; // Если есть хотя бы одна незаполненная клетка, это не ничья
            }
            return true; // Если все клетки заполнены и нет победителя — ничья
        }

        private void ResetGame()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Enabled = true;
                buttons[i].Image = null;
            }

            // Обнуляем массив moves
            moves = new bool[buttons.Length];
            compMove = checkBox1.Checked; // Если установлен флаг, то ходит компьютер

            // Если компьютер начинает, сразу делаем ход
            if (compMove)
            {
                MakeComputerMove();
            }


        }


        private void MakePlayerMove(Button clicked)
        {
            int clickedIndex = Array.IndexOf(buttons, clicked);
            if (!moves[clickedIndex])
            {

                moves[clickedIndex] = true;
                buttons[clickedIndex].Image = imgX;
                buttons[clickedIndex].Enabled = false;

            }

            if (CheckWinner(imgX))
            {
                MessageBox.Show("Игрок X победил!");
                ResetGame();
                return;
            }

            if (CheckDraw())
            {
                MessageBox.Show("Ничья!");
                ResetGame();
                return;
            }

            compMove = false; //передать ход компьютеру
        }

 


        private void MakeComputerMove()
        {
            bool hasEmptyAreas = false;

            int randomIndex;


            if (radioButton1.Checked) // Легкий уровень
            {
                foreach (bool move in moves)
                {
                    if (!move)
                    {
                        hasEmptyAreas = true;
                        break;
                    }
                }

                if (hasEmptyAreas)
                {

                    do
                    {
                        randomIndex = random.Next(0, buttons.Length);
                    }
                    while (moves[randomIndex]);

                    moves[randomIndex] = true;
                    buttons[randomIndex].Image = imgO;
                    buttons[randomIndex].Enabled = false;

                }
            }
            else if(radioButton2.Checked) {

                // Сложный уровень - более умная логика (например, блокировка выигрыша)
                if (!MakeSmartMove()) // Если не удалось заблокировать или победить, делаем случайный ход
                {
                    do
                    {
                        randomIndex = random.Next(0, buttons.Length);
                    } while (moves[randomIndex]);

                    moves[randomIndex] = true;
                    buttons[randomIndex].Image = imgO;
                    buttons[randomIndex].Enabled = false;
                }

            }

            if (CheckWinner(imgO))
            {
                MessageBox.Show("Игрок O победил!");
                ResetGame();
                return;
            }

            if (CheckDraw())
            {
                MessageBox.Show("Ничья!");
                ResetGame();
                return;
            }


            compMove = false; //передать ход игроку
        }


        private bool MakeSmartMove()
        {
            // Логика для сложного уровня (например, блокируем возможную победу игрока или пытаемся выиграть)
            foreach (var combination in winningCombinations)
            {
                int[] line = new int[] { combination[0], combination[1], combination[2] };
                int emptySpot = -1;

                int xCount = 0, oCount = 0;
                foreach (int index in line)
                {
                    if (buttons[index].Image == imgX) xCount++;
                    else if (buttons[index].Image == imgO) oCount++;
                    else emptySpot = index; // Найдено пустое место
                }

                // Если есть два X и пустая ячейка, блокируем
                if (xCount == 2 && emptySpot != -1)
                {
                    moves[emptySpot] = true;
                    buttons[emptySpot].Image = imgO; 
                    buttons[emptySpot].Enabled = false;
                    return true;
                }

                // Если есть два O и пустая ячейка, пытаемся выиграть
                if (oCount == 2 && emptySpot != -1)
                {
                    moves[emptySpot] = true;
                    buttons[emptySpot].Image = imgO;
                    buttons[emptySpot].Enabled = false;
                    return true;
                }
            }
            return false; // Если не нашли, делаем случайный ход
        }


    }
}
