<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cache.aspx.cs" Inherits="SitefinityWebApp.Cache" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Cache Management Page</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="Cache invalidation management">
    <meta name="author" content="Peter Marinov">

    <link href="https://v4-alpha.getbootstrap.com/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />
        <div class="container-fluid m-4">
            <div class="alert alert-info" role="alert">
                <h4 class="alert-heading">Machine Name</h4>
                <p class="mb-0">
                    Current machine name is: 
                    <strong>
                        <asp:Literal ID="literalMachineKey" runat="server" />
                    </strong>
                </p>
            </div>

            <div class="row">
                <div class="col-12">
                    <h1 class="cover-heading">Cache Management</h1>
                    <p class="lead">Click the button below to process any cache invalidation messages in the database used by this node</p>
                    <p class="lead">
                        <asp:Button runat="server" ID="buttonMain" class="btn btn-lg btn-secondary" Text="Process System Messages" OnClick="buttonMain_Click"></asp:Button>
                    </p>
                </div>

                <div class="col-12">
                    <asp:UpdatePanel ID="updatePanelMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="divCallout" class="bd-callout" runat="server" visible="false">
                                <h4>
                                    <asp:Literal Text="text" id="literalHeading" runat="server" />
                                </h4>
                                <asp:Repeater runat="server" ID="repeaterItems">
                                    <HeaderTemplate>
                                        <ul class="list-group">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li class="list-group-item">
                                            <asp:Literal ID="literalItem" runat="server" Text='<%# Eval("SystemMessage") %>'></asp:Literal>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="buttonMain" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <style scoped="scoped">
            .bd-callout {
                padding: 1.25rem;
                margin-top: 1.25rem;
                margin-bottom: 1.25rem;
                border: 1px solid #eee;
                border-left-width: .25rem;
                border-radius: .25rem;
                border-left-color: #31708f;
                background-color: #f7f7f9;
            }

                .bd-callout h4 {
                    color: #31708f;
                }
        </style>
    </form>
    <script src="https://code.jquery.com/jquery-3.1.1.slim.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/tether/1.4.0/js/tether.min.js"></script>
    <script src="https://v4-alpha.getbootstrap.com/dist/js/bootstrap.min.js"></script>
</body>
</html>
