<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AssignmentOfPersonnel.ascx.vb" Inherits="CockpitWebClient.AssignmentOfPersonnel" %>

<%@ Register Src="~/Controls/TableData.ascx" TagPrefix="tableData" TagName="tableData" %>

<div class="MultipleTableWrapper">
    <tableData:tableData ID="tbl1" TableConfigurationID="tblAssignmentOfPers1" GridWidth="420" float="left"  runat="server"  />
    <tableData:tableData ID="tbl2" TableConfigurationID="tblAssignmentOfPers2" GridWidth="420" float="right"  runat="server"/>
</div>
<div class="MultipleTableWrapper floatClearing">
    <tableData:tableData ID="tbl3" TableConfigurationID="tblAssignmentOfPers3" GridWidth="420" float="left" GridRowInsertedJavaScriptHandler="RowInsertedHandler_AssignmentOfPersonnelTable3" runat="server"/>
    <tableData:tableData ID="tbl4" TableConfigurationID="tblAssignmentOfPers4" GridWidth="420" float="right" runat="server"/>
</div>
<div  class="MultipleTableWrapper floatClearing"">
    <tableData:tableData ID="tbl5" TableConfigurationID="tblAssignmentOfPers5" GridWidth="420" float="left" GridRowInsertedJavaScriptHandler="RowInsertedHandler_AssignmentOfPersonnelTable5" runat="server" />
    <tableData:tableData ID="tbl6" TableConfigurationID="tblAssignmentOfPers6"  GridWidth="420" float="right" runat="server"/>
</div>
<%--clears floating--%>
<div class="floatClearing"> </div>