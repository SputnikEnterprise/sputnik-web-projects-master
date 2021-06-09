<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="General.ascx.vb" Inherits="CockpitWebClient.General" %>

<%@ Register Src="~/Controls/TableData.ascx" TagPrefix="tableData" TagName="tableData" %>

<div class="MultipleTableWrapper">
    <tableData:tableData ID="tbl2" TableConfigurationID="tblGeneralData2"  GridWidth="230" Float="left" runat="server" />
    <tableData:tableData ID="tbl3" TableConfigurationID="tblGeneralData3" GridWidth="230" Float="left" runat="server" />
    <tableData:tableData ID="tbl4" TableConfigurationID="tblGeneralData4" GridWidth="380" Float="right" runat="server" />
</div>
<div class="MultipleTableWrapper floatClearing">
    <tableData:tableData ID="tbl5" TableConfigurationID="tblGeneralData5" GridWidth="470" Float="left" runat="server" />
    <tableData:tableData ID="tbl6" TableConfigurationID="tblGeneralData6" GridWidth="380" Float="right" runat="server"/>
</div>
<%--clears floating--%>
<div class="floatClearing"> </div>