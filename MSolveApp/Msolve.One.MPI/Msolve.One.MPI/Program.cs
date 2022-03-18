using System;
//using MGroup.Environments.Mpi;
using MPI;
namespace DotNetClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting up an MPI program!");
            using (var mpi = new MPI.Environment(ref args))
            {
                Console.WriteLine($"Process {MPI.Communicator.world.Rank}: Hello world!");
            }
        }
    }
}
