using MGroup.Solvers.DDM.Tests;

namespace DotNetClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MpiTestSuite.RunTestsWith5Processes();
		}
    }
}
