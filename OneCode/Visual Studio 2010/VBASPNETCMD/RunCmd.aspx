<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RunCmd.aspx.vb" Inherits="VBASPNETCMD.RunCmd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ASPNETCMD</title>
    <script type="text/javascript">
        //Before submit clear tbResult value
        function clearResult() {
            document.getElementById("tbResult").value = "";
        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset>
        <legend>Please Load Batch File: </legend>
        <asp:RegularExpressionValidator ForeColor="Red" ID="regExpValidatorForfileUpload"
            runat="server" ControlToValidate="fileUpload" ErrorMessage="please choose .bat file"
            ValidationGroup="UploadFile" ValidationExpression=".+\.bat$" Display="Dynamic"></asp:RegularExpressionValidator>
        <asp:RequiredFieldValidator Display="Dynamic" ForeColor="Red" ID="eequiredValidatorForfileUpload"
            ControlToValidate="fileUpload" runat="server" ValidationGroup="UploadFile" ErrorMessage="Please choose a file"></asp:RequiredFieldValidator>
        <div>
            <asp:FileUpload Width="320px" ID="fileUpload" runat="server" />
            &nbsp;
            <asp:Button ID="btnRunBatch" OnClientClick="clearResult();" ValidationGroup="UploadFile" runat="server" Text="UpLoad And Run"
                OnClick="btnRunBatch_Click" />
        </div>
    </fieldset>
    <br />
    <fieldset>
        <legend>Please type Command line: </legend>
        <br />
        <asp:TextBox ID="tbCommand" TextMode="MultiLine" runat="server" Width="300px" Height="100px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="requiredValidatorFortbCommand" ForeColor="Red" ControlToValidate="tbCommand"
            runat="server" ValidationGroup="CmdText" Display="Dynamic" ErrorMessage="Please input some command line"></asp:RequiredFieldValidator>
        <br />
        <asp:Button ID="btnRunCmd"  OnClientClick="clearResult();" runat="server" Text="RunCmd" OnClick="btnRunCmd_Click"
            ValidationGroup="CmdText" />
    </fieldset>
    <fieldset>
        <legend>Please input identify information </legend>
        <label>
            DomainName:</label>
        <asp:TextBox ID="tbDomainName" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredValidatorfortbDomainName" ForeColor="Red"
            ControlToValidate="tbDomainName" runat="server" ValidationGroup="UploadFile"
            Display="Dynamic" ErrorMessage="Please inputdomain name"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" ControlToValidate="tbDomainName"
            runat="server" ValidationGroup="CmdText" Display="Dynamic" ErrorMessage="Please inputdomain name"></asp:RequiredFieldValidator>
        <br />
        <label>
            UserName:</label>
        <asp:TextBox ID="tbUserName" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredValidatorfortbUserName" ForeColor="Red" ControlToValidate="tbUserName"
            runat="server" Display="Dynamic" ValidationGroup="UploadFile" ErrorMessage="Please inputuser name"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" ControlToValidate="tbUserName"
            runat="server" Display="Dynamic" ValidationGroup="CmdText" ErrorMessage="Please inputuser name"></asp:RequiredFieldValidator>
        <br />
        <label>
            Password:</label>
        <asp:TextBox ID="tbPassword" TextMode="Password" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredValidatorfortbPassword" ForeColor="Red" ControlToValidate="tbPassword"
            runat="server" Display="Dynamic" ValidationGroup="UploadFile" ErrorMessage="Please inputpassword"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" ControlToValidate="tbPassword"
            runat="server" Display="Dynamic" ValidationGroup="CmdText" ErrorMessage="Please inputpassword"></asp:RequiredFieldValidator>
    </fieldset>
    <div>
        <label>
            Outputs:
        </label>
        <br />
        <asp:TextBox ID="tbResult" TextMode="MultiLine" runat="server" Width="500px" Height="300px"></asp:TextBox>
    </div>
    </form>
</body>
</html>
