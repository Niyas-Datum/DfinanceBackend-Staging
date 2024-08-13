using AutoMapper;
using Dfinance.Core;
using Dfinance.Core.Domain;
using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;

namespace Dfinance.Application.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<InventoryTransactionDto, FiTransaction>().ReverseMap();
            CreateMap<TblMaFinYear, FinanceYearDto>()
                .ForMember(tb=>tb.FinanceYear , f=>f.MapFrom(fi=>fi.FinYearCode));


            CreateMap<FinanceTransactionDto, JournalVoucherDto>().ReverseMap();

            CreateMap<FinanceTransactionDto,ContraDto>().ReverseMap();

            CreateMap<FinanceTransactionDto, OpeningVoucherDto>().ReverseMap();


            CreateMap<RestaurentDto, InventoryTransactionDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            CreateMap<InvRestTransItemDto, InvTransItemDto>();
            CreateMap<FinanceTransactionDto, JournalDto>().ReverseMap();
            CreateMap<ItemReservationDto, InventoryTransactionDto>();
            CreateMap<InvTransItemReservDto, InvTransItemDto>();
            //CreateMap<InventoryTransactionDto, ItemReservationDto>();
            //CreateMap<InvTransItemDto, InvTransItemReservDto>();









            //CreateMap<BranchDto, MaCompany>()
            //    .ForMember(b => b.ContactPersonId, ma => ma.MapFrom(mac => mac.ContactPerson.Id))
            //    .ForMember(b => b.Nature, ma => ma.MapFrom(mac => mac.BranchType.Value))
            //    .ForMember(b => b.Country, ma => ma.MapFrom(mac => mac.Country.Value))
            //    .ForMember(b => b.SalesTaxNo, ma => ma.MapFrom(mac => mac.VATNo))
            //    .ForMember(b => b.Company, ma => ma.MapFrom(mac => mac.CompanyName))
            //    .ForMember(b => b.ActiveFlag, opt => opt.MapFrom(src => src.Active));

            //CreateMap<SpMacompanyFillallbranch, MaCompany>()
            //    .ForMember(b => b.Id, ma => ma.MapFrom(mac => mac.Id))
            //    .ForMember(b => b.HocompanyName, ma => ma.MapFrom(mac => mac.Name));

            //CreateMap<test123, test123_Domain>().ForMember(t=>t.ContactPersonId,dt=>dt.MapFrom(e=>e.ContactPerson.Id))
            //    .ForMember(b => b.Nature, ma => ma.MapFrom(mac => mac.BranchType.Value))
            //    .ForMember(b => b.Country, ma => ma.MapFrom(mac => mac.Country.Value))
            //    .ForMember(b => b.SalesTaxNo, ma => ma.MapFrom(mac => mac.VATNo))
            //    .ForMember(b => b.Company, ma => ma.MapFrom(mac => mac.CompanyName))
            //    ;
        }
    }
}
