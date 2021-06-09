<%@ Control Language="VB" AutoEventWireup="false"
    Inherits="WOS_Website.CandidatesNav" Codebehind="CandidatesNav.ascx.vb" %>
<ul>
    <li>
        <iwc:HyperLink ID="hlnkArbeitsbescheinigung" TextKey="TEXT_MENU_EMPLOYERS_CERTIFICATE" runat="server" /></li>
    <li>
        <iwc:HyperLink ID="hlnkZwischenverdienstformulare" TextKey="TEXT_MENU_BETWEEN_EARNINGS_FORMS" runat="server" /></li>
    <li>
        <iwc:HyperLink ID="hlnkVakanzen" TextKey="TEXT_MENU_JOB_VACANCIES" runat="server" />
        <ul>
            <li><iwc:HyperLink ID="hlnkAlleVakanzen" TextKey="TEXT_MENU_JOB_VACANCIES_ALL" runat="server" Visible="false" /></li>
            <li><iwc:HyperLink ID="hlnkMeineVakanzen" TextKey="TEXT_MENU_JOB_VACANCIES_FOR_ME" runat="server" Visible="false" /></li>
        </ul>
    </li>
    <li>
			</li>
    <li>
        <iwc:HyperLink ID="hlnkLohnabrechnungen" TextKey="TEXT_MENU_PAYROLL" runat="server" /></li>
    <li>
        <iwc:HyperLink ID="hlnkLohnausweis" TextKey="TEXT_MENU_WAGE_STATEMENT" runat="server" /></li>
    <li>
			</li>
    <li>
			</li>
    <li>
        <iwc:HyperLink ID="hlnkEinsatzvertraege" TextKey="TEXT_MENU_OPERATING_CONTRACTS" runat="server" /></li>
    <li>
        <iwc:HyperLink ID="hlnkMARapporte" TextKey="TEXT_MENU_REPORTS" runat="server" /></li>
</ul>
