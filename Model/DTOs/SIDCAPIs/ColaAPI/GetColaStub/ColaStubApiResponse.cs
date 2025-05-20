using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCColaSyncer.Model.DTOs.SIDCAPIs.ColaAPI.GetColaStub
{
    public class ColaStubApiResponse
    {
        public ColaStubApiInnerResponse Response { get; set; }
    }

    public class ColaStubApiInnerResponse
    {
        [JsonProperty("status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("result")]
        public ColaStubResult Result { get; set; }
    }

    public class ColaStubResult
    {
        [JsonProperty("hct00")]
        public List<ColaStubResponseApiData> Hct00 { get; set; }

        [JsonProperty("hct10")]
        public List<ColaStubApiDetails> Hct10 { get; set; }
    }

    public class ColaStubResponseApiData
    {
        [JsonProperty("transNum")]
        public string TransNum { get; set; }

        [JsonProperty("transDate")]
        public string TransDate { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("employeeID")]
        public string EmployeeID { get; set; }

        [JsonProperty("Total")]
        public decimal Total { get; set; }

        [JsonProperty("cancelled")]
        public string Cancelled { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("segmentCode")]
        public string SegmentCode { get; set; }

        [JsonProperty("businessSegment")]
        public string BusinessSegment { get; set; }

        [JsonProperty("branchCode")]
        public string BranchCode { get; set; }

        [JsonProperty("remarks")]
        public string Remarks { get; set; }

        [JsonProperty("systemDate")]
        public string SystemDate { get; set; }

        [JsonProperty("idUser")]
        public string IdUser { get; set; }
        public List<ColaStubApiDetails> Details { get; set; }
    }

    public class ColaStubApiDetails
    {
        [JsonProperty("detailNum")]
        public string DetailNum { get; set; }

        [JsonProperty("transNum")]
        public string TransNum { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("itemCode")]
        public string ItemCode { get; set; }

        [JsonProperty("itemDescription")]
        public string ItemDescription { get; set; }

        [JsonProperty("uomCode")]
        public string UOMCode { get; set; }

        [JsonProperty("uomDescription")]
        public string UOMDescription { get; set; }

        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty("cost")]
        public decimal Cost { get; set; }

        [JsonProperty("sellingPrice")]
        public decimal SellingPrice { get; set; }

        [JsonProperty("discount")]
        public decimal? Discount { get; set; }

        [JsonProperty("Total")]
        public decimal Total { get; set; }

        [JsonProperty("conversion")]
        public decimal? Conversion { get; set; }

        [JsonProperty("systemDate")]
        public string SystemDate { get; set; }

        [JsonProperty("idUser")]
        public string IdUser { get; set; }

        [JsonProperty("colaid")]
        public string ColaID { get; set; }

        [JsonProperty("isCola")]
        public string IsCola { get; set; }
    }

}
