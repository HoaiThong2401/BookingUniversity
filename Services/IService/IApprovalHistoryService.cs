using Repositories.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IApprovalHistoryService
    {
        void SaveApprovalHistory(ApprovalHistoryModel approvalHistoryModel, int userId);
    }
}
