using SIDCColaSyncer.Helper;
using SIDCColaSyncer.Infrastracture.Shared.Services;
using SIDCColaSyncer.Model.DTOs.SIDCAPIs.ColaAPI.GetColaStub;
using SIDCColaSyncer.Service;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SIDCColaSyncer.Accounts.Controller
{
    public class ColaController
    {
        public List<ColaStubResponseApiData> GetCOLAStubAPI()
        {
            {
                Console.WriteLine("\nConnecting to API.......");

                var result = new List<ColaStubResponseApiData>();

                SIDCAPIService sidcAPIService = new SIDCAPIService();
                //string token = sidcAPIService.SendAuthentication();

                var branchCode = AppSettingHelper.GetSetting("branchCode");
                //var token = AppSettingHelper.GetSetting("TOKEN");

                ColaStubRequest colaStubRequest = new ColaStubRequest()
                {
                   BranchCode = branchCode,
                   //Token = token,
                };

                var colaStubFetched = sidcAPIService.GetColaStub(colaStubRequest); // ✅ Correct

                List<ColaStubResponseApiData> finalResponseList = new List<ColaStubResponseApiData>(); // ✅ Declare it

                if (colaStubFetched?.Response?.Result?.Hct00 != null)
                {
                    foreach (var header in colaStubFetched.Response.Result.Hct00)
                    {
                        var matchingDetails = colaStubFetched.Response.Result.Hct10?
                            .Where(d => d.TransNum == header.TransNum)
                            .ToList();

                        var combined = new ColaStubResponseApiData
                        {
                            TransNum = header.TransNum,
                            TransDate = header.TransDate,
                            Reference = header.Reference,
                            EmployeeID = header.EmployeeID,
                            Total = header.Total,
                            Cancelled = header.Cancelled,
                            Status = header.Status,
                            SegmentCode = header.SegmentCode,
                            BusinessSegment = header.BusinessSegment,
                            BranchCode = header.BranchCode,
                            Remarks = header.Remarks,
                            SystemDate = header.SystemDate,
                            IdUser = header.IdUser,
                            Details = matchingDetails
                        };

                        finalResponseList.Add(combined);
                    }
                }

                Console.WriteLine("\nGathering data thru API connection is completed.");

                return finalResponseList;

            }
        }

        public void MainProcess(List<ColaStubResponseApiData> api)
        {
            using (var con = new MySQLHelper(Global.DestinationDatabase))
            {
                con.BeginTransaction();
                foreach (var item in api)
                {

                    InsertSofos2ColaStub(con, item);
                }
                    

                con.CloseConnection();
            }
        }

        public void MarkAsInserted(string branchCode)
        {
            string BASE_URL = Properties.Settings.Default.BASE_URL.TrimEnd('/');
            string COLA_STUB_URL = Properties.Settings.Default.COLA_STUB_URL.TrimStart('/');
            string fullUrl = $"{BASE_URL}/{COLA_STUB_URL}";

            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
            {
                { "branchCode", branchCode },
                { "isInsertUpdate", "1" }
            };

                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync(fullUrl, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error updating insert status: {response.StatusCode}");
                }
                else
                {
                    Console.WriteLine("Successfully marked data as inserted.");
                }
            }
        }

        private void InsertSofos2ColaStub(MySQLHelper conn2, ColaStubResponseApiData itemAPI)
        {
            try
            {
                // Insert Header
                var headerParam = new Dictionary<string, object>()
                    {
                        { "@transNum", itemAPI.TransNum },
                        { "@transDate", itemAPI.TransDate },
                        { "@reference", itemAPI.Reference },
                        { "@employeeID", itemAPI.EmployeeID ?? "" },
                        { "@Total", itemAPI.Total },
                        { "@cancelled", itemAPI.Cancelled },
                        { "@status", itemAPI.Status },
                        { "@segmentCode", itemAPI.SegmentCode ?? "" },
                        { "@businessSegment", itemAPI.BusinessSegment ?? "" },
                        { "@branchCode", itemAPI.BranchCode ?? "" },
                        { "@remarks", itemAPI.Remarks ?? "" },
                        { "@systemDate", itemAPI.SystemDate },
                        { "@idUser", itemAPI.IdUser ?? "" },
                    };

                conn2.ArgSQLCommand = ColaQuery.InsertColaStub(ColaQuery.todo.InsertColaStubHeader); // Replace with actual header insert query
                conn2.ArgSQLParam = headerParam;
                conn2.ExecuteMySQL();

                // Insert Details
                if (itemAPI.Details != null)
                {
                    foreach (var detail in itemAPI.Details)
                    {
                        var detailParam = new Dictionary<string, object>()
                {
                    { "@detailNum", detail.DetailNum },
                    { "@transNum", detail.TransNum },
                    { "@barcode", detail.Barcode ?? "" },
                    { "@itemCode", detail.ItemCode ?? "" },
                    { "@itemDescription", detail.ItemDescription ?? "" },
                    { "@uomCode", detail.UOMCode ?? "" },
                    { "@uomDescription", detail.UOMDescription ?? "" },
                    { "@quantity", detail.Quantity },
                    { "@cost", detail.Cost },
                    { "@sellingPrice", detail.SellingPrice },
                    { "@discount", detail.Discount ?? 0 },
                    { "@total", detail.Total },
                    { "@conversion", detail.Conversion ?? 0 },
                    { "@systemDate", detail.SystemDate },
                    { "@idUser", detail.IdUser ?? "" },
                    { "@colaID", detail.ColaID ?? "" },
                    { "@isCola", detail.IsCola }
                };

                        conn2.ArgSQLCommand = ColaQuery.InsertColaStub(ColaQuery.todo.InsertColaStubDetails); // Replace with actual detail insert query
                        conn2.ArgSQLParam = detailParam;
                        conn2.ExecuteMySQL();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
