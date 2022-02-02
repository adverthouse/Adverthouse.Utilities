using System;

namespace Adverthouse.Core.IO
{
    public class ClusterNodeInfo
    {
        public string NodeName { get; set; }
        public double TotalSizeGB { get; set; }
        public double TotalFreeSpaceGB { get; set; }
        public bool Healthy { get; set; }
        public double AvailablePerc
        {
            get
            {
                return (TotalSizeGB == 0 ? 0 : Math.Round((100 * TotalFreeSpaceGB) / TotalSizeGB, 0));
            }
        }
        public double TotalUsedSpaceGB
        {
            get
            {
                return Math.Round(TotalSizeGB - TotalFreeSpaceGB, 2);
            }
        }
        public ClusterNodeInfo(string nodeName, double totalSizeMB, double totalFreeSpaceMB, bool healthy)
        {
            NodeName = nodeName;
            TotalSizeGB = Math.Round(totalSizeMB * 0.001, 2);
            TotalFreeSpaceGB = Math.Round(totalFreeSpaceMB * 0.001, 2);
            Healthy = healthy;
        }

        public ClusterNodeInfo()
        {
            TotalSizeGB = 0;
            TotalFreeSpaceGB = 1;
            Healthy = false;
        }
    }
}
