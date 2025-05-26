using SIDCColaSyncer.Accounts.Controller;
using SIDCColaSyncer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIDCColaSyncer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                Console.WriteLine("The same program is already running in progress, please close this window ASAP.");
                System.Environment.Exit(1);


            }
            else
            {
                System.Console.WriteLine("COLA syncer tool will now begin.");
                //System.Console.ReadLine();
                try
                {

                    #region APIController
                    ColaController colaController = new ColaController();
                    var apiColaStub= colaController.GetCOLAStubAPI();
                    //var localColaStub = membersController.GetMembersLocal();
                    //var localFiles = membersController.GetFilesLocal();
                    //membersController.MainProcessA(localMembers, localFiles);
                    var branchCode = AppSettingHelper.GetSetting("branchCode");

                    if (apiColaStub.Count > 0)
                    {
                        colaController.MainProcess(apiColaStub);
                        colaController.MarkAsInserted(branchCode);
                        Console.WriteLine("\nAll processes are completed. Thank you!");
                        Thread.Sleep(10000);
                        System.Environment.Exit(1);

                    }
                    else
                    {
                        System.Environment.Exit(1);
                        //Console.WriteLine(AppSettingHelper.GetSetting("lastUpdateDate"));
                    }

                    #endregion MembersController
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Thread.Sleep(10000);


            }


        }
    }
}
