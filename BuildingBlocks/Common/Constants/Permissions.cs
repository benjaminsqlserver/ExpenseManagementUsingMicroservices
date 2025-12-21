using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseManagement.BuildingBlocks.Common.Constants
{
    public static class Permissions
    {
        public const string ExpenseCreate = "expense.create";
        public const string ExpenseReadOwn = "expense.read.own";
        public const string ExpenseReadAll = "expense.read.all";
        public const string ExpenseUpdateOwn = "expense.update.own";
        public const string ExpenseDeleteOwn = "expense.delete.own";
        public const string ApprovalProcess = "approval.process";
        public const string ReimbursementProcess = "reimbursement.process";
        public const string ReportsView = "reports.view";
        public const string UsersManage = "users.manage";
    }
}
