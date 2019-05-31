using System;
using System.Threading;
using Logic.Interfaces;

namespace Logic.Examples
{
    public class OtherWork : IOtherWork
    {
        /// <summary>
        ///     A stupid example to take time
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public long SlowCalculation(long input)
        {
            // Base Case
            if (input == 0)
                return input;

            Thread.Sleep(10);
            return SlowCalculation(input - 1);
        }

        /// <summary>
        ///     Another stupid example to take time
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public void ImportantWork()
        {
            Console.WriteLine("This is super important work, trust me I'm an Engineer");
        }
    }
}