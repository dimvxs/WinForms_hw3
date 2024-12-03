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
    new int[] { 0, 1, 2 }, // ��������������
    new int[] { 3, 4, 5 },
    new int[] { 6, 7, 8 },
    new int[] { 0, 3, 6 }, // ������������
    new int[] { 1, 4, 7 },
    new int[] { 2, 5, 8 },
    new int[] { 0, 4, 8 }, // ������������
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





            if (!compMove) // ��� ������
            {
                MakePlayerMove(clicked);
                if (!CheckWinner(imgX) && !CheckDraw())
                {
                    compMove = true; // �������� ���� ����������
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
                if (!move) return false; // ���� ���� ���� �� ���� ������������� ������, ��� �� �����
            }
            return true; // ���� ��� ������ ��������� � ��� ���������� � �����
        }

        private void ResetGame()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Enabled = true;
                buttons[i].Image = null;
            }

            // �������� ������ moves
            moves = new bool[buttons.Length];
            compMove = checkBox1.Checked; // ���� ���������� ����, �� ����� ���������

            // ���� ��������� ��������, ����� ������ ���
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
                MessageBox.Show("����� X �������!");
                ResetGame();
                return;
            }

            if (CheckDraw())
            {
                MessageBox.Show("�����!");
                ResetGame();
                return;
            }

            compMove = false; //�������� ��� ����������
        }

 


        private void MakeComputerMove()
        {
            bool hasEmptyAreas = false;

            int randomIndex;


            if (radioButton1.Checked) // ������ �������
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

                // ������� ������� - ����� ����� ������ (��������, ���������� ��������)
                if (!MakeSmartMove()) // ���� �� ������� ������������� ��� ��������, ������ ��������� ���
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
                MessageBox.Show("����� O �������!");
                ResetGame();
                return;
            }

            if (CheckDraw())
            {
                MessageBox.Show("�����!");
                ResetGame();
                return;
            }


            compMove = false; //�������� ��� ������
        }


        private bool MakeSmartMove()
        {
            // ������ ��� �������� ������ (��������, ��������� ��������� ������ ������ ��� �������� ��������)
            foreach (var combination in winningCombinations)
            {
                int[] line = new int[] { combination[0], combination[1], combination[2] };
                int emptySpot = -1;

                int xCount = 0, oCount = 0;
                foreach (int index in line)
                {
                    if (buttons[index].Image == imgX) xCount++;
                    else if (buttons[index].Image == imgO) oCount++;
                    else emptySpot = index; // ������� ������ �����
                }

                // ���� ���� ��� X � ������ ������, ���������
                if (xCount == 2 && emptySpot != -1)
                {
                    moves[emptySpot] = true;
                    buttons[emptySpot].Image = imgO;
                    buttons[emptySpot].Enabled = false;
                    return true;
                }

                // ���� ���� ��� O � ������ ������, �������� ��������
                if (oCount == 2 && emptySpot != -1)
                {
                    moves[emptySpot] = true;
                    buttons[emptySpot].Image = imgO;
                    buttons[emptySpot].Enabled = false;
                    return true;
                }
            }
            return false; // ���� �� �����, ������ ��������� ���
        }


    }
}
