<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CustomerData.ascx.vb" Inherits="CockpitWebClient.CustomerData" %>

<%@ Register Src="~/Controls/TableData.ascx" TagPrefix="tableData" TagName="tableData" %>

<div class="MultipleTableWrapper">
    <tableData:tableData ID="tbl1" TableConfigurationID="tblCustomerData1" GridWidth="400" Float="left" runat="server" />
    <tableData:tableData ID="tbl2" TableConfigurationID="tblCustomerData2" GridWidth="440" Float="right" runat="server" />
</div>
<div class="MultipleTableWrapper floatClearing">
    <tableData:tableData ID="tbl3" TableConfigurationID="tblCustomerData3" GridWidth="400" Float="left" runat="server" />
</div>
<div class="MultipleTableWrapper floatClearing">
    <tableData:tableData ID="tbl5" TableConfigurationID="tblCustomerData4" GridWidth="400" Float="left" runat="server" />
</div>
<%--clears floating--%>
<div class="floatClearing"> </div>