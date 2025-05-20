using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCColaSyncer.Model.DTOs.SIDCAPIs.ColaAPI.GetColaStub
{
    public class ColaStubResponse : SIDCBaseResponse
    {
        public List<ColaStubResponseData> Data { get; set; }
    }

    public class ColaStubResponseData
    {
        public string TransNum { get; set; }
        public string TransDate { get; set; }
        public string Reference { get; set; }
        public string EmployeeID { get; set; }
        public decimal Total { get; set; }
        public string Cancelled { get; set; }
        public string Status { get; set; }
        public string SegmentCode { get; set; }
        public string BusinessSegment { get; set; }
        public string BranchCode { get; set; }
        public string Remarks { get; set; }
        public string SystemDate { get; set; }
        public string IdUser { get; set; }

        public ColaStubDetails Details { get; set; }
    }

    public class ColaStubDetails
    {
        public string DetailNum { get; set; }
        public string TransNum { get; set; }
        public string Barcode { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string UOMCode { get; set; }
        public string UOMDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public decimal Conversion { get; set; }
        public string SystemDate { get; set; }
        public string IdUser { get; set; }
        public string ColaID { get; set; }
    }
}

