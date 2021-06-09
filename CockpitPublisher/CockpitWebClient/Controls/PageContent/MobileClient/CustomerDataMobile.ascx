<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CustomerDataMobile.ascx.vb" Inherits="CockpitWebClient.CustomerDataMobile" %>

<%@ Register Src="~/Controls/TableData.ascx" TagPrefix="tableData" TagName="tableData" %>

<div class="MultipleTableWrapper">
    <tableData:tableData ID="tbl1" TableConfigurationID="tblCustomerData1" GridWidth="390" IsMobileBrowser="true" runat="server" />
    <tableData:tableData ID="tbl2" TableConfigurationID="tblCustomerData2" GridWidth="390" IsMobileBrowser="true" runat="server" />
    <tableData:tableData ID="tbl3" TableConfigurationID="tblCustomerData3" GridWidth="390" IsMobileBrowser="true" runat="server" />
    <tableData:tableData ID="tbl5" TableConfigurationID="tblCustomerData4" GridWidth="390" IsMobileBrowser="true" runat="server" />
</div>
