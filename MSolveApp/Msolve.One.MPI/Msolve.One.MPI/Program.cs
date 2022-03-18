using System;
using System.Collections.Generic;
using System.Linq;
using MGroup.Environments.Mpi;
using MGroup.Environments;
using MPI;
using MGroup.Environments.Tests.Topologies;
using MGroup.Environments.Tests;
namespace DotNetClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Starting up an MPI program!");
            //using (var mpi = new MPI.Environment(ref args))
            //{
            //    Console.WriteLine($"Process {MPI.Communicator.world.Rank}: Hello world!");
            //}

            Console.WriteLine("Starting MPI Tests...");
            Console.WriteLine("Runnign TestLocalNeighbors");
            TestLocalNeighbors();
            Console.WriteLine("Runnign TestRemoteNeighbors");
            TestRemoteNeighbors();
            Console.WriteLine("Runnign TestSendRecvTags");
            TestSendRecvTags();
        }

        #region TestLocalNeighbors
        public static void TestLocalNeighbors()
        {
            var topology = new Hexagon1DTopology().CreateNodeTopology();
            foreach (ComputeNodeCluster cluster in topology.Clusters.Values)
            {
                var mpiTags = new MpiP2PTransfers(topology, cluster);
                foreach (int nodeID in cluster.Nodes.Keys)
                {
                    int[] localNodesExpected = GetLocalNeighbors()[nodeID];
                    int[] localNodesComputed = mpiTags.GetLocalNeighborsOf(nodeID).ToArray();
                    if (Utilities.AreEqual(localNodesExpected, localNodesComputed))
                        Console.WriteLine("-> TestLocalNeighbors completed successfully!");
                    else
                        Console.WriteLine("-> There was an error in TestLocalNeighbors");

                }
            }
        }
        private static Dictionary<int, int[]> GetLocalNeighbors()
        {
            var localNodes = new Dictionary<int, int[]>();
            localNodes[0] = new int[] { 1 };
            localNodes[1] = new int[] { 0 };
            localNodes[2] = new int[] { 3 };
            localNodes[3] = new int[] { 2 };
            localNodes[4] = new int[] { 5 };
            localNodes[5] = new int[] { 4 };
            return localNodes;
        }
        #endregion

        #region TestRemoteNeighbors
        public static void TestRemoteNeighbors()
        {
            var topology = new Hexagon1DTopology().CreateNodeTopology();
            foreach (ComputeNodeCluster cluster in topology.Clusters.Values)
            {
                var mpiTags = new MpiP2PTransfers(topology, cluster);
                foreach (int nodeID in cluster.Nodes.Keys)
                {
                    int[] remoteNodesExpected = GetRemoteNeighbors()[nodeID];
                    int[] remoteNodesComputed = mpiTags.GetRemoteNeighborsOf(nodeID).ToArray();
                    if (Utilities.AreEqual(remoteNodesExpected, remoteNodesComputed))
                        Console.WriteLine("-> TestRemoteNeighbors completed successfully!");
                    else
                        Console.WriteLine("-> There was an error in TestRemoteNeighbors");
                }
            }
        }
        private static Dictionary<int, int[]> GetRemoteNeighbors()
        {
            var remoteNodes = new Dictionary<int, int[]>();
            remoteNodes[0] = new int[] { 5 };
            remoteNodes[1] = new int[] { 2 };
            remoteNodes[2] = new int[] { 1 };
            remoteNodes[3] = new int[] { 4 };
            remoteNodes[4] = new int[] { 3 };
            remoteNodes[5] = new int[] { 0 };
            return remoteNodes;
        }
        #endregion

        #region TestSendRecvTags
        public static void TestSendRecvTags()
        {
            var topology = new Hexagon1DTopology().CreateNodeTopology();


            foreach (ComputeNodeCluster cluster in topology.Clusters.Values)
            {
                var mpiTags = new MpiP2PTransfers(topology, cluster);
                foreach (int nodeID in cluster.Nodes.Keys)
                {
                    foreach (MpiJob job in Enum.GetValues(typeof(MpiJob)))
                    {
                        foreach (int remoteNeighborID in GetRemoteNeighbors()[nodeID])
                        {
                            int computedTag = mpiTags.GetSendRecvTag(job, nodeID, remoteNeighborID);
                            int expectedTag = GetTag(job, nodeID, remoteNeighborID);
                            if(expectedTag == computedTag)
                                Console.WriteLine("-> TestSendRecvTags completed successfully!");
                            else
                                Console.WriteLine("-> There was an error in TestSendRecvTags");
                        }
                    }
                }
            }
        }
        private static int GetTag(MpiJob job, int node0, int node1)
        {
            if (job == MpiJob.TransferBufferLengthDuringNeighborhoodAllToAll)
            {
                if ((Math.Min(node0, node1) == 0) && (Math.Max(node0, node1) == 5))
                {
                    return 0;
                }
                else if ((Math.Min(node0, node1) == 1) && (Math.Max(node0, node1) == 2))
                {
                    return 0;
                }
                else if ((Math.Min(node0, node1) == 3) && (Math.Max(node0, node1) == 4))
                {
                    return 0;
                }
                else
                {
                    throw new ArgumentException($"Nodes {node0}, {node1} are not neighboring remote nodes.");
                }
            }
            else if (job == MpiJob.TransferBufferDuringNeighborhoodAllToAll)
            {
                if ((Math.Min(node0, node1) == 0) && (Math.Max(node0, node1) == 5))
                {
                    return 1;
                }
                else if ((Math.Min(node0, node1) == 1) && (Math.Max(node0, node1) == 2))
                {
                    return 1;
                }
                else if ((Math.Min(node0, node1) == 3) && (Math.Max(node0, node1) == 4))
                {
                    return 1;
                }
                else
                {
                    throw new ArgumentException($"Nodes {node0}, {node1} are not neighboring remote nodes.");
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
