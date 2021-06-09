<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="GeneralMobile.ascx.vb" Inherits="CockpitWebClient.GeneralMobile" %>

<%@ Register Src="~/Controls/TableData.ascx" TagPrefix="tableData" TagName="tableData" %>

<div class="MultipleTableWrapper">
    <tableData:tableData ID="tbl2" TableConfigurationID="tblGeneralData2" GridWidth="390" IsMobileBrowser="true" runat="server" />
    <tableData:tableData ID="tbl3" TableConfigurationID="tblGeneralData3" GridWidth="390" IsMobileBrowser="true" runat="server" />
    <tableData:tableData ID="tbl4" TableConfigurationID="tblGeneralData4" GridWidth="390" IsMobileBrowser="true" runat="server" />
    <tableData:tableData ID="tbl5" TableConfigurationID="tblGeneralData5" GridWidth="390" IsMobileBrowser="true" runat="server" />
    <tableData:tableData ID="tbl6" TableConfigurationID="tblGeneralData6" GridWidth="390" IsMobileBrowser="true" runat="server"/>
</div>

