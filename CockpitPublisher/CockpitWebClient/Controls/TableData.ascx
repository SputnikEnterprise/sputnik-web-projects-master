<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TableData.ascx.vb" Inherits="CockpitWebClient.TableData" %>

<script type="text/javascript">
// <![CDATA[
    $(document).ready(function () {
        SetupTableDataControl($("#<%=wrapperDiv.ClientID %>"),
                                '<%=TableConfigurationID %>',
                                '<%= ResolveClientUrl("~/WebServices/CockpitDataService.asmx/GetTableData") %>',
                                '<%=GridRowInsertedJavaScriptHandler %>',
                                '<%=IsMobileBrowser %>',
                                '<%=GridWidth %>', 
                                 175);
    });
 // ]]>
</script>

<div id="wrapperDiv" class="ui-widget tableDataWrapper" runat="server" >
    
   <%-- Header--%>
    <div class="ui-widget-header ui-corner-tl ui-corner-tr tableDataHeader">
        <asp:HyperLink ID="tableCaption" NavigateUrl="#" CssClass="tableCaption" runat="server" />
        <div class="imgMagnifier imgMangifierStateNormal"></div>
    </div>

    <%-- Content --%>
    <asp:panel id="tableData" runat="server" CssClass="ui-widget-content tableData">
        <%-- Content is filled in dynamically with the use of jQuery and jqGrid --%>
    </asp:panel>
</div>