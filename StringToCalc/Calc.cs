using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.Linq.Expressions;

namespace StringToCalc
{
    class Calc
    {
        public void enter()
        {
            //გამოძახების ძირითადი მეთოდი
            var working = true;

            while (working)
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("Enter Instractions: E - Exit, C - Clear\n");
                var userInp = Console.ReadLine();
                if (userInp.ToLower() == "e")
                {
                    break;
                }

                //დინამიური ხაზის გენერატორი
                var lineBot = "";
                for (int i = 0; i < userInp.Length; i++)
                {
                    lineBot += "-";
                }

                Console.WriteLine(lineBot);
                Console.WriteLine(GetValue(userInp).ToString("0.000"));
                Console.WriteLine("\n");

                if (userInp.ToLower() == "c")
                {
                    Console.Clear();
                }
            }

            Console.WriteLine("\n\nDavit Saginashvili - 2016");
        }


        #region ინფორმაციის მიღება და კალკულაციის მილსადენში გადაგზავნა
        public float GetValue(string input)
        {

            float result = 0;

            var curly = @"\(\d+.\d+\)";
            var CurlyReggo = new Regex(curly);

            var root = @"(\d+(\.\d+)*\^\d+(\.\d+)*)";
            var RootReggo = new Regex(root);

            var percent = @"(\d+(\.\d+)*\%\d+(\.\d+)*)";
            var PercentReggo = new Regex(percent);

            var multi = @"(\d+(\.\d+)*\*\d+(\.\d+)*)";
            var MultiReggo = new Regex(multi);

            var divide = @"(\d+(\.\d+)*\/\d+(\.\d+)*)";
            var DivideReggo = new Regex(divide);

            var plus = @"(\d+(\.\d+)*\+\d+(\.\d+)*)";
            var PlusReggo = new Regex(plus);

            var minus = @"(\d+(\.\d+)*\-\d+(\.\d+)*)";
            //var minus = @"(\d+(\.\d+)*\-\d+(\.\d+)*)(\-\d+(\.\d+)*)*";

            var MinuseReggo = new Regex(minus);


            while (CurlyReggo.IsMatch(input))
            {
                var localInp = input;
                var curlyInput = CurlyReggo.Match(input).ToString();
                input = curlyInput.Trim('(', ')');

                getToWork(ref input, ref result, RootReggo, PercentReggo, MultiReggo, DivideReggo, PlusReggo, MinuseReggo);

                var inp = input;

                input = localInp.Replace(curlyInput, inp);
            }

            getToWork(ref input, ref result, RootReggo, PercentReggo, MultiReggo, DivideReggo, PlusReggo, MinuseReggo);

            return result;
        }
        #endregion

        #region ძირითადი კალკულაციის "მილსადენი"
        private void getToWork(ref string input, ref float result, Regex RootReggo, Regex PercentReggo, Regex MultiReggo, Regex DivideReggo, Regex PlusReggo, Regex MinuseReggo)
        {
            while (RootReggo.IsMatch(input))
            {
                var calcInput = RootReggo.Match(input).ToString();
                result = RootFan(calcInput);
                input = input.Replace(calcInput, result.ToString());
            }

            while (PercentReggo.IsMatch(input))
            {
                var calcInput = PercentReggo.Match(input).ToString();
                result = PercentFan(calcInput);
                input = input.Replace(calcInput, result.ToString());
            }

            while (MultiReggo.IsMatch(input))
            {
                var calcInput = MultiReggo.Match(input).ToString();
                result = MultiFan(calcInput);
                input = input.Replace(calcInput, result.ToString());
            }

            while (DivideReggo.IsMatch(input))
            {
                var calcInput = DivideReggo.Match(input).ToString();
                result = DivideFan(calcInput);
                input = input.Replace(calcInput, result.ToString("0.000"));
            }

            while (PlusReggo.IsMatch(input))
            {
                var calcInput = PlusReggo.Match(input).ToString();
                result = PlusFan(calcInput);
                input = input.Replace(calcInput, result.ToString());
            }

            while (MinuseReggo.IsMatch(input))
            {
                var calcInput = MinuseReggo.Match(input).ToString();
                result = MinusFan(calcInput);
                input = input.Replace(calcInput, result.ToString());


            }
        }
        #endregion

        #region ხარისხი
        public float RootFan(string input)
        {

            var lisTest = input.Split('^').ToList();
            var result = new List<float>();
            for (int i = 0; i < lisTest.Count; i++)
            {
                result.Add(float.Parse(lisTest[i].ToString()));

            }

            var multiRes = result[0];

            for (int i = 0; i < result[1] - 1; i++)
            {
                multiRes *= multiRes;
            }

            return multiRes;
        }
        #endregion

        #region პროცენტი
        public float PercentFan(string input)
        {

            var lisTest = input.Split('%').ToList();
            var result = new List<float>();
            for (int i = 0; i < lisTest.Count; i++)
            {
                result.Add(float.Parse(lisTest[i].ToString()));

            }

            var multiRes = (result[0] * result[1]) / 100;


            return multiRes;
        }
        #endregion

        #region ნამრავლი
        public float MultiFan(string input)
        {

            var lisTest = input.Split('*').ToList();
            var result = new List<float>();
            for (int i = 0; i < lisTest.Count; i++)
            {
                result.Add(float.Parse(lisTest[i]));

            }

            var multiRes = result[0] * result[1];

            return multiRes;
        }
        #endregion

        #region განაყაოფი
        public float DivideFan(string input)
        {

            var lisTest = input.Split('/').ToList();
            var result = new List<float>();
            for (int i = 0; i < lisTest.Count; i++)
            {
                result.Add(float.Parse(lisTest[i]));

            }

            var multiRes = result[0] / result[1];

            return multiRes;
        }
        #endregion

        #region ჯამი
        public float PlusFan(string input)
        {

            var lisTest = input.Split('+').ToList();
            var result = new List<float>();
            for (int i = 0; i < lisTest.Count; i++)
            {
                result.Add(float.Parse(lisTest[i]));
            }

            return result.Sum();
        }
        #endregion

        #region სხვაობა
        public float MinusFan(string input)
        {
            var lisTest = input.Split('-').ToList();
            var result = new List<float>();
            for (int i = 0; i < lisTest.Count; i++)
            {
                result.Add(float.Parse(lisTest[i]));
            }

            return result[0] - result[1];
        }
        #endregion

    }
}
