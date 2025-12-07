using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUMG.AssetManagement.Models
{
    public class AssetHistory
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public int ActionBy { get; set; }
    }
}
