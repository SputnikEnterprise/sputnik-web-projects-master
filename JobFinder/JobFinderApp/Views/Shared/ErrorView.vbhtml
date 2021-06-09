@ModelType System.Web.Mvc.HandleErrorInfo

@Code
    ViewData("Title") = "ErrorView"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div data-role="page" id="error" style="padding: 20px">
    <br />
    <br />
    <h2 style="text-align: center">
        Error occurred!
    </h2>
    
    <h3>
    Some technical informations: 
    </h3>
    <p>
        @Model.Exception.Message
        <br />
        Controller: @Model.ControllerName
        <br />
        Action: @Model.ActionName
        <br />
        The Web Application Log on the server contains more informations for the administrator.
    </p>
</div>
