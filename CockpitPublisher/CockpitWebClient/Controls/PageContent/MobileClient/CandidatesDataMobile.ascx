<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CandidatesDataMobile.ascx.vb" Inherits="CockpitWebClient.CandidatesDataMobile" %>

<%@ Register Src="~/Controls/TableData.ascx" TagPrefix="tableData" TagName="tableData" %>

<div class="MultipleTableWrapper">
    <tableData:tableData ID="tbl1" TableConfigurationID="tblCandidateData1" GridWidth="390" IsMobileBrowser="true" runat="server" />
    <tableData:tableData ID="tbl2" TableConfigurationID="tblCandidateData2" GridWidth="390" IsMobileBrowser="true" GridRowInsertedJavaScriptHandler="CandidatesDataTable2Formatter" runat="server" />
    <tableData:tableData ID="tbl3" TableConfigurationID="tblCandidateData3" GridWidth="390" IsMobileBrowser="true" runat="server" />
    <tableData:tableData ID="tbl4" TableConfigurationID="tblCandidateData4" GridWidth="390" IsMobileBrowser="true" GridRowInsertedJavaScriptHandler="CandidatesDataTable4Formatter" runat="server"/>
    <tableData:tableData ID="tbl5" TableConfigurationID="tblCandidateData5" GridWidth="390" IsMobileBrowser="true" runat="server" />
    <tableData:tableData ID="tbl6" TableConfigurationID="tblCandidateData6" GridWidth="390" IsMobileBrowser="true" runat="server" />
</div>
