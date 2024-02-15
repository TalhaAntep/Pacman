using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace CursorMovement
{
    class Program
    {
        public static void gameover(int score)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 4);
            Console.WriteLine("\n    ,----..      ,---,               ,'  , `.    ,---,.     " +
                                "\n   /   /   \\    '  .' \\           ,-+-,.' _ |  ,'  .' |     " +
                                "\n  |   :     :  /  ;    '.      ,-+-. ;   , ||,---.'   |     " +
                                "\n  .   |  ;. / :  :       \\    ,--.'|'   |  ;||   |   .'     " +
                                "\n  .   ; /--`  :  |   /\\   \\  |   |  ,', |  '::   :  |-,     " +
                                "\n  ;   | ;  __ |  :  ' ;.   : |   | /  | |  ||:   |  ;/|     " +
                                "\n  |   : |.' .'|  |  ;/  \\   \\'   | :  | :  |,|   :   .'     " +
                                "\n  .   | '_.' :'  :  | \\  \\ ,';   . |  ; |--' |   |  |-,     " +
                                "\n  '   ; : \\  ||  |  '  '--'  |   : |  | ,    '   :  ;/|     " +
                                "\n  '   | '/  .'|  :  :        |   : '  |/     |   |    \\     " +
                                "\n  |   :    /  |  | ,'        ;   | |`-'      |   :   .'     " +
                                "\n   \\   \\ .'   `--''          |   ;/          |   | ,'       " +
                                "\n    `---`                    '---'           `----'         " +
                                "\n      ,----..        SCORE: " + score +
                                "\n     /   /   \\                  ,---,.,-.----.              " +
                                "\n    /   .     :        ,---.  ,'  .' |\\    /  \\             " +
                                "\n   .   /   ;.  \\      /__./|,---.'   |;   :    \\            " +
                                "\n  .   ;   /  ` ; ,---.;  ; ||   |   .'|   | .\\ :            " +
                                "\n  ;   |  ; \\ ; |/___/ \\  | |:   :  |-,.   : |: |            " +
                                "\n  |   :  | ; | '\\   ;  \\ ' |:   |  ;/||   |  \\ :            " +
                                "\n  .   |  ' ' ' : \\   \\  \\: ||   :   .'|   : .  /            " +
                                "\n  '   ;  \\; /  |  ;   \\  ' .|   |  |-,;   | |  \\            " +
                                "\n   \\   \\  ',  /    \\   \\   ''   :  ;/||   | ;\\  \\           " +
                                "\n    ;   :    /      \\   `  ;|   |    \\:   ' | \\.'           " +
                                "\n     \\   \\ .'        :   \\ ||   :   .':   : :-'             " +
                                "\n      `---`           '---\" |   | , '  |   |.'               " +
                                "\n                            `----'    `---'                 ");
            Console.ReadKey();
        }
        public struct Player //the player
        {
            public char name;

            public int x;
            public int y;

            public int oldx;
            public int oldy;

            public int mines;
            public int score;
            public int energy;


            public Boolean isDead;

            public Player(char name, int x, int y)
            {
                this.name = name;

                this.x = x;
                this.y = y;

                this.oldx = x;
                this.oldy = y;

                this.mines = 0;
                this.score = 0;
                this.energy = 0;
                this.isDead = false;
            }

            public void savelocation(int newx, int newy)
            {
                this.oldx = this.x;
                this.oldy = this.y;

                this.x = newx;
                this.y = newy;
            }
            public void addscore(int score) { this.score = this.score + score; }
            public void addenergy(int energy) { this.energy = this.energy + energy; }
            public void addmines(int mines) { this.mines = this.mines + mines; } // also you can use mines by addmines(-1)

        }

        public static string[,] Enemy_arr = new string[500, 3];
        public static string[,] wall_coordinate = new string[55, 25];
        public static string[,] mine_coordinate = new string[55, 25];
        public static string[,] numb_coordinate = new string[55, 25];
        public static int cursorx;
        public static int cursory;

        public static bool checkMine(int x, int y)
        {
            if (mine_coordinate[x, y] == "+")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool checkEnemy(int x, int y)
        {
            bool final = false;
            for (int i = 0; i < 500; i++)
            {
                if (Enemy_arr[i, 1] == x.ToString() && Enemy_arr[i, 2] == y.ToString())
                {
                    final = true;
                    return final;
                }
                else
                {
                    final = false;
                }
            }
            return final;
        }

        public static void checkNumbers(int x, int y, Player p)
        {
            if (mine_coordinate[x, y] == "1")
            {
                p.addscore(10);
            }
            else if (mine_coordinate[x, y] == "2")
            {
                p.addscore(30);
                p.addenergy(50);
            }
            else if (mine_coordinate[x, y] == "3")
            {
                p.addscore(90);
                p.addmines(1);
                p.addenergy(200);
            }

        }

        static void Main(string[] args)
        {
            Player p = new Player('P', 0, 0);

            int counter_for_enmy = 0;
            // position of cursor Random olmalı
            ConsoleKeyInfo cki;               // required for readkey

            // --- Static screen parts
            Console.SetCursorPosition(3, 3);
            Console.WriteLine("#####################################################");
            for (int i = 0; i < 22; i++)
            {
                Console.SetCursorPosition(3, 3 + i + 1);
                Console.WriteLine("#                                                   #");
            }
            Console.SetCursorPosition(3, 25);
            Console.WriteLine("#####################################################");





            Random rnd = new Random();

            int[,] wall_position = { { 5, 5 }, { 10, 5 }, { 15, 5 }, { 20, 5 }, { 25, 5 }, { 30, 5 }, { 35, 5 }, { 40, 5 }, { 45, 5 }, { 50, 5 } };// for wall and x,y,p location.

            for (int aa = 0; aa < 4; aa++)// for rows
            {
                for (int ee = 0; ee < wall_position.GetLength(0); ee++) //for columns
                {
                    int wallx = wall_position[ee, 0];
                    int wally = wall_position[ee, 1];
                    int counter = 0; // to avoid create 4 wall in one area
                    for (int ii = 1; ii < 5; ii++)// for wall
                    {

                        int istherewall = rnd.Next(2);// we choose that if wall is or not.
                        if (counter < 3 && istherewall == 1 && ii == 1)
                        {
                            //we write wall to screen and store their location in wall_position array
                            Console.SetCursorPosition(wallx, wally + 5 * aa);
                            Console.Write("#");
                            Console.SetCursorPosition(wallx + 1, wally + 5 * aa);
                            Console.Write("#");
                            Console.SetCursorPosition(wallx + 2, wally + 5 * aa);
                            Console.Write("#");
                            Console.SetCursorPosition(wallx + 3, wally + 5 * aa);
                            Console.Write("#");
                            wall_coordinate[wallx, wally + 5 * aa] = "#";
                            wall_coordinate[wallx + 1, wally + 5 * aa] = "#";
                            wall_coordinate[wallx + 2, wally + 5 * aa] = "#";
                            wall_coordinate[wallx + 3, wally + 5 * aa] = "#";
                            counter++;
                        }
                        else if (counter < 3 && istherewall == 1 && ii == 2)
                        {
                            Console.SetCursorPosition(wallx, wally + 5 * aa);
                            Console.WriteLine("#");
                            Console.SetCursorPosition(wallx, wally + 1 + 5 * aa);
                            Console.WriteLine("#");
                            Console.SetCursorPosition(wallx, wally + 2 + 5 * aa);
                            Console.WriteLine("#");
                            Console.SetCursorPosition(wallx, wally + 3 + 5 * aa);
                            Console.WriteLine("#");
                            wall_coordinate[wallx, wally + 5 * aa] = "#";
                            wall_coordinate[wallx, wally + 1 + 5 * aa] = "#";
                            wall_coordinate[wallx, wally + 2 + 5 * aa] = "#";
                            wall_coordinate[wallx, wally + 3 + 5 * aa] = "#";
                            counter++;
                        }
                        else if (counter < 3 && istherewall == 1 && ii == 3)
                        {
                            Console.SetCursorPosition(wallx, wally + 3 + 5 * aa);
                            Console.Write("#");
                            Console.SetCursorPosition(wallx + 1, wally + 3 + 5 * aa);
                            Console.Write("#");
                            Console.SetCursorPosition(wallx + 2, wally + 3 + 5 * aa);
                            Console.Write("#");
                            Console.SetCursorPosition(wallx + 3, wally + 3 + 5 * aa);
                            Console.Write("#");
                            wall_coordinate[wallx, wally + 3 + 5 * aa] = "#";
                            wall_coordinate[wallx + 1, wally + 3 + 5 * aa] = "#";
                            wall_coordinate[wallx + 2, wally + 3 + 5 * aa] = "#";
                            wall_coordinate[wallx + 3, wally + 3 + 5 * aa] = "#";
                            counter++;

                        }
                        else if (counter < 3 && istherewall == 1 && ii == 4)
                        {
                            Console.SetCursorPosition(wallx + 3, wally + 5 * aa);
                            Console.WriteLine("#");
                            Console.SetCursorPosition(wallx + 3, wally + 1 + 5 * aa);
                            Console.WriteLine("#");
                            Console.SetCursorPosition(wallx + 3, wally + 2 + 5 * aa);
                            Console.WriteLine("#");
                            Console.SetCursorPosition(wallx + 3, wally + 3 + 5 * aa);
                            Console.WriteLine("#");
                            wall_coordinate[wallx + 3, wally + 5 * aa] = "#";
                            wall_coordinate[wallx + 3, wally + 1 + 5 * aa] = "#";
                            wall_coordinate[wallx + 3, wally + 2 + 5 * aa] = "#";
                            wall_coordinate[wallx + 3, wally + 3 + 5 * aa] = "#";
                            counter += 1;

                        }
                    }
                    // repeats the loop if no wall is created
                    if (counter == 0)
                    {
                        ee--;
                    }
                }

            }

            // we place x, y and p randomly on map.
            Random rand = new Random();
            while (true)
            {
                bool flag = true;
                Enemy_arr[0, 0] = "x";
                Enemy_arr[0, 1] = rand.Next(4, 55).ToString();
                Enemy_arr[0, 2] = rand.Next(4, 25).ToString();
                Enemy_arr[1, 0] = "x";
                Enemy_arr[1, 1] = rand.Next(4, 55).ToString();
                Enemy_arr[1, 2] = rand.Next(4, 25).ToString();
                Enemy_arr[2, 0] = "y";
                Enemy_arr[2, 1] = rand.Next(4, 55).ToString();
                Enemy_arr[2, 2] = rand.Next(4, 25).ToString();
                Enemy_arr[3, 0] = "y";
                Enemy_arr[3, 1] = rand.Next(4, 55).ToString();
                Enemy_arr[3, 2] = rand.Next(4, 25).ToString();
                cursorx = rand.Next(4, 55);
                cursory = rand.Next(4, 25);


                for (int i = 0; i < wall_coordinate.GetLength(0); i++)
                {
                    for (int j = 0; j < wall_coordinate.GetLength(1); j++)
                    {
                        if (wall_coordinate[i, j] == "#" && ((i == int.Parse(Enemy_arr[0, 1]) && j == int.Parse(Enemy_arr[0, 2])) || (i == int.Parse(Enemy_arr[1, 1]) && j == int.Parse(Enemy_arr[1, 2])) || (i == int.Parse(Enemy_arr[2, 1]) && j == int.Parse(Enemy_arr[2, 2])) || (i == int.Parse(Enemy_arr[3, 1]) && j == int.Parse(Enemy_arr[3, 2])) || (i == cursorx && j == cursory)))
                        {
                            flag = false;
                            break;
                        }
                        else if ((int.Parse(Enemy_arr[0, 1]) == int.Parse(Enemy_arr[1, 1]) && int.Parse(Enemy_arr[0, 2]) == int.Parse(Enemy_arr[1, 2])) || (int.Parse(Enemy_arr[0, 1]) == int.Parse(Enemy_arr[2, 1]) && int.Parse(Enemy_arr[0, 2]) == int.Parse(Enemy_arr[2, 1])) || (int.Parse(Enemy_arr[0, 1]) == int.Parse(Enemy_arr[3, 1]) && int.Parse(Enemy_arr[0, 2]) == int.Parse(Enemy_arr[3, 2])) || (int.Parse(Enemy_arr[2, 1]) == int.Parse(Enemy_arr[1, 2]) && int.Parse(Enemy_arr[2, 2]) == int.Parse(Enemy_arr[1, 2])) || (int.Parse(Enemy_arr[3, 1]) == int.Parse(Enemy_arr[1, 1]) && int.Parse(Enemy_arr[3, 2]) == int.Parse(Enemy_arr[1, 2])) || (int.Parse(Enemy_arr[2, 1]) == int.Parse(Enemy_arr[3, 1]) && int.Parse(Enemy_arr[2, 2]) == int.Parse(Enemy_arr[3, 2])))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (!flag)
                        break;
                }
                if (flag)
                    break;
            }
            wall_coordinate[int.Parse(Enemy_arr[0, 1]), int.Parse(Enemy_arr[0, 2])] = "x";
            wall_coordinate[int.Parse(Enemy_arr[1, 1]), int.Parse(Enemy_arr[1, 2])] = "x";
            wall_coordinate[int.Parse(Enemy_arr[2, 1]), int.Parse(Enemy_arr[2, 2])] = "y";
            wall_coordinate[int.Parse(Enemy_arr[3, 1]), int.Parse(Enemy_arr[3, 2])] = "y";




            int number;
            int numberx;
            int numbery;

            for (int k = 0; k < 20; k++)
            {
                while (true)      //LOCATE OF NUMBERS
                {
                    int[] arr = { 1, 1, 1, 1, 1, 1, 2, 2, 2, 3 };
                    number = rand.Next(0, 10);
                    number = arr[number];

                    numberx = rand.Next(4, 55);
                    numbery = rand.Next(4, 25);

                    if (wall_coordinate[numberx, numbery] == null && wall_coordinate[numberx, numbery] != "#")
                    {
                        break;
                    }

                }


                Console.SetCursorPosition(numberx, numbery);
                wall_coordinate[numberx, numbery] = number.ToString();
                numb_coordinate[numberx, numbery] = number.ToString();
                switch (number)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                }
                Console.WriteLine(number);
                Console.ResetColor();


            }
            int time = 0;
            int loopcounter = 0;
            p.energy = 200;


            int counter_for_new_enemy = 4;
            // --- Main game loop
            while (true)
            {

                if (checkEnemy(p.x, p.y))
                {
                    gameover(p.score);
                    break;
                }

                Console.SetCursorPosition(40, 27);
                Console.WriteLine("TIME : " + time);

                Console.SetCursorPosition(6, 27);
                if (p.energy >= 125) Console.ForegroundColor = ConsoleColor.Green;
                if (75 < p.energy && p.energy < 125) Console.ForegroundColor = ConsoleColor.Yellow;
                if (p.energy <= 75) Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ENERGY: " + p.energy + " ");
                Console.ResetColor();

                Console.SetCursorPosition(40, 26);
                Console.WriteLine("SCORE: " + p.score);

                Console.SetCursorPosition(6, 26);
                if (p.mines <= 0) Console.ForegroundColor = ConsoleColor.Red;
                else Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("MINES : " + p.mines);
                Console.ResetColor();

                if (counter_for_enmy % 150 == 0 && counter_for_enmy != 0)
                {
                    while (true)
                    {
                        int X_or_y = rand.Next(0, 2);
                        if (X_or_y == 1)
                        {

                            Enemy_arr[counter_for_new_enemy, 0] = "x";
                            Enemy_arr[counter_for_new_enemy, 1] = rand.Next(4, 55).ToString();
                            Enemy_arr[counter_for_new_enemy, 2] = rand.Next(4, 25).ToString();
                            if (wall_coordinate[Convert.ToInt16(Enemy_arr[counter_for_new_enemy, 1]), Convert.ToInt32(Enemy_arr[counter_for_new_enemy, 2])] == null && counter_for_new_enemy < 500)
                            {
                                counter_for_new_enemy++;
                                break;
                            }

                        }
                        else
                        {
                            Enemy_arr[counter_for_new_enemy, 0] = "y";
                            Enemy_arr[counter_for_new_enemy, 1] = rand.Next(4, 55).ToString();
                            Enemy_arr[counter_for_new_enemy, 2] = rand.Next(4, 25).ToString();
                            if (wall_coordinate[Convert.ToInt32(Enemy_arr[counter_for_new_enemy, 1]), Convert.ToInt32(Enemy_arr[counter_for_new_enemy, 2])] == null && counter_for_new_enemy < 500)
                            {
                                counter_for_new_enemy++;
                                break;
                            }

                        }
                    }

                }



                if (Console.KeyAvailable)
                {       // true: there is a key in keyboard buffer
                    cki = Console.ReadKey(true);       // true: do not write character 

                    if (cki.Key == ConsoleKey.RightArrow && cursorx < 54)
                    {   // key and boundary control
                        if (wall_coordinate[cursorx + 1, cursory] != "#")
                        {
                            wall_coordinate[cursorx, cursory] = null;

                            if (p.energy == 0 && loopcounter % 2 == 0)
                            {
                                Console.SetCursorPosition(cursorx, cursory);
                                Console.WriteLine(" ");
                                cursorx++;
                                wall_coordinate[cursorx, cursory] = "p";
                            }
                            else
                            {
                                Console.SetCursorPosition(cursorx, cursory);
                                Console.WriteLine(" ");
                                cursorx++;
                                wall_coordinate[cursorx, cursory] = "p";
                            }

                            if (p.energy > 0)
                                p.energy--;


                            checkNumbers(cursorx, cursory, p);
                            p.savelocation(cursorx, cursory);

                        }
                    }
                    if (cki.Key == ConsoleKey.LeftArrow && cursorx > 4)
                    {
                        if (wall_coordinate[cursorx - 1, cursory] != "#")
                        {
                            wall_coordinate[cursorx, cursory] = null;

                            if (p.energy == 0 && loopcounter % 2 == 0)
                            {
                                Console.SetCursorPosition(cursorx, cursory);
                                Console.WriteLine(" ");
                                cursorx--; wall_coordinate[cursorx, cursory] = "p";
                            }
                            else if (p.energy > 0)
                            {
                                Console.SetCursorPosition(cursorx, cursory);
                                Console.WriteLine(" ");
                                cursorx--; wall_coordinate[cursorx, cursory] = "p";
                            }
                            if (p.energy > 0)
                                p.energy--;

                            p.savelocation(cursorx, cursory);
                            checkNumbers(cursorx, cursory, p);

                        }
                    }
                    if (cki.Key == ConsoleKey.UpArrow && cursory > 4)
                    {
                        if (wall_coordinate[cursorx, cursory - 1] != "#")
                        {
                            wall_coordinate[cursorx, cursory] = null;

                            if (p.energy == 0 && loopcounter % 2 == 0)
                            {
                                Console.SetCursorPosition(cursorx, cursory);
                                Console.WriteLine(" ");
                                cursory--; wall_coordinate[cursorx, cursory] = "p";
                            }
                            else if (p.energy > 0)
                            {
                                Console.SetCursorPosition(cursorx, cursory);
                                Console.WriteLine(" ");
                                cursory--; wall_coordinate[cursorx, cursory] = "p";
                            }


                            if (p.energy > 0)
                                p.energy--;

                            p.savelocation(cursorx, cursory);
                            checkNumbers(cursorx, cursory - 1, p);

                        }
                    }
                    if (cki.Key == ConsoleKey.DownArrow && cursory < 24)
                    {
                        if (wall_coordinate[cursorx, cursory + 1] != "#")
                        {
                            wall_coordinate[cursorx, cursory] = null;



                            if (p.energy == 0 && loopcounter % 2 == 0)
                            {
                                Console.SetCursorPosition(cursorx, cursory);
                                Console.WriteLine(" ");
                                cursory++; wall_coordinate[cursorx, cursory] = "p";
                            }
                            else if (p.energy > 0)
                            {
                                Console.SetCursorPosition(cursorx, cursory);
                                Console.WriteLine(" ");
                                cursory++; wall_coordinate[cursorx, cursory] = "p";
                            }

                            if (p.energy > 0)
                                p.energy--;


                            p.savelocation(cursorx, cursory);
                            checkNumbers(cursorx, cursory, p);
                        }
                    }
                    if (cki.KeyChar == 32)
                    {       // keys: a-f 
                        if (p.mines > 0)
                        {
                            Console.SetCursorPosition(p.oldx, p.oldy);
                            Console.WriteLine("+");
                            mine_coordinate[p.oldx, p.oldy] = "+";
                            wall_coordinate[p.oldx, p.oldy] = "+";
                            p.addmines(-1);
                        }
                    }

                    if (cki.Key == ConsoleKey.Escape) break;
                }



                while (counter_for_enmy % 10 == 0)      //LOCATE OF NUMBERS
                {
                    int[] arr = { 1, 1, 1, 1, 1, 1, 2, 2, 2, 3 };
                    number = rand.Next(0, 10);
                    number = arr[number];

                    bool flag = true;
                    numberx = rand.Next(4, 55);
                    numbery = rand.Next(4, 25);

                    if (wall_coordinate[numberx, numbery] == null)
                    {
                        flag = false;

                        Console.SetCursorPosition(numberx, numbery);    
                        wall_coordinate[numberx, numbery] = number.ToString();
                        numb_coordinate[numberx, numbery] = number.ToString();
                        switch (number)
                        {
                            case 1:
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                        }
                        Console.WriteLine(number);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    if (flag) break;
                }

                if (numb_coordinate[cursorx, cursory] == "1")
                {
                    p.score += 10;
                    numb_coordinate[cursorx, cursory] = " ";
                }
                if (numb_coordinate[cursorx, cursory] == "2")
                {
                    p.score += 30;
                    p.energy += 50;
                    numb_coordinate[cursorx, cursory] = " ";
                }
                if (numb_coordinate[cursorx, cursory] == "3")
                {
                    p.score += 90;
                    p.energy += 200;
                    p.mines++;
                    numb_coordinate[cursorx, cursory] = " ";
                }

                for (int counter0 = 0; counter0 < Enemy_arr.GetLength(0); counter0++)
                {
                    if (Enemy_arr[counter0, 0] == "x" && Enemy_arr[counter0, 1] != null && Enemy_arr[counter0, 2] != null)
                    {
                        if (!adjust_location_of_X(Convert.ToInt32(Enemy_arr[counter0, 1]), Convert.ToInt32(Enemy_arr[counter0, 2]), cursorx, cursory, wall_coordinate, counter0))
                        {
                            Console.SetCursorPosition(Convert.ToInt32(Enemy_arr[counter0, 1]), Convert.ToInt32(Enemy_arr[counter0, 2]));  // refresh X (current position)
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("X");
                            Console.ResetColor();

                        }
                        else
                        {
                            numb_coordinate[cursorx, cursory] = null;
                            wall_coordinate[cursorx, cursory] = null;
                            mine_coordinate[cursorx, cursory] = null;
                            Console.SetCursorPosition(Convert.ToInt32(Enemy_arr[counter0, 1]), Convert.ToInt32(Enemy_arr[counter0, 2]));  // refresh X (current position)
                            Console.WriteLine(" ");

                        }

                    }
                    else if (Enemy_arr[counter0, 0] == "y" && Enemy_arr[counter0, 1] != null && Enemy_arr[counter0, 2] != null)
                    {
                        if (!adjust_location_of_Y(Convert.ToInt32(Enemy_arr[counter0, 1]), Convert.ToInt32(Enemy_arr[counter0, 2]), cursorx, cursory, wall_coordinate, counter0))
                        {
                            Console.SetCursorPosition(Convert.ToInt32(Enemy_arr[counter0, 1]), Convert.ToInt32(Enemy_arr[counter0, 2]));  // refresh X (current position)
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Y");
                            Console.ResetColor();

                        }
                        else
                        {
                            numb_coordinate[cursorx, cursory] = null;
                            wall_coordinate[cursorx, cursory] = null;
                            mine_coordinate[cursorx, cursory] = null;
                            Console.SetCursorPosition(Convert.ToInt32(Enemy_arr[counter0, 1]), Convert.ToInt32(Enemy_arr[counter0, 2]));
                            Console.WriteLine(" ");

                        }

                    }
                }

                //adjust_location_of_X(Convert.ToInt32(Enemy_arr[0,1]), Convert.ToInt32(Enemy_arr[0, 2]), cursorx, cursory, wall_coordinate);
                //adjust_location_of_Y(Convert.ToInt32(Enemy_arr[1, 1]), Convert.ToInt32(Enemy_arr[1, 2]), cursorx, cursory, wall_coordinate);




                //Console.SetCursorPosition(Xx, Xy);    // refresh X (current position)
                //Console.WriteLine("X");

                Console.SetCursorPosition(cursorx, cursory);    // refresh P (current position)
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("P");
                Console.ResetColor();





                int aa = rnd.Next(0, 4); // aa for row
                int ee = rnd.Next(0, 10); // ee for column
                int wallx = wall_position[ee, 0]; // position of wall of x axis 
                int wally = wall_position[ee, 1]; // position of wall of y axis
                int ii = rnd.Next(1, 5); // for which wall we will change
                int remove_or_add = rnd.Next(1, 3); // 1 remove, 2 add

                int counter_for_4_wall = 0; // if num of wall less than 3 and greater than 1 we can create a new wall or delete

                // look at the num of wall
                if (wall_coordinate[wallx, wally + 5 * aa] == "#" && wall_coordinate[wallx + 1, wally + 5 * aa] == "#" && wall_coordinate[wallx + 2, wally + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 5 * aa] == "#") { counter_for_4_wall += 1; }
                if (wall_coordinate[wallx, wally + 5 * aa] == "#" && wall_coordinate[wallx, wally + 1 + 5 * aa] == "#" && wall_coordinate[wallx, wally + 2 + 5 * aa] == "#" && wall_coordinate[wallx, wally + 3 + 5 * aa] == "#") { counter_for_4_wall += 1; }
                if (wall_coordinate[wallx, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 1, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 2, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] == "#") { counter_for_4_wall += 1; }
                if (wall_coordinate[wallx + 3, wally + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 1 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 2 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] == "#") { counter_for_4_wall += 1; }

                if (ii == 1) // ii is show us, which wall we delete or append.//    1.
                             //   ####
                             //2. #  # 4.
                             //   #  #
                             //   ####
                             //    3.
                {   // we check the num of wall greather than 1 and is there a wall at that location.
                    if (remove_or_add == 1 && counter_for_4_wall > 1 && wall_coordinate[wallx, wally + 5 * aa] == "#" && wall_coordinate[wallx + 1, wally + 5 * aa] == "#" && wall_coordinate[wallx + 2, wally + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 5 * aa] == "#")
                    {
                        // we check the other wall to avoid problems
                        if (wall_coordinate[wallx, wally + 5 * aa] == "#" && wall_coordinate[wallx, wally + 1 + 5 * aa] == "#" && wall_coordinate[wallx, wally + 2 + 5 * aa] == "#" && wall_coordinate[wallx, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 1 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 2 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] == "#")
                        {
                            Console.SetCursorPosition(wallx + 1, wally + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 2, wally + 5 * aa);
                            Console.Write(" ");
                            wall_coordinate[wallx + 1, wally + 5 * aa] = null;
                            wall_coordinate[wallx + 2, wally + 5 * aa] = null;
                        }
                        // we check the other wall to avoid problems
                        else if (wall_coordinate[wallx, wally + 5 * aa] == "#" && wall_coordinate[wallx, wally + 1 + 5 * aa] == "#" && wall_coordinate[wallx, wally + 2 + 5 * aa] == "#" && wall_coordinate[wallx, wally + 3 + 5 * aa] == "#")
                        {
                            Console.SetCursorPosition(wallx + 1, wally + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 2, wally + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 3, wally + 5 * aa);
                            Console.Write(" ");
                            wall_coordinate[wallx + 1, wally + 5 * aa] = null;
                            wall_coordinate[wallx + 2, wally + 5 * aa] = null;
                            wall_coordinate[wallx + 3, wally + 5 * aa] = null;
                        }
                        // we check the other wall to avoid problems
                        else if (wall_coordinate[wallx + 3, wally + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 1 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 2 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] == "#")

                        {
                            Console.SetCursorPosition(wallx, wally + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 1, wally + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 2, wally + 5 * aa);
                            Console.Write(" ");
                            wall_coordinate[wallx, wally + 5 * aa] = null;
                            wall_coordinate[wallx + 1, wally + 5 * aa] = null;
                            wall_coordinate[wallx + 2, wally + 5 * aa] = null;
                        }
                        // we check the other wall to avoid problems
                        else
                        {
                            Console.SetCursorPosition(wallx, wally + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 1, wally + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 2, wally + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 3, wally + 5 * aa);
                            Console.Write(" ");
                            wall_coordinate[wallx, wally + 5 * aa] = null;
                            wall_coordinate[wallx + 1, wally + 5 * aa] = null;
                            wall_coordinate[wallx + 2, wally + 5 * aa] = null;
                            wall_coordinate[wallx + 3, wally + 5 * aa] = null;
                        }
                    }
                    else // if there isnt any wall this point we append a new wall here.
                    {
                        if (counter_for_4_wall < 3 && wall_coordinate[wallx, wally + 5 * aa] != "y" && wall_coordinate[wallx, wally + 5 * aa] != "x" && wall_coordinate[wallx, wally + 5 * aa] != "p" && wall_coordinate[wallx, wally + 5 * aa] != "1" && wall_coordinate[wallx, wally + 5 * aa] != "2" && wall_coordinate[wallx, wally + 5 * aa] != "3" && wall_coordinate[wallx + 1, wally + 5 * aa] == null && wall_coordinate[wallx + 2, wally + 5 * aa] == null && wall_coordinate[wallx + 3, wally + 5 * aa] != "x" && wall_coordinate[wallx + 3, wally + 5 * aa] != "y" && wall_coordinate[wallx + 3, wally + 5 * aa] != "p" && wall_coordinate[wallx + 3, wally + 5 * aa] != "1" && wall_coordinate[wallx + 3, wally + 5 * aa] != "2" && wall_coordinate[wallx + 3, wally + 5 * aa] != "3")
                        {
                            Console.SetCursorPosition(wallx, wally + 5 * aa);
                            Console.Write("#");
                            Console.SetCursorPosition(wallx + 1, wally + 5 * aa);
                            Console.Write("#");
                            Console.SetCursorPosition(wallx + 2, wally + 5 * aa);
                            Console.Write("#");
                            Console.SetCursorPosition(wallx + 3, wally + 5 * aa);
                            Console.Write("#");
                            wall_coordinate[wallx, wally + 5 * aa] = "#";
                            wall_coordinate[wallx + 1, wally + 5 * aa] = "#";
                            wall_coordinate[wallx + 2, wally + 5 * aa] = "#";
                            wall_coordinate[wallx + 3, wally + 5 * aa] = "#";
                        }


                    }
                }
                else if (ii == 2)// ii is show us, which wall we delete or append.//    1.
                                 //   ####
                                 //2. #  # 4.
                                 //   #  #
                                 //   ####
                                 //    3.
                {
                    if (remove_or_add == 1 && counter_for_4_wall > 1 && wall_coordinate[wallx, wally + 5 * aa] == "#" && wall_coordinate[wallx, wally + 1 + 5 * aa] == "#" && wall_coordinate[wallx, wally + 2 + 5 * aa] == "#" && wall_coordinate[wallx, wally + 3 + 5 * aa] == "#")
                    {
                        if (wall_coordinate[wallx, wally + 5 * aa] == "#" && wall_coordinate[wallx + 1, wally + 5 * aa] == "#" && wall_coordinate[wallx + 2, wally + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 5 * aa] == "#" && wall_coordinate[wallx, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 1, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 2, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] == "#")
                        {
                            Console.SetCursorPosition(wallx, wally + 1 + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx, wally + 2 + 5 * aa);
                            Console.WriteLine(" ");
                            wall_coordinate[wallx, wally + 1 + 5 * aa] = null;
                            wall_coordinate[wallx, wally + 2 + 5 * aa] = null;

                        }
                        else if (wall_coordinate[wallx, wally + 5 * aa] == "#" && wall_coordinate[wallx + 1, wally + 5 * aa] == "#" && wall_coordinate[wallx + 2, wally + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 5 * aa] == "#")
                        {
                            Console.SetCursorPosition(wallx, wally + 1 + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx, wally + 2 + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx, wally + 3 + 5 * aa);
                            Console.WriteLine(" ");
                            wall_coordinate[wallx, wally + 1 + 5 * aa] = null;
                            wall_coordinate[wallx, wally + 2 + 5 * aa] = null;
                            wall_coordinate[wallx, wally + 3 + 5 * aa] = null;

                        }
                        else if (wall_coordinate[wallx, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 1, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 2, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] == "#")
                        {
                            Console.SetCursorPosition(wallx, wally + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx, wally + 1 + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx, wally + 2 + 5 * aa);
                            Console.WriteLine(" ");

                            wall_coordinate[wallx, wally + 5 * aa] = null;
                            wall_coordinate[wallx, wally + 1 + 5 * aa] = null;
                            wall_coordinate[wallx, wally + 2 + 5 * aa] = null;

                        }
                        else
                        {
                            Console.SetCursorPosition(wallx, wally + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx, wally + 1 + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx, wally + 2 + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx, wally + 3 + 5 * aa);
                            Console.WriteLine(" ");
                            wall_coordinate[wallx, wally + 5 * aa] = null;
                            wall_coordinate[wallx, wally + 1 + 5 * aa] = null;
                            wall_coordinate[wallx, wally + 2 + 5 * aa] = null;
                            wall_coordinate[wallx, wally + 3 + 5 * aa] = null;
                        }

                    }
                    else
                    {
                        if (counter_for_4_wall < 3 && wall_coordinate[wallx, wally + 5 * aa] != "x" && wall_coordinate[wallx, wally + 5 * aa] != "y" && wall_coordinate[wallx, wally + 5 * aa] != "p" && wall_coordinate[wallx, wally + 5 * aa] != "1" && wall_coordinate[wallx, wally + 5 * aa] != "2" && wall_coordinate[wallx, wally + 5 * aa] != "3" && wall_coordinate[wallx, wally + 1 + 5 * aa] == null && wall_coordinate[wallx, wally + 2 + 5 * aa] == null && wall_coordinate[wallx, wally + 3 + 5 * aa] != "x" && wall_coordinate[wallx, wally + 3 + 5 * aa] != "y" && wall_coordinate[wallx, wally + 3 + 5 * aa] != "p" && wall_coordinate[wallx, wally + 3 + 5 * aa] != "1" && wall_coordinate[wallx, wally + 3 + 5 * aa] != "2" && wall_coordinate[wallx, wally + 3 + 5 * aa] != "3")
                        {
                            Console.SetCursorPosition(wallx, wally + 5 * aa);
                            Console.WriteLine("#");
                            Console.SetCursorPosition(wallx, wally + 1 + 5 * aa);
                            Console.WriteLine("#");
                            Console.SetCursorPosition(wallx, wally + 2 + 5 * aa);
                            Console.WriteLine("#");
                            Console.SetCursorPosition(wallx, wally + 3 + 5 * aa);
                            Console.WriteLine("#");
                            wall_coordinate[wallx, wally + 5 * aa] = "#";
                            wall_coordinate[wallx, wally + 1 + 5 * aa] = "#";
                            wall_coordinate[wallx, wally + 2 + 5 * aa] = "#";
                            wall_coordinate[wallx, wally + 3 + 5 * aa] = "#";
                        }

                    }
                }
                else if (ii == 3)// ii is show us, which wall we delete or append.//    1.
                                 //   ####
                                 //2. #  # 4.
                                 //   #  #
                                 //   ####
                                 //    3.
                {
                    if (remove_or_add == 1 && counter_for_4_wall > 1 && wall_coordinate[wallx, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 1, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 2, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] == "#")
                    {
                        if (wall_coordinate[wallx, wally + 5 * aa] == "#" && wall_coordinate[wallx, wally + 1 + 5 * aa] == "#" && wall_coordinate[wallx, wally + 2 + 5 * aa] == "#" && wall_coordinate[wallx, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 1 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 2 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] == "#")
                        {
                            Console.SetCursorPosition(wallx + 1, wally + 3 + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 2, wally + 3 + 5 * aa);
                            Console.Write(" ");
                            wall_coordinate[wallx + 1, wally + 3 + 5 * aa] = null;
                            wall_coordinate[wallx + 2, wally + 3 + 5 * aa] = null;

                        }
                        else if (wall_coordinate[wallx, wally + 5 * aa] == "#" && wall_coordinate[wallx, wally + 1 + 5 * aa] == "#" && wall_coordinate[wallx, wally + 2 + 5 * aa] == "#" && wall_coordinate[wallx, wally + 3 + 5 * aa] == "#")
                        {

                            Console.SetCursorPosition(wallx + 1, wally + 3 + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 2, wally + 3 + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 3, wally + 3 + 5 * aa);
                            Console.Write(" ");
                            wall_coordinate[wallx + 1, wally + 3 + 5 * aa] = null;
                            wall_coordinate[wallx + 2, wally + 3 + 5 * aa] = null;
                            wall_coordinate[wallx + 3, wally + 3 + 5 * aa] = null;
                        }
                        else if (wall_coordinate[wallx + 3, wally + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 1 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 2 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] == "#")
                        {
                            Console.SetCursorPosition(wallx, wally + 3 + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 1, wally + 3 + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 2, wally + 3 + 5 * aa);
                            Console.Write(" ");
                            wall_coordinate[wallx, wally + 3 + 5 * aa] = null;
                            wall_coordinate[wallx + 1, wally + 3 + 5 * aa] = null;
                            wall_coordinate[wallx + 2, wally + 3 + 5 * aa] = null;

                        }
                        else
                        {
                            Console.SetCursorPosition(wallx, wally + 3 + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 1, wally + 3 + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 2, wally + 3 + 5 * aa);
                            Console.Write(" ");
                            Console.SetCursorPosition(wallx + 3, wally + 3 + 5 * aa);
                            Console.Write(" ");
                            wall_coordinate[wallx, wally + 3 + 5 * aa] = null;
                            wall_coordinate[wallx + 1, wally + 3 + 5 * aa] = null;
                            wall_coordinate[wallx + 2, wally + 3 + 5 * aa] = null;
                            wall_coordinate[wallx + 3, wally + 3 + 5 * aa] = null;
                        }
                    }
                    else
                    {
                        if (counter_for_4_wall < 3 && wall_coordinate[wallx, wally + 3 + 5 * aa] != "x" && wall_coordinate[wallx, wally + 3 + 5 * aa] != "y" && wall_coordinate[wallx, wally + 3 + 5 * aa] != "p" && wall_coordinate[wallx, wally + 3 + 5 * aa] != "1" && wall_coordinate[wallx, wally + 3 + 5 * aa] != "2" && wall_coordinate[wallx, wally + 3 + 5 * aa] != "3" && wall_coordinate[wallx + 1, wally + 3 + 5 * aa] == null && wall_coordinate[wallx + 2, wally + 3 + 5 * aa] == null && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] != "x" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] != "y" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] != "p" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] != "1" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] != "2" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] != "3")
                        {
                            Console.SetCursorPosition(wallx, wally + 3 + 5 * aa);
                            Console.Write("#");
                            Console.SetCursorPosition(wallx + 1, wally + 3 + 5 * aa);
                            Console.Write("#");
                            Console.SetCursorPosition(wallx + 2, wally + 3 + 5 * aa);
                            Console.Write("#");
                            Console.SetCursorPosition(wallx + 3, wally + 3 + 5 * aa);
                            Console.Write("#");
                            wall_coordinate[wallx, wally + 3 + 5 * aa] = "#";
                            wall_coordinate[wallx + 1, wally + 3 + 5 * aa] = "#";
                            wall_coordinate[wallx + 2, wally + 3 + 5 * aa] = "#";
                            wall_coordinate[wallx + 3, wally + 3 + 5 * aa] = "#";
                        }

                    }
                }
                else if (ii == 4)// ii is show us, which wall we delete or append.//    1.
                                 //   ####
                                 //2. #  # 4.
                                 //   #  #
                                 //   ####
                                 //    3.
                {
                    if (remove_or_add == 1 && counter_for_4_wall > 1 && wall_coordinate[wallx + 3, wally + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 1 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 2 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] == "#")
                    {

                        if (wall_coordinate[wallx, wally + 5 * aa] == "#" && wall_coordinate[wallx + 1, wally + 5 * aa] == "#" && wall_coordinate[wallx + 2, wally + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 5 * aa] == "#" && wall_coordinate[wallx, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 1, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 2, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] == "#")
                        {
                            Console.SetCursorPosition(wallx + 3, wally + 1 + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx + 3, wally + 2 + 5 * aa);
                            Console.WriteLine(" ");
                            wall_coordinate[wallx + 3, wally + 1 + 5 * aa] = null;
                            wall_coordinate[wallx + 3, wally + 2 + 5 * aa] = null;

                        }

                        else if (wall_coordinate[wallx, wally + 5 * aa] == "#" && wall_coordinate[wallx + 1, wally + 5 * aa] == "#" && wall_coordinate[wallx + 2, wally + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 5 * aa] == "#")
                        {
                            Console.SetCursorPosition(wallx + 3, wally + 1 + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx + 3, wally + 2 + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx + 3, wally + 3 + 5 * aa);
                            Console.WriteLine(" ");
                            wall_coordinate[wallx + 3, wally + 1 + 5 * aa] = null;
                            wall_coordinate[wallx + 3, wally + 2 + 5 * aa] = null;
                            wall_coordinate[wallx + 3, wally + 3 + 5 * aa] = null;

                        }
                        else if (wall_coordinate[wallx, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 1, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 2, wally + 3 + 5 * aa] == "#" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] == "#")
                        {

                            Console.SetCursorPosition(wallx + 3, wally + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx + 3, wally + 1 + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx + 3, wally + 2 + 5 * aa);
                            Console.WriteLine(" ");
                            wall_coordinate[wallx + 3, wally + 5 * aa] = null;
                            wall_coordinate[wallx + 3, wally + 1 + 5 * aa] = null;
                            wall_coordinate[wallx + 3, wally + 2 + 5 * aa] = null;

                        }
                        else
                        {
                            Console.SetCursorPosition(wallx + 3, wally + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx + 3, wally + 1 + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx + 3, wally + 2 + 5 * aa);
                            Console.WriteLine(" ");
                            Console.SetCursorPosition(wallx + 3, wally + 3 + 5 * aa);
                            Console.WriteLine(" ");
                            wall_coordinate[wallx + 3, wally + 5 * aa] = null;
                            wall_coordinate[wallx + 3, wally + 1 + 5 * aa] = null;
                            wall_coordinate[wallx + 3, wally + 2 + 5 * aa] = null;
                            wall_coordinate[wallx + 3, wally + 3 + 5 * aa] = null;

                        }
                    }
                    else
                    {
                        if (counter_for_4_wall < 3 && wall_coordinate[wallx + 3, wally + 5 * aa] != "x" && wall_coordinate[wallx + 3, wally + 5 * aa] != "y" && wall_coordinate[wallx + 3, wally + 5 * aa] != "p" && wall_coordinate[wallx + 3, wally + 5 * aa] != "1" && wall_coordinate[wallx + 3, wally + 5 * aa] != "2" && wall_coordinate[wallx + 3, wally + 5 * aa] != "3" && wall_coordinate[wallx + 3, wally + 1 + 5 * aa] == null && wall_coordinate[wallx + 3, wally + 2 + 5 * aa] == null && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] != "x" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] != "y" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] != "p" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] != "1" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] != "2" && wall_coordinate[wallx + 3, wally + 3 + 5 * aa] != "3")
                        {
                            Console.SetCursorPosition(wallx + 3, wally + 5 * aa);
                            Console.WriteLine("#");
                            Console.SetCursorPosition(wallx + 3, wally + 1 + 5 * aa);
                            Console.WriteLine("#");
                            Console.SetCursorPosition(wallx + 3, wally + 2 + 5 * aa);
                            Console.WriteLine("#");
                            Console.SetCursorPosition(wallx + 3, wally + 3 + 5 * aa);
                            Console.WriteLine("#");
                            wall_coordinate[wallx + 3, wally + 5 * aa] = "#";
                            wall_coordinate[wallx + 3, wally + 1 + 5 * aa] = "#";
                            wall_coordinate[wallx + 3, wally + 2 + 5 * aa] = "#";
                            wall_coordinate[wallx + 3, wally + 3 + 5 * aa] = "#";
                        }
                    }
                }
                counter_for_enmy++;
                loopcounter++;
                Thread.Sleep(200);
                if (loopcounter % 5 == 0)
                {
                    time++;
                }
            }
            Console.ReadLine();

        }


        public static bool adjust_location_of_X(int Xx, int Xy, int cursorx, int cursory, string[,] wall_coordinate, int counter0)
        {
            bool isdead = false;

            if (Xx > 3 && Xx < 55 && Xx < cursorx) // change direction at boundaries
            {
                if (wall_coordinate[Xx + 1, Xy] != "#" && wall_coordinate[Xx + 1, Xy] != "x" && wall_coordinate[Xx + 1, Xy] != "y")
                {
                    Console.SetCursorPosition(Xx, Xy);    // delete old X
                    Console.WriteLine(" ");

                    wall_coordinate[Xx, Xy] = null;
                    Xx++;

                    if (checkMine(Xx, Xy))
                    {
                        wall_coordinate[Xx, Xy] = null;
                        Enemy_arr[counter0, 0] = null;
                        Enemy_arr[counter0, 1] = null;
                        Enemy_arr[counter0, 2] = null;
                        isdead = true;
                        //return isdead;
                    }
                    else
                    {
                        wall_coordinate[Xx, Xy] = "x";
                        Enemy_arr[counter0, 1] = Xx.ToString();
                    }





                }
            }

            else if (Xx > 3 && Xx < 55 && Xx > cursorx)
            {
                if (wall_coordinate[Xx - 1, Xy] != "#" && wall_coordinate[Xx - 1, Xy] != "x" && wall_coordinate[Xx - 1, Xy] != "y")
                { 
                    Console.SetCursorPosition(Xx, Xy);    // delete old X
                    Console.WriteLine(" ");
                    wall_coordinate[Xx, Xy] = null;
                    Xx--;

                    if (checkMine(Xx, Xy))
                    {
                        wall_coordinate[Xx, Xy] = null;
                        Enemy_arr[counter0, 0] = null;
                        Enemy_arr[counter0, 1] = null;
                        Enemy_arr[counter0, 2] = null;
                        isdead = true;
                        //return isdead;
                    }
                    else
                    {
                        wall_coordinate[Xx, Xy] = "x";
                        Enemy_arr[counter0, 1] = Xx.ToString();
                    }




                }
            }
            else if (Xy > 3 && Xy < 25 && Xy < cursory)
            {
                if (wall_coordinate[Xx, Xy + 1] != "#" && wall_coordinate[Xx, Xy + 1] != "x" && wall_coordinate[Xx, Xy + 1] != "y")
                {
                    Console.SetCursorPosition(Xx, Xy);    // delete old X
                    Console.WriteLine(" ");
                    wall_coordinate[Xx, Xy] = null;
                    Xy++;

                    if (checkMine(Xx, Xy))
                    {
                        wall_coordinate[Xx, Xy] = null;
                        Enemy_arr[counter0, 0] = null;
                        Enemy_arr[counter0, 1] = null;
                        Enemy_arr[counter0, 2] = null;
                        isdead = true;
                        //return isdead;
                    }
                    else
                    {
                        wall_coordinate[Xx, Xy] = "x";
                        Enemy_arr[counter0, 2] = Xy.ToString();
                    }




                }
            }
            else if (Xy > 3 && Xy < 25 && Xy > cursory)
            {
                if (wall_coordinate[Xx, Xy - 1] != "#" && wall_coordinate[Xx, Xy - 1] != "x" && wall_coordinate[Xx, Xy - 1] != "y")
                {
                    Console.SetCursorPosition(Xx, Xy);    // delete old X
                    Console.WriteLine(" ");
                    wall_coordinate[Xx, Xy] = null;
                    Xy--;

                    if (checkMine(Xx, Xy))
                    {
                        wall_coordinate[Xx, Xy] = null;
                        Enemy_arr[counter0, 0] = null;
                        Enemy_arr[counter0, 1] = null;
                        Enemy_arr[counter0, 2] = null;
                        isdead = true;
                        //return isdead;
                    }
                    else
                    {
                        wall_coordinate[Xx, Xy] = "x";
                        Enemy_arr[counter0, 2] = Xy.ToString();
                    }


                }
            }
            if (isdead)
            {
                Console.SetCursorPosition(Xx, Xy);    // delete old X
                Console.WriteLine(" ");
                mine_coordinate[Xx, Xy] = null;
            }
            return isdead;
        }






        public static bool adjust_location_of_Y(int Yx, int Yy, int cursorx, int cursory, string[,] wall_coordinate, int counter0)
        {
            bool isdead = false;
            if (Yy > 3 && Yy < 25 && Yy < cursory)
            {
                if (wall_coordinate[Yx, Yy + 1] != "#" && wall_coordinate[Yx, Yy + 1] != "x" && wall_coordinate[Yx, Yy + 1] != "y")
                {
                    Console.SetCursorPosition(Yx, Yy);    // delete old X
                    Console.WriteLine(" ");
                    wall_coordinate[Yx, Yy] = null;
                    Yy++;

                    if (checkMine(Yx, Yy))
                    {
                        wall_coordinate[Yx, Yy] = null;
                        Enemy_arr[counter0, 0] = null;
                        Enemy_arr[counter0, 1] = null;
                        Enemy_arr[counter0, 2] = null;
                        isdead = true;
                        //return isdead;
                    }
                    else
                    {

                        wall_coordinate[Yx, Yy] = "y";
                        Enemy_arr[counter0, 2] = Yy.ToString();
                    }




                }
            }
            else if (Yy > 3 && Yy < 25 && Yy > cursory)
            {
                if (wall_coordinate[Yx, Yy - 1] != "#" && wall_coordinate[Yx, Yy - 1] != "x" && wall_coordinate[Yx, Yy - 1] != "y")
                {
                    Console.SetCursorPosition(Yx, Yy);    // delete old X
                    Console.WriteLine(" ");
                    wall_coordinate[Yx, Yy] = null;
                    Yy--;

                    if (checkMine(Yx, Yy))
                    {
                        wall_coordinate[Yx, Yy] = null;
                        Enemy_arr[counter0, 0] = null;
                        Enemy_arr[counter0, 1] = null;
                        Enemy_arr[counter0, 2] = null;
                        isdead = true;
                        //return isdead;
                    }
                    else
                    {
                        wall_coordinate[Yx, Yy] = "y";
                        Enemy_arr[counter0, 2] = Yy.ToString();
                    }



                }
            }
            else if (Yx > 3 && Yx < 55 && Yx < cursorx) // change direction at boundaries
            {
                if (wall_coordinate[Yx + 1, Yy] != "#" && wall_coordinate[Yx + 1, Yy] != "x" && wall_coordinate[Yx + 1, Yy] != "y")
                {
                    Console.SetCursorPosition(Yx, Yy);    // delete old X
                    Console.WriteLine(" ");
                    wall_coordinate[Yx, Yy] = null;
                    Yx++;

                    if (checkMine(Yx, Yy))
                    {
                        wall_coordinate[Yx, Yy] = null;
                        Enemy_arr[counter0, 0] = null;
                        Enemy_arr[counter0, 1] = null;
                        Enemy_arr[counter0, 2] = null;
                        isdead = true;
                        //return isdead;
                    }
                    else
                    {
                        wall_coordinate[Yx, Yy] = "y";
                        Enemy_arr[counter0, 1] = Yx.ToString();
                    }





                }
            }
            else if (Yx > 3 && Yx < 55 && Yx > cursorx)
            {
                if (wall_coordinate[Yx - 1, Yy] != "#" && wall_coordinate[Yx - 1, Yy] != "x" && wall_coordinate[Yx - 1, Yy] != "y")
                {
                    Console.SetCursorPosition(Yx, Yy);    // delete old X
                    Console.WriteLine(" ");
                    wall_coordinate[Yx, Yy] = null;
                    Yx--;

                    if (checkMine(Yx, Yy))
                    {
                        wall_coordinate[Yx, Yy] = null;
                        Enemy_arr[counter0, 0] = null;
                        Enemy_arr[counter0, 1] = null;
                        Enemy_arr[counter0, 2] = null;
                        isdead = true;
                        //return isdead;
                    }
                    else
                    {
                        wall_coordinate[Yx, Yy] = "y";
                        Enemy_arr[counter0, 1] = Yx.ToString();
                    }


                }
            }
            if (isdead)
            {
                Console.SetCursorPosition(Yx, Yy);    // delete old X
                Console.WriteLine(" ");
                mine_coordinate[Yx, Yy] = null;
            }
            return isdead;
        }

    }





}

