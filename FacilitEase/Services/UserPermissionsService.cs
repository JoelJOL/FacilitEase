using FacilitEase.Contracts.ServiceContracts;
using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using System.Linq.Dynamic.Core;

namespace FacilitEase.Services
{
    public class UserPermissionsService : IUserPermissionsService
    {
        private readonly AppDbContext _context;

        public UserPermissionsService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// To get the permissions for each role
        /// </summary>
        /// <param name="userId">Id for user</param>
        /// <returns>All the details of permissions</returns>
        public IEnumerable<UserPermissionData> GetUserPermissions(int userId)
        {

            //Select the employees that have similar names to the text
            //Here ProfileData is an ApiModel to stroe the required data
            var userPermissions = _context.TBL_USER_PERMISSION
                                    .Where(userPermission => userPermission.UserId == userId)
                                    .Join(_context.TBL_PERMISSION,
                                        userPermission => userPermission.PermissionId,
                                        permissions => permissions.Id,
                                        (userPermission, permissions) => new { userPermission, permissions })
                                    .Join(_context.TBL_USER,
                                        userPermissionData => userPermissionData.userPermission.UserId,
                                        user => user.Id,
                                        (userPermissionData, user) => new UserPermissionData
                                        {
                                            UserPermissionId = userPermissionData.userPermission.Id,
                                            UserId = userPermissionData.userPermission.UserId,
                                            PermissionId = userPermissionData.userPermission.PermissionId,
                                            PermissionName = userPermissionData.permissions.PermissionName,
                                            IsActive = userPermissionData.userPermission.IsActive,
                                            EmployeeId = user.EmployeeId 
                                        })
    .ToList();
            return userPermissions;
        }

        public bool UpdatePermission(int userPermissionId, bool isActive)
        {
            var permission = _context.TBL_USER_PERMISSION.FirstOrDefault(t => t.Id == userPermissionId);

            permission.IsActive = isActive;
            _context.SaveChanges();
            return true;
        }
    }
}