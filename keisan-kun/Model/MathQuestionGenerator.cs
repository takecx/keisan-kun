using MaterialDesignColors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keisan_kun.Model
{
    public enum QuestionType
    {
        /// <summary>
        /// 一桁の正の整数の足し算（繰り上がりなし）
        /// </summary>
        add_single_no_carryup,
        /// <summary>
        /// 一桁の正の整数の足し算（繰り上がりあり）
        /// </summary>
        add_single_carryup,
        /// <summary>
        /// 一桁の正の整数の引き算（繰り下がりなし）
        /// </summary>
        sub_single_no_carrydown,
        /// <summary>
        /// 11から19までの数から一桁の正の整数の引き算（繰り下がりあり）
        /// </summary>
        sub_single_carrydown,
    }

    public class MathQuestionGenerator
    {
        private Random random1 = new Random(DateTime.Now.GetHashCode());
        private Random random2 = new Random(DateTime.Now.GetHashCode() + 1);
        private int randMin = 0;
        private int randMax = 10;
        private QuestionType questionType;

        public MathQuestionGenerator(QuestionType inQuestionType)
        {
            questionType = inQuestionType;
        }

        public (int,int) UpdateQuestion()
        {
            var first = random1.Next(randMin, randMax);
            var second = random2.Next(randMin, randMax);
            switch (questionType)
            {
                case QuestionType.add_single_no_carryup:
                    while ( first + second > 10)
                    {
                        (first, second) = GenNextRand();
                    }
                    break;
                case QuestionType.add_single_carryup:
                    while (first + second < 10)
                    {
                        (first, second) = GenNextRand();
                    }
                    break;
                case QuestionType.sub_single_no_carrydown:
                    first = Math.Max(first, second);
                    second = Math.Min(first, second);
                    break;
                case QuestionType.sub_single_carrydown:
                    first = random1.Next(11, 19);
                    if(first - second > 10)
                    {
                        second = random2.Next(randMin, randMax);
                    }
                    break;
                default:
                    throw new Exception();
            }
            return (first, second);
        }

        private (int first, int second) GenNextRand()
        {
            return (random1.Next(randMin, randMax), random2.Next(randMin, randMax));
        }

        public bool CheckAnswer(int first, int second, int? answer)
        {
            switch (questionType)
            {
                case QuestionType.add_single_no_carryup:
                case QuestionType.add_single_carryup:
                    return first + second == answer;
                case QuestionType.sub_single_no_carrydown:
                case QuestionType.sub_single_carrydown:
                    return first - second == answer;
                default:
                    throw new Exception();
            }
        }
    }
}
