<%@ Page Title="" Language="C#" MasterPageFile="~/PMempleados.Master" AutoEventWireup="true" CodeBehind="Bienvenido.aspx.cs" Inherits="ProyectoFinal.Bienvenido" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .style2
    {
    }
    .style3
    {
        width: 437px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width:100%;">
    <tr>
        <td class="style3">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style3">
            &nbsp;</td>
        <td>
            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="XX-Large" 
                ForeColor="#CC0000" Text="BIENVENIDO"></asp:Label>
        </td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style2" colspan="3">
            &nbsp;</td>
    </tr>
</table>
</asp:Content>
