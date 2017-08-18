<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="S3UploaderControl.ascx.cs" Inherits="Kadena.CMSFormControls.Kadena.S3Uploader" %>

<script>
    function initPhotoUpload() {
        _ifrUploader = document.getElementById('ifrUploader');
        _fldFileUrl = document.getElementById('fldFileUrl');

        var btnUpload = _ifrUploader.contentWindow.document.getElementById('btnUpload');
        var lnkFile = _ifrUploader.contentWindow.document.getElementById('lnkFile');
        var lblMessage = _ifrUploader.contentWindow.document.getElementById('lblMessage');
        var inpFile = _ifrUploader.contentWindow.document.getElementById('inpFile');
        
        if (lnkFile.textContent.length == 0
            && lblMessage.textContent.length == 0) {
            lnkFile.setAttribute('href', _fldFileUrl.value);
            lnkFile.textContent = _fldFileUrl.value;
        }
        else {
            _fldFileUrl.value = lnkFile.getAttribute('href');
        }

        inpFile.onchange = function (event) {
            _ifrUploader.contentWindow.document.getElementById('aspnetForm').submit();
        }

        btnUpload.onclick = function (event) {
            inpFile.click();
        }
    }
</script>

<asp:HiddenField ID="fldFileUrl" runat="server" ClientIDMode="Static"/>
<div id="divFrame">
    <iframe id="ifrUploader" onload="initPhotoUpload()" scrolling="no" frameborder="0" hidefocus="true" 
        style="text-align: center; vertical-align: middle; padding:0px; margin: 0px; border-style:none; width: 100%; height: 70px" 
        src="/CMSPages/Kadena/S3Uploader.aspx"></iframe>
</div>