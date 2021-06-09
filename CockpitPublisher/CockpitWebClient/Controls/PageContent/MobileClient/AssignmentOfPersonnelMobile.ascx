<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AssignmentOfPersonnelMobile.ascx.vb" Inherits="CockpitWebClient.AssignmentOfPersonnelMobile" %>

<%@ Register Src="~/Controls/TableData.ascx" TagPrefix="tableData" TagName="tableData" %>

<div class="MultipleTableWrapper">
    <tableData:tableData ID="tbl1" TableConfigurationID="tblAssignmentOfPers1" GridWidth="390" IsMobileBrowser="true" runat="server"  />
    <tableData:tableData ID="tbl2" TableConfigurationID="tblAssignmentOfPers2" GridWidth="390" IsMobileBrowser="true" runat="server"/>
    <tableData:tableData ID="tbl3" TableConfigurationID="tblAssignmentOfPers3" GridWidth="390" IsMobileBrowser="true" GridRowInsertedJavaScriptHandler="RowInsertedHandler_AssignmentOfPersonnelTable3" runat="server"/>
    <tableData:tableData ID="tbl4" TableConfigurationID="tblAssignmentOfPers4" GridWidth="390" IsMobileBrowser="true" runat="server"/>
    <tableData:tableData ID="tbl5" TableConfigurationID="tblAssignmentOfPers5" GridWidth="390" IsMobileBrowser="true" GridRowInsertedJavaScriptHandler="RowInsertedHandler_AssignmentOfPersonnelTable3" runat="server"/>
    <tableData:tableData ID="tbl6" TableConfigurationID="tblAssignmentOfPers6" GridWidth="390" IsMobileBrowser="true" runat="server"/>
</div>
