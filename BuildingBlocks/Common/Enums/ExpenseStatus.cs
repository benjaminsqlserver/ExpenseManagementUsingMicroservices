using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseManagement.BuildingBlocks.Common.Enums
{
    public enum ExpenseStatus
    {
        Draft = 0,
        Submitted = 1,
        PendingApproval = 2,
        Approved = 3,
        Rejected = 4,
        PendingReimbursement = 5,
        Reimbursed = 6,
        Cancelled = 7
    }
}
