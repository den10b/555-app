using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace _555_app
{

    public partial class Form1 : Form
    {
        private static Dictionary<string, long> numberTable = new Dictionary<string, long>{
        {"zero",0},{"one",1},{"two",2},{"three",3},{"four",4},{"five",5},
        {"six",6},{"seven",7},{"eight",8},{"nine",9},

        {"ten",10}, {"eleven",11},{"twelve",12}, {"thirteen",13},{"fourteen",14},
        {"fifteen",15},{"sixteen",16},{"seventeen",17},{"eighteen",18},{"nineteen",19},

        {"twenty",20},{"thirty",30},{"forty",40}, {"fifty",50},
         {"sixty",60},{"seventy",70},{"eighty",80},{"ninety",90},

        {"hundred",100}};

        public static string ConvertToNumbers(string numberString)
        {
            var beg = Regex.Matches(numberString, @"\w+").Cast<Match>().Select(m => m.Value);
            var prew = Regex.Matches(numberString, @"\w+").Cast<Match>().Select(m => m.Value.ToLowerInvariant());

            bool ifWW = false;
            int WWpos = 10;
            string WWord = "";
            List<string> numsList = new List<string>();       //Список названий всех
            List<string> newList = new List<string>();         //список только правильных
            for (int i = 0; i < prew.Count(); i++)
            {
                numsList.Add(beg.ElementAt(i));                                    //Заполняем всеми
                if (numberTable.ContainsKey(beg.ElementAt(i).ToLowerInvariant()))   //Заполняем только правильными
                {
                    newList.Add(beg.ElementAt(i));
                }
                if (!numberTable.ContainsKey(prew.ElementAt(i)) && !ifWW)
                {
                    ifWW = true;
                    WWpos = i;
                    WWord = prew.ElementAt(i);
                }
            }


            /*
              Идеи ошибок
            one one
            twenty hundred
            hundred hundred
            twenty eleven
            eleven one

            Ничего => десятки
                   => teen
                   => единицы
                   => ошибка
            Ед => ничего
               => сотни
               => ошибка
            сотни  => десятки
                   => teen
                   => единицы
                   => Ничего
                   => ошибка
            десятки => единицы
                    => Ничего
                    => ошибка
            teen => Ничего
                 => ошибка

             Ничего сотни десятки   => единицы
                          Ничего   сотни  => teen

                   


            */
            var numbers = newList.Select(v => numberTable[v.ToLowerInvariant()]);

            long acc = 0, total = 0L;
            bool wsHund = false, wsTen = false, wsTeen = false, wsOne = false;
            bool wsNONE = true;
            bool isWP = false;
            bool HundEx = false;
            string wsWrd = "", WPword, WPtext = "";
            int WPpos = 10;

            for (int i = 0; i < numbers.Count(); i++)
            {
                var curNum = numbers.ElementAt(i);
                var curWord = newList.ElementAt(i);


                if (curNum >= 100) acc *= curNum;
                else acc += curNum;
                if (isWP) break;
                if (curNum < 10)
                {
                    if (wsOne)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("После {0} не может идти {1} Повторение единичных разрядов", wsWrd, curWord);
                    }
                    if (wsTeen)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("После {0} не может идти {1} Единичный разряд после чисел 10-19", wsWrd, curWord);
                    }
                    wsHund = false; wsTen = false; wsTeen = false; wsOne = true; wsNONE = false;
                }
                if (curNum > 9 && curNum < 20)
                {
                    if (wsOne)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("После {0} не может идти {1} Числа 10-19 после единичных разрядов", wsWrd, curWord);
                    }
                    if (wsTeen)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("После {0} не может идти {1} Повторение чисел 10-19", wsWrd, curWord);
                    }
                    if (wsTen)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("После {0} не может идти {1} Числа 10-19 после десятичных разрядов", wsWrd, curWord);
                    }

                    wsHund = false; wsTen = false; wsTeen = true; wsOne = false; wsNONE = false;
                }
                if (curNum > 19 && curNum < 100)
                {
                    if (wsOne)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("После {0} не может идти {1} Десятичные разряды после единичных разрядов", wsWrd, curWord);
                    }
                    if (wsTeen)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("После {0} не может идти {1} Десятичные разряды после чисел 10-19", wsWrd, curWord);
                    }
                    if (wsTen)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("После {0} не может идти {1} Повторение десятичных разрядов", wsWrd, curWord);
                    }
                    wsHund = false; wsTen = true; wsTeen = false; wsOne = false; wsNONE = false;
                }
                if (curNum == 100)
                {
                    if (HundEx && wsOne)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("После {0} не может идти {1} Разряд сотен после единичных разрядов", wsWrd, curWord);
                    }
                    if (wsTeen)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("После {0} не может идти {1} Разряд сотен после чисел 10-19", wsWrd, curWord);
                    }
                    if (wsTen)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("После {0} не может идти {1} Разряд сотен после десятичных разрядов", wsWrd, curWord);
                    }
                    if (wsHund)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("После {0} не может идти {1} Повторение разряда сотен", wsWrd, curWord);
                    }
                    if (wsNONE)
                    {
                        isWP = true;
                        WPpos = i;
                        WPword = newList.ElementAt(i);
                        WPtext = String.Format("Нельзя писать {0}, написанного без единичного разряда", curWord);
                    }



                    wsHund = true; wsTen = false; wsTeen = false; wsOne = false; wsNONE = false; HundEx = true;
                }
                wsWrd = curWord;
            }

            if (!isWP && !ifWW)
            {
                return ((total + acc) * (numberString.StartsWith("minus", StringComparison.InvariantCultureIgnoreCase) ? -1 : 1)).ToString();                
            }
            if (isWP && ifWW)
                if (WWpos <= WPpos) return String.Format("Ошибка синтаксиса, неизвестное слово {0}", WWord);
                else return WPtext;
            if (ifWW) return String.Format("Ошибка синтаксиса, неизвестное слово {0}", WWord);
            if (isWP) return WPtext;
            return " ";


        }
        public static string TurntoOld(int number)
        {
            string res="";
            while(number>=500)
            {
                res += "Ф";
                number -= 500;
            }
            while (number >= 100)
            {
                res += "Р";
                number -= 100;
            }
            while (number >= 30)
            {
                res += "Л";
                number -= 30;
            }
            while (number >= 8)
            {
                res += "И";
                number -= 8;
            }
            while (number >= 2)
            {
                res += "В";
                number -= 2;
            }
            while (number >= 1)
            {
                res += "А";
                number -= 1;
            }
            return res;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string txt = textBox1.Text;
            string rtnNumber = ConvertToNumbers(txt);
            textBox2.Text = rtnNumber;
            int numm;
            bool result = Int32.TryParse(textBox2.Text, out numm);
            textBox3.Text = " ";
            if (result) textBox3.Text = TurntoOld(numm);
        }

 
    }
}
