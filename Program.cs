using System;

namespace MGroup.LinearAlgebra.Distributed.Tests
{
	class Program
	{
		static void Main(string[] args)
		{
			HelloWorldMpi(args);
		}

		public static void HelloWorldMpi(string[] args)
		{
			using (new MPI.Environment(ref args))
			{
				Console.WriteLine($"Process {MPI.Communicator.world.Rank}: Hello world!");
			}
		}
	}
}
