<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SalesFigures.ascx.vb"
    Inherits="CockpitWebClient.SalesFigures" %>

<%@ Register Src="~/Controls/TableData.ascx" TagName="tableData" TagPrefix="tableData" %>

<div class="MultipleTableWrapper">
    <tableData:tableData ID="tbl1" TableConfigurationID="tblSalesFigures1" GridWidth="420" Float="left" GridRowInsertedJavaScriptHandler="RowInsertedHandler_SalesFiguresTable1" runat="server" />
    <tableData:tableData ID="tbl2" TableConfigurationID="tblSalesFigures2" GridWidth="420" Float="right" GridRowInsertedJavaScriptHandler="RowInsertedHandler_SalesFiguresTable2" runat="server" />
</div>
<div class="MultipleTableWrapper floatClearing">
    <tableData:tableData ID="tbl3" TableConfigurationID="tblSalesFigures3" GridWidth="420" Float="left" GridRowInsertedJavaScriptHandler="RowInsertedHandler_SalesFiguresTable3" runat="server" />
    <tableData:tableData ID="tbl4" TableConfigurationID="tblSalesFigures4" GridWidth="420" Float="right" runat="server"/>
</div>
<div class="MultipleTableWrapper floatClearing">
    <tableData:tableData ID="tbl5" TableConfigurationID="tblSalesFigures5" GridWidth="420" Float="left" GridRowInsertedJavaScriptHandler="RowInsertedHandler_SalesFiguresTable5" runat="server" />
    <tableData:tableData ID="tbl6" TableConfigurationID="tblSalesFigures6" GridWidth="420" Float="right" runat="server" />
</div>
<%--clears floating--%>
<div class="floatClearing"> </div>