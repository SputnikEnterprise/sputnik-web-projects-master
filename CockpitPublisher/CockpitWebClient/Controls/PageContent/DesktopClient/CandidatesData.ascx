<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CandidatesData.ascx.vb" Inherits="CockpitWebClient.CandidatesData" %>

<%@ Register Src="~/Controls/TableData.ascx" TagPrefix="tableData" TagName="tableData" %>

<div class="MultipleTableWrapper">
    <tableData:tableData ID="tbl1" TableConfigurationID="tblCandidateData1" GridWidth="420" Float="left" runat="server" />
    <tableData:tableData ID="tbl2" TableConfigurationID="tblCandidateData2" GridWidth="420" Float="right" GridRowInsertedJavaScriptHandler="CandidatesDataTable2Formatter" runat="server" />
</div>
<div class="MultipleTableWrapper floatClearing">
    <tableData:tableData ID="tbl3" TableConfigurationID="tblCandidateData3" GridWidth="420" Float="left" runat="server" />
    <tableData:tableData ID="tbl4" TableConfigurationID="tblCandidateData4" GridWidth="420" Float="right" GridRowInsertedJavaScriptHandler="CandidatesDataTable4Formatter" runat="server"/>
</div>
<div class="MultipleTableWrapper floatClearing">
    <tableData:tableData ID="tbl5" TableConfigurationID="tblCandidateData5" GridWidth="420" Float="left" runat="server" />
    <tableData:tableData ID="tbl6" TableConfigurationID="tblCandidateData6" GridWidth="420" Float="right" runat="server" />
</div>
<%--clears floating--%>
<div class="floatClearing"> </div>