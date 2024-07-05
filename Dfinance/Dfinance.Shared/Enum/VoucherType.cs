using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Shared.Enum
{
    public enum VoucherType
    {
        // New Change. Here we can use primary voucherid for making it generalize
        // So all the vouchers under this primary voucher can use this page.
        Purchase = 17,
    Sales_Invoice=23,
    Sales_POS=89,
    //Opening_Stock = 38,
    Opening_Stock = 28, // Primary voucher id of Opening stock is 28.
    Purchase_Return=76,
    Sales_Return = 77, Delivery = 80, Service = 81, Payment_Voucher = 5, Receipt_Voucher = 7, Journal = 6, Contra = 4, Purchase_Order = 18, Sales_Enquiry = 84, Sales_Quotation = 85,
    Stock_Transfer = 83, Delivery_In = 13, Delivery_Out = 14, Stock_Adjustment = 46, Physical_Stock = 16, Stock_Return = 86, Stock_Issue = 90, Stock_Receipt = 91,
    Sales_Estimate = 93,Sales_Order = 94,Purchase_Enquiry = 95,Production = 92,Service_Invoice = 107, Service_Estimate = 104, Service_Order = 106, Service_Quotation = 105,
    Service_Enquiry = 103, Service_Delivery_Out=108, GRN_Quotation=113,JobCard=112,WarrentyClaim=114,GRN=111, Queue = 137, Budgeting = 70, RestaurantInvoice = 118, RestaurantKOT = 117,
    LaundryReceive=122, LaundryInvoice=121,Shipping_Gate_Pass=145,Shipping_Delivery_In=148, Shipping_Delivery_Out=149,Contract=150,Shipping_Order=151,
    Container_Delivery = 152, Container_Storage_IN = 153, Container_Storage_OUT = 154, Container_Return = 155, Material_Request=156, Material_Issue=157, Material_Receive=158,
    Provisional_Purchase=162,Stock_Request=134,Import_PurchaseOrder=135, Import_Purchase=136, ProjectInvoice=109, Purchase_Request = 143,Purchase_Quotation=161,
    Opening_Balance = 26 // openingvoucher primaryvoucherId

    }
}
