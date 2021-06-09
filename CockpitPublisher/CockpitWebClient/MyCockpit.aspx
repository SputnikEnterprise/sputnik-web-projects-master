<%@ Page Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="MyCockpit.aspx.vb" Inherits="CockpitWebClient.MyCockpit" %>
<%@ Register Src="~/Controls/SelectIdentity.ascx" TagPrefix="general" TagName="SelectIdentity" %>

<%--Additional css content--%>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="AdditionalCssContent">
   <asp:Placeholder id="additionalCSSPlaceholder" runat="server" />
</asp:Content>

<%--Additional script contents--%>
<asp:Content ID="AdditionalScriptContent" runat="server" ContentPlaceHolderID="AdditionalScriptContent">
</asp:Content>

<%--Body contents--%>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="imgLoadingAnimation">
    </div>
    <div id="selectIdentity">
        <general:SelectIdentity runat="server" />
    </div>
    <%-- Placeholder for concrete layout. --%>
    <asp:PlaceHolder ID="layoutPalceholder" runat="server" />
    <%-- Dialog content can be inserted here with jquery and shown with jquery ui dialog --%>
    <div id="globalDialogWrapper">
    </div>
</asp:Content>
