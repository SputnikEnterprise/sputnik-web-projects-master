<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SelectIdentity.ascx.vb" Inherits="CockpitWebClient.SelectIdentity" %>

<script type="text/javascript">
// <![CDATA[
    $(document).ready(function () {
        $("#selectIdentityBtn").button()
                               .click(function () {
                                   if ($("#chooseIdentityWrapper").data("opened") == true) {
                                       $("#chooseIdentityWrapper").slideUp();
                                       $("#chooseIdentityWrapper").data("opened", false);
                                   }
                                   else {
                                       $("#chooseIdentityWrapper").slideDown();
                                       $("#chooseIdentityWrapper").data("opened", true);
                                   }
                                   return false;
                               });
        $("#closeIdentityBtn").button()
                              .click(function () {
                                  $("#chooseIdentityWrapper").slideUp();
                                  $("#chooseIdentityWrapper").data("opened", false);
                                  return false;
                              });
    });
  // ]]>
</script>

<button id="selectIdentityBtn">Mandant wählen</button>

<div id="selectIdentityWrapper" class="ui-widget">

    <%--This wrapper is positioned absolute (look in css) so it can slide down over the tabs.--%>
    <div id="chooseIdentityWrapper" class="ui-widget-content ui-state-active">
        <table>
            <tr>
                <td>Mandanten</td>
                <td><asp:DropDownList ID="ddlMDs" runat="server" ClientIDMode="Static" /></td>
            </tr>
            <tr>
                <td>Berater</td>
                <td><asp:DropDownList ID="ddlKSTs" runat="server" ClientIDMode="Static" /></td>
            </tr>
        </table>

        <button id="closeIdentityBtn">Schliessen</button>
    </div>
</div>
