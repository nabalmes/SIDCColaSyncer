using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCColaSyncer.Service
{
    public class ColaQuery
    {
        public static StringBuilder InsertColaStub(todo process)
        {
            var sQuery = new StringBuilder();
            switch (process)
            {
                case todo.InsertColaStubHeader:
                    sQuery.Append(@"INSERT INTO hct00 (transNum, transDate, reference, employeeID, Total, cancelled, status, branchCode, systemDate, idUser)
                            SELECT @transNum, @transDate, @reference, @employeeID, @Total, @cancelled, @status, @branchCode, @systemDate, @idUser FROM DUAL
                            WHERE NOT EXISTS (SELECT transNum FROM hct00 WHERE transNum = @transNum);");

                    break;
                case todo.InsertColaStubDetails:
                    sQuery.Append(@"INSERT INTO hct10 (detailNum, transNum, barcode, itemCode, itemDescription, uomCode, quantity, cost, sellingPrice, Total, systemDate, idUser, ColaID)
                            SELECT @detailNum, @transNum, @barcode, @itemCode, @itemDescription, @uomCode, @quantity, @cost, @sellingPrice, @Total, @systemDate, @idUser, @ColaID FROM DUAL
                            WHERE NOT EXISTS (SELECT detailNum FROM hct10 WHERE detailNum = @detailNum);");

                    break;


                //case todo.GetDuplicateMemberCode:
                //    sQuery.Append(@"SELECT DISTINCT t1.memberId AS CodeNumber
                //                    FROM cci00 t1
                //                    INNER JOIN cci00 t2 ON t1.memberId = t2.memberId
                //                    WHERE t1.memberNum < t2.memberNum;
                //                    ");
                //    break;


                default:
                    break;
            }

            return sQuery;
        }

        public enum todo
        {
            InsertColaStubHeader, InsertColaStubDetails

        }
    }
}
