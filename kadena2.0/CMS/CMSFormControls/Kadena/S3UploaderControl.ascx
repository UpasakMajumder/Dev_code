<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="S3UploaderControl.ascx.cs" Inherits="Kadena.CMSFormControls.Kadena.S3Uploader" %>

<script>
    function initPhotoUpload() {
        _divFrame = document.getElementById('divFrame');
        _divUploadMessage = document.getElementById('divUploadMessage');
        _ifrUploader = document.getElementById('ifrUploader');
        _fldFileUrl = document.getElementById('fldFileUrl');

        var btnUpload = _ifrUploader.contentWindow.document.getElementById('btnUpload');
        var lnkFile = _ifrUploader.contentWindow.document.getElementById('lnkFile');

        if (lnkFile.textContent.length == 0) {
            lnkFile.setAttribute('href', _fldFileUrl.value);
            lnkFile.textContent = _fldFileUrl.value;
        }
        else {
            _fldFileUrl.value = lnkFile.getAttribute('href');
        }

        btnUpload.onclick = function (event) {
            var inpFile = _ifrUploader.contentWindow.document.getElementById('inpFile');
            
            inpFile.click();

            //Baisic validation for Photo
            _divUploadMessage.style.display = 'none';

            //if (inpFile.value.length == 0) {
            //    return;
            //}

            //var regExp = /^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.jpg|.JPG|.gif|.GIF|.png|.PNG|.bmp|.BMP)$/;

            //if (!regExp.test(inpFile.value)) //Somehow the expression does not work in Opera
            //{
            //    _divUploadMessage.innerHTML = '<span style=\"color:#ff0000\">Invalid file type. Only supports jpg, gif, png and bmp.</span>';
            //    _divUploadMessage.style.display = '';
            //    return;
            //}

            _ifrUploader.contentWindow.document.getElementById('aspnetForm').submit();
            //_divFrame.style.display = 'none';
        }
    }

    function photoUploadComplete(message, isError) {
        _divUploadMessage.style.display = 'none';
        _divFrame.style.display = '';

        if (message.length) {
            var color = (isError) ? '#ff0000' : '#008000';

            _divUploadMessage.innerHTML = '<span style=\"color:' + color + '\;font-weight:bold">' + message + '</span>';
            _divUploadMessage.style.display = '';
        }
    }
</script>

<asp:HiddenField ID="fldFileUrl" runat="server" ClientIDMode="Static"/>
<div id="divFrame">
    <iframe id="ifrUploader" onload="initPhotoUpload()" scrolling="no" frameborder="0" hidefocus="true" 
        style="text-align: center; vertical-align: middle; padding:0px; margin: 0px; border-style:none; width: 100%; height: 70px" src="/CMSFormControls/S3Uploader.aspx"></iframe>
</div>
<div id="divUploadMessage" style="display: none"></div>