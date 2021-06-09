<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AccordionLayout.ascx.vb" Inherits="CockpitWebClient.AccordionLayout" %>

<script type="text/javascript">
    // <![CDATA[
    $(document).ready(function () {
        // Init the accordion.
        $('#accordion').accordion({ autoHeight: false });
    });
    $("div#main").show();
    // ]]>
</script>

 <div id="accordion">
    <h2><a href="#">Allgemein</a></h2>
    <div id="generalTab"><asp:PlaceHolder ID="generalPalceholder" runat="server"/></div>
     <h2><a href="#">Einsatz-Detail</a></h2>
    <div id="adoptionTab"><asp:PlaceHolder ID="assignmentOfPersonnelPlaceholder" runat="server"/></div>
     <h2><a href="#">Umsätze</a></h2>
    <div id="salesFiguresTab"><asp:PlaceHolder ID="salesFiguresPlaceholder" runat="server"/></div>
     <h2><a href="#">Kandidatendaten</a></h2>
    <div id="candidatesDataTab"><asp:PlaceHolder ID="candidatesDataPlaceholder" runat="server"/></div>
     <h2><a href="#">Kundendaten</a></h2>
    <div id="customerDataTab"><asp:PlaceHolder ID="customerDataPlaceholder" runat="server"/></div>
 </div>