using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace Dfinance.Application.Services.Inventory
{
    public class TaxCategoryService : ITaxCategoryService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;

        public TaxCategoryService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        /// <summary>
        /// Saves or updates a TaxCategory.
        /// </summary>
        /// <param name="taxCategoryDto">The DTO containing TaxCategory data.</param>
        /// <returns>A response indicating the outcome.</returns>
        public CommonResponse SaveTaxCategory(TaxCategoryDto taxCategoryDto)
        {
            try
            {
                if (taxCategoryDto.Id == 0)
                {
                    var response = SaveMaTaxDetails(taxCategoryDto);
                    return response;
                }
                else
                {
                    return UpdateMaTaxDetails(taxCategoryDto);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error($"An error occurred while saving the TaxCategory: {ex.Message}");
            }
        }

        /// <summary>
        /// Inserts or updates TaxCategory details.
        /// </summary>
        /// <param name="taxCategoryDto">The DTO containing TaxCategory details.</param>
        /// <returns>A response indicating the outcome.</returns>
        private CommonResponse SaveMaTaxDetails(TaxCategoryDto taxCategoryDto)
        {
            try
            {
                string criteria = "InsertMaTaxDetails";

                foreach (var taxType in taxCategoryDto.TaxTypeIds)
                {
                    SqlParameter newIdParameter = new SqlParameter("@NewID", SqlDbType.BigInt)
                    {
                        Direction = ParameterDirection.Output
                    };

                    _context.Database.ExecuteSqlRaw(
                        "EXEC MaTaxSP @Criteria={0}, @CategoryID={1}, @TaxTypeID={2}, " +
                        "@SalesPerc={3}, @PurchasePerc={4}, @NewID={5} OUTPUT",
                        criteria,
                        taxCategoryDto.Id,
                        taxType.Id,
                        taxCategoryDto.SalesPercentages[taxCategoryDto.TaxTypeIds.IndexOf(taxType)],
                        taxCategoryDto.PurchasePercentages[taxCategoryDto.TaxTypeIds.IndexOf(taxType)],
                        newIdParameter);
                }

                return CommonResponse.Created($"TaxCategory details for '{taxCategoryDto.CategoryName}' saved successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error($"An error occurred while saving TaxCategory details: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates TaxCategory details.
        /// </summary>
        /// <param name="taxCategoryDto">The DTO containing TaxCategory details.</param>
        /// <returns>A response indicating the outcome.</returns>
        private CommonResponse UpdateMaTaxDetails(TaxCategoryDto taxCategoryDto)
        {
            try
            {
                string criteria = "UpdateMaTaxDetails";

                foreach (var taxType in taxCategoryDto.TaxTypeIds)
                {
                    _context.Database.ExecuteSqlRaw(
                        "EXEC MaTaxSP @Criteria={0}, @CategoryID={1}, @TaxTypeID={2}, " +
                        "@SalesPerc={3}, @PurchasePerc={4}, @ID={5}",
                        criteria,
                        taxCategoryDto.Id,
                        taxType.Id,
                        taxCategoryDto.SalesPercentages[taxCategoryDto.TaxTypeIds.IndexOf(taxType)],
                        taxCategoryDto.PurchasePercentages[taxCategoryDto.TaxTypeIds.IndexOf(taxType)],
                        taxCategoryDto.Id);
                }

                return CommonResponse.Ok($"TaxCategory details for '{taxCategoryDto.CategoryName}' updated successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error($"An error occurred while updating TaxCategory details: {ex.Message}");
            }
        }
        /// <summary>
        /// DeleteTaxCat
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CommonResponse DeleteTaxCat(int Id)
        {
            try
            {
                string msg = null;
                var name = _context.MaTaxDetail.Where(i => i.Id == Id).
                   Select(i => i.CategoryId).
                   SingleOrDefault();
                if (name == null)
                {
                    msg = "This Category is Not Found";
                }
                else
                {
                    string Criteria = "DeleteMaTaxDetails";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC MaTaxSP @Criteria='{Criteria}',@ID='{Id}'");
                    msg = name + " Deleted Successfully";
                }
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        ///// <summary>
        ///// Inserts a new TaxCategory.
        ///// </summary>
        ///// <param name="name">The name of the TaxCategory.</param>
        ///// <param name="description">The description associated with the TaxCategory.</param>
        ///// <param name="active">Indicates whether the TaxCategory is active.</param>
        ///// <returns>A response indicating the outcome.</returns>
        //private CommonResponse SaveMaTax(string name, string description, bool active)
        //{
        //    try
        //    {
        //        int createdBy = _authService.GetId().Value;
        //        DateTime createdOn = DateTime.Now;
        //        int createdBranchId = _authService.GetBranchId().Value;
        //        string criteria = "InsertMaTax";

        //        SqlParameter newIdParameter = new SqlParameter("@NewID", SqlDbType.BigInt)
        //        {
        //            Direction = ParameterDirection.Output
        //        };

        //        _context.Database.ExecuteSqlRaw(
        //            "EXEC MaTaxSP @Criteria={0}, @Name={1}, @Note={2}, @Active={3}, " +
        //            "@CreatedOn={4}, @CreatedBy={5}, @CreatedBranchID={6}, @NewID={7} OUTPUT",
        //            criteria,
        //            name,
        //            description,
        //            active,
        //            createdOn,
        //            createdBy,
        //            createdBranchId,
        //            newIdParameter);

        //        return CommonResponse.Created($"TaxCategory '{name}' created successfully with ID {newIdParameter.Value}");
        //    }
        //    catch (Exception ex)
        //    {
        //        return CommonResponse.Error($"An error occurred while saving the TaxCategory: {ex.Message}");
        //    }
        //}

        ///// <summary>
        ///// Updates an existing TaxCategory.
        ///// </summary>
        ///// <param name="taxCategoryDto">The DTO containing TaxCategory data.</param>
        ///// <returns>A response indicating the outcome.</returns>
        //private CommonResponse UpdateMaTax(string name, string description, bool active ,int Id)
        //{
        //    try
        //    {
        //        int createdBy = _authService.GetId().Value;
        //        DateTime createdOn = DateTime.Now;
        //        int createdBranchId = _authService.GetBranchId().Value;
        //        string criteria = "UpdateMaTax";

        //        _context.Database.ExecuteSqlRaw(
        //              "EXEC MaTaxSP @Criteria={0}, @Name={1}, @Note={2}, @Active={3}, " +
        //              "@CreatedOn={4}, @CreatedBy={5}, @CreatedBranchID={6}, @ID={7}",
        //              criteria,
        //              name,
        //        description,
        //              active,
        //              createdOn,
        //              createdBy,
        //              createdBranchId,
        //              Id);

        //        return CommonResponse.Ok($"TaxCategory '{name}' updated successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return CommonResponse.Error($"An error occurred while updating the TaxCategory: {ex.Message}");
        //    }
        //}

    }
}
