using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CA_1___Synchronized__fact__fib__sq_
{
    class Program
    {
        static int currentFactorial;
        static int currentFibonachi;
        static int currentSquare;
        static void Main(string[] args)
        {
            Console.WriteLine($"{"Factorial",-20} {"Fibonachi",-20} {"Square",-20}");
            for (int i = 0; i < 20; i++)
            {
                Thread thread1 = new Thread(Factorial);
                thread1.Start(i);
                Thread thread2 = new Thread(Fibonachi);
                thread2.Start(i);
                Thread thread3 = new Thread(Square);
                thread3.Start(i);
                Thread thread4 = new Thread(Show);
                thread4.Start();
            }


        }

        static void Factorial(object digit)
        {
            lock (digit)
            {
                if ((int)digit == 0)
                {
                    currentFactorial = 1;
                }
                else
                {
                    currentFactorial = (int)digit * currentFactorial;
                }
            }
        }

        static void Fibonachi(object digit)
        {
            lock (digit)
            {

                if ((int)digit == 0)
                {
                    currentFibonachi = 0;
                }
                else if ((int)digit == 1)
                {
                    currentFibonachi = 1;
                }
                else
                {
                    int beforePrev = 0;
                    int prev = 1;

                    for (int i = 0; i < (int)digit; i++)
                    {
                        currentFibonachi = beforePrev + prev;
                        beforePrev = prev;
                        prev = currentFibonachi;
                    }
                }
            }
        }

        static void Square(object digit)
        {
            lock (digit)
            {
                currentSquare = (int)digit * (int)digit;
            }
        }

        static void Show()
        {
            Console.WriteLine($"{currentFactorial, -20}  {currentFibonachi, -20}  {currentSquare, -20}");
        }
    }
}
