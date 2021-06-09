<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SalesFiguresMobile.ascx.vb"
    Inherits="CockpitWebClient.SalesFiguresMobile" %>

<%@ Register Src="~/Controls/TableData.ascx" TagName="tableData" TagPrefix="tableData" %>

<div class="MultipleTableWrapper">
    <tableData:tableData ID="tbl1" TableConfigurationID="tblSalesFigures1" GridWidth="390" IsMobileBrowser="true" GridRowInsertedJavaScriptHandler="RowInsertedHandler_SalesFiguresTable1" runat="server" />
    <tableData:tableData ID="tbl2" TableConfigurationID="tblSalesFigures2" GridWidth="390" IsMobileBrowser="true" GridRowInsertedJavaScriptHandler="RowInsertedHandler_SalesFiguresTable2" runat="server" />
    <tableData:tableData ID="tbl3" TableConfigurationID="tblSalesFigures3" GridWidth="390" IsMobileBrowser="true" GridRowInsertedJavaScriptHandler="RowInsertedHandler_SalesFiguresTable3" runat="server" />
    <tableData:tableData ID="tbl4" TableConfigurationID="tblSalesFigures4" GridWidth="390" IsMobileBrowser="true" runat="server"/>
    <tableData:tableData ID="tbl5" TableConfigurationID="tblSalesFigures5" GridWidth="390" IsMobileBrowser="true" GridRowInsertedJavaScriptHandler="RowInsertedHandler_SalesFiguresTable5" runat="server" />
    <tableData:tableData ID="tbl6" TableConfigurationID="tblSalesFigures6" GridWidth="390" IsMobileBrowser="true" runat="server" />
</div>
