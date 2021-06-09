<%@ Control Language="VB" AutoEventWireup="false" Inherits="WOS_Website.KDNav" Codebehind="KDNav.ascx.vb" %>
<ul>
    <li>
        <iwc:HyperLink ID="hlnkVerleihvertraege" TextKey="TEXT_MENU_RENTAL_AGREEMENT" runat="server" />
        <ul>
           <li><iwc:HyperLink ID="hlnkOffeneVerleihvertraege" TextKey="TEXT_MENU_RENTAL_AGREEMENT_OPEN" runat="server" /></li>
        </ul>
    </li>
    <li>
        <iwc:HyperLink ID="hlnkRapporte" TextKey="TEXT_MENU_REPORTS" runat="server" />
    </li>
    <!-- <li>
        <iwc:HyperLink ID="hlnkJobKandidaten" TextKey="TEXT_MENU_AVAILABLE_CANDIDATES" runat="server" />
        <ul>
            <li><iwc:HyperLink ID="hlnkAlleJobKandidaten" TextKey="TEXT_MENU_AVAILABLE_CANDIDATES_ALL" runat="server" Visible="false"/></li>
            <li><iwc:HyperLink ID="hlnkMeineJobKandidaten" TextKey="TEXT_MENU_AVAILABLE_CANDIDATES_FOR_ME" runat="server" Visible="false"/></li>
        </ul>
    </li> -->
    <li>
        <iwc:HyperLink ID="hlnkRechnungen" TextKey="TEXT_MENU_BILLS" runat="server" />
    </li>
</ul>
