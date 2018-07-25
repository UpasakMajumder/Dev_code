using AutoMapper;
using Kadena.Dto.Common;
using Kadena.Dto.ViewOrder.Responses;
using Kadena.Models.OrderHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Mapping
{
    public class OrderHistoryProfile : Profile
    {
        private const string CellTypeText = "text";
        private const string CellTypeDate = "date";
        private const string CellTypeLink = "link";

        public OrderHistoryProfile()
        {
            CreateMap<OrderHistory, OrderHistoryDto>();
            CreateMap<OrderChange, TableCellDto[]>()
                .ConvertUsing(oc => new[]
                {
                    new TableCellDto { Text = oc.Category, Type = CellTypeText },
                    new TableCellDto { Text = oc.OldValue, Type = CellTypeText },
                    new TableCellDto { Text = oc.NewValue, Type = CellTypeText },
                    new TableCellDto { Text = oc.Date.ToString(), Type = CellTypeDate },
                    new TableCellDto { Text = oc.User, Type = CellTypeText },
                });
            CreateMap<OrderChanges, OrderHistoryChangesDto>()
                .ForMember(dest => dest.Headers, opt => opt.MapFrom(src => new[]
                {
                    src.ColumnHeaderCategory,
                    src.ColumnHeaderOldValue,
                    src.ColumnHeaderNewValue,
                    src.ColumnHeaderDate,
                    src.ColumnHeaderUser
                }));
            CreateMap<ItemChange, TableCellDto[]>()
                .ConvertUsing(ic => new[]
                {
                    new TableCellDto { Text = ic.ItemDescription, Type = CellTypeText },
                    new TableCellDto { Text = ic.ChangeType, Type = CellTypeText },
                    new TableCellDto { Text = ic.OldValue, Type = CellTypeText },
                    new TableCellDto { Text = ic.NewValue, Type = CellTypeText },
                    new TableCellDto { Text = ic.Date.ToString(), Type = CellTypeDate },
                    new TableCellDto { Text = ic.User, Type = CellTypeText },
                });
            CreateMap<ItemChanges, OrderHistoryChangesDto>()
                .ForMember(dest => dest.Headers, opt => opt.MapFrom(src => new[] 
                {
                    src.ColumnHeaderItemDescription,
                    src.ColumnHeaderChangeType,
                    src.ColumnHeaderOldValue,
                    src.ColumnHeaderNewValue,
                    src.ColumnHeaderDate,
                    src.ColumnHeaderUser
                }));
        }
    }
}
