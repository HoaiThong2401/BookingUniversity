﻿using Microsoft.AspNetCore.Http;
using Repositories.Entities;
using Repositories.IRepository;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.ViewModel;

namespace Services.Service
{
    public class ApprovalHistoryService:IApprovalHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApprovalHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
            public void SaveApprovalHistory(ApprovalHistoryModel approvalHistoryModel, int userId)
            {
                var existingApprovalHistory = _unitOfWork.ApprovalHistoryRepository.Entities
                    .FirstOrDefault(ah => ah.Id == approvalHistoryModel.Id);

                if (existingApprovalHistory == null)
                {
                    var newApprovalHistory = new ApprovalHistory
                    {
                        CampusId = approvalHistoryModel.CampusId,
                        DepartmentId = approvalHistoryModel.DepartmentId,
                        UserBookingId = approvalHistoryModel.UserBookingId,
                        BookingId = approvalHistoryModel.BookingId,
                        BookingDetailId = approvalHistoryModel.BookingDetailId,
                        HeadDepartmentId = userId,
                        ApprovalByHeadDepartment = approvalHistoryModel.ApprovalByHeadDepartment, 
                        ReasonByHeadApproval = approvalHistoryModel.ReasonByHeadApproval,
                        //temp
                        ManagerId = userId,
                    };

                    _unitOfWork.ApprovalHistoryRepository.Insert(newApprovalHistory);
                    _unitOfWork.Save();
                }
                else
                {
                    existingApprovalHistory.ManagerId = userId; 
                    existingApprovalHistory.ApprovalByManager = approvalHistoryModel.ApprovalByManager;
                    existingApprovalHistory.ReasonByManager = approvalHistoryModel.ReasonByManager;
                    _unitOfWork.ApprovalHistoryRepository.Update(existingApprovalHistory);
                    _unitOfWork.Save();
                }
            }

    }
}
