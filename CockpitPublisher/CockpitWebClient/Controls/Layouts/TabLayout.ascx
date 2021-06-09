<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TabLayout.ascx.vb" Inherits="CockpitWebClient.TabLayout" %>

<script type="text/javascript">
    // <![CDATA[
    $(document).ready(function () {
        // Init the tabs.
        $("#tabs").tabs();
    });
    // ]]>
</script>

 <div id="tabs">
    <ul>
        <li><a href="#generalTab">Allgemein</a></li>
        <li><a href="#adoptionTab">Einsatz-Detail</a></li>
        <li><a href="#salesFiguresTab">Umsätze</a></li>
        <li><a href="#candidatesDataTab">Kandidatendaten</a></li>
        <li><a href="#customerDataTab">Kundendaten</a></li>
    </ul>
    <div id="generalTab" class="DataTab"><asp:PlaceHolder ID="generalTabPalceholder" runat="server"/></div>
    <div id="adoptionTab" class="DataTab"><asp:PlaceHolder ID="assignmentOfPersonnelTabPlaceholder" runat="server"/></div>
    <div id="salesFiguresTab" class="DataTab"><asp:PlaceHolder ID="salesFiguresTabPlaceholder" runat="server"/></div>
    <div id="candidatesDataTab" class="DataTab"><asp:PlaceHolder ID="candidatesDataTabPlaceholder" runat="server"/></div>
    <div id="customerDataTab" class="DataTab"><asp:PlaceHolder ID="customerDataTabPlaceholder" runat="server"/></div>
 </div>