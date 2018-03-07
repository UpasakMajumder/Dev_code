// GENERAL FUNCTIONS

function IsEmailValid(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function getParameterByName(name) {
    var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
};

if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}

function getDate(datePicker) {
    try {
        datePicker = $(datePicker);

        var format = datePicker.datepicker("option", "dateFormat"),
            text = datePicker.val(),
            settings = datePicker.datepicker("option", "settings");

        return $.datepicker.parseDate(format, text, settings);
    }
    catch (err) {
        return null;
    }
}

// LOGOUT

(function ($) {
    $.fn.logout = function (options) {
        var base = this;

        $(base).click(function (e) {
            e.preventDefault();

            $.ajax({
                type: "POST",
                url: '/KadenaWebService.asmx/' + base.attr("data-function"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    window.location.href = "/";
                },
                error: function (xhr, ajaxOptions, thrownError) { }
            });
        });
    }
}(jQuery));

// INITIAL PASSWORD SETTING

(function ($) {
    $.fn.createPassword = function (options) {
        var base = this;

        var settings = $.extend({
            passwordInput: '.j-password',
            confirmPasswordInput: '.j-confirm-password',
            passwordInputs: '.j-passwords',
            submitButton: '.j-submit',
            passwordErrorLabel: '.j-password-error-label',
            confirmPasswordErrorLabel: '.j-confirm-password-error-label'
        }, options);

        base.find(settings.passwordInputs).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.submitButton).click(function (e) {
            e.preventDefault();

            base.find(settings.passwordErrorLabel).hide();
            base.find(settings.passwordInput).removeClass("input--error");
            base.find(settings.confirmPasswordErrorLabel).hide();
            base.find(settings.confirmPasswordInput).removeClass("input--error");

            var userGUID = getParameterByName("h");

            if (base.find(settings.passwordInput).val() == '') {
                base.find(settings.passwordInput).addClass("input--error");
                base.find(settings.passwordErrorLabel).html(config.localization.initialPasswordSetting.passwordEmptyValidationMessage);
                base.find(settings.passwordErrorLabel).show();
                return;
            }
            if (base.find(settings.passwordInput).val().indexOf(' ') != -1) {
                base.find(settings.passwordInput).addClass("input--error");
                base.find(settings.passwordErrorLabel).html(config.localization.initialPasswordSetting.passwordContainsWhitespaceValidationMessage);
                base.find(settings.passwordErrorLabel).show();
                return;
            }
            if (base.find(settings.confirmPasswordInput).val() == '') {
                base.find(settings.confirmPasswordInput).addClass("input--error");
                base.find(settings.confirmPasswordErrorLabel).html(config.localization.initialPasswordSetting.passwordEmptyValidationMessage);
                base.find(settings.confirmPasswordErrorLabel).show();
                return;
            }
            if (base.find(settings.confirmPasswordInput).val().indexOf(' ') != -1) {
                base.find(settings.confirmPasswordInput).addClass("input--error");
                base.find(settings.confirmPasswordErrorLabel).html(config.localization.initialPasswordSetting.passwordContainsWhitespaceValidationMessage);
                base.find(settings.confirmPasswordErrorLabel).show();
                return;
            }
            if (base.find(settings.passwordInput).val() != base.find(settings.confirmPasswordInput).val()) {
                base.find(settings.passwordInput).addClass("input--error");
                base.find(settings.passwordErrorLabel).html(config.localization.initialPasswordSetting.passwordsAreNotTheSameValidationMessage);
                base.find(settings.passwordErrorLabel).show();
                return;
            }
            if (userGUID == null) {
                base.find(settings.passwordInput).addClass("input--error");
                base.find(settings.passwordErrorLabel).html(config.localization.initialPasswordSetting.passswordCantBeSetUpValidationMessage);
                base.find(settings.passwordErrorLabel).show();
            }

            var passedData = {
                password: base.find(settings.passwordInput).val(),
                confirmPassword: base.find(settings.confirmPasswordInput).val(),
                userGUID: userGUID
            };

            base.find(settings.submitButton).attr("disabled", "disabled");

            $.ajax({
                type: "POST",
                url: '/KadenaWebService.asmx/' + base.attr("data-function"),
                data: JSON.stringify(passedData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d.success) {
                        window.location.href = '/';
                    } else {
                        base.find(settings.passwordInput).addClass("input--error");
                        base.find(settings.passwordErrorLabel).html(data.d.errorMessage);
                        base.find(settings.passwordErrorLabel).show();
                    }
                    base.find(settings.submitButton).removeAttr("disabled");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    base.find(settings.passwordInput).addClass("input--error");
                    base.find(settings.passwordErrorLabel).html(config.localization.initialPasswordSetting.passswordCantBeSetUpValidationMessage);
                    base.find(settings.passwordErrorLabel).show();

                    base.find(settings.submitButton).removeAttr("disabled");
                }
            });
        });
    }
}(jQuery));

// CONTACT PERSON INFORMATION SET

(function ($) {
    $.fn.setPersonContactInformation = function (options) {
        var base = this;

        var settings = $.extend({
            firstNameInput: ".j-first-name",
            lastNameInput: ".j-last-name",
            mobileInput: ".j-mobile",
            phoneInput: ".j-phone",
            submitButton: ".j-submit-button",
            errorMassage: ".j-error-message"
        }, options);

        base.find(settings.firstNameInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.lastNameInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.mobileInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.phoneInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.submitButton).click(function (e) {
            e.preventDefault();

            base.find(settings.errorMassage).hide();

            var userGUID = base.attr("data-id");

            var passedData = {
                userGUID: userGUID,
                firstName: base.find(settings.firstNameInput).val(),
                lastName: base.find(settings.lastNameInput).val(),
                mobile: base.find(settings.mobileInput).val(),
                phoneNumber: base.find(settings.phoneInput).val()
            };

            base.find(settings.submitButton).attr("disabled", "disabled");

            $.ajax({
                type: "POST",
                url: '/KadenaWebService.asmx/' + base.attr("data-function"),
                data: JSON.stringify(passedData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d.success) {
                        toastr.success(config.localization.ContactPersonDetailsChange.SuccessTitle, config.localization.ContactPersonDetailsChange.Success);
                    } else {
                        toastr.error(config.localization.ContactPersonDetailsChange.ErrorTitle, data.d.errorMessage);
                    }
                    base.find(settings.submitButton).removeAttr("disabled");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    toastr.error(config.localization.ContactPersonDetailsChange.ErrorTitle, config.localization.ContactPersonDetailsChange.Error);

                    base.find(settings.submitButton).removeAttr("disabled");
                }
            });
        });
    }
}(jQuery));

// PASSWORD CHANGE

(function ($) {
    $.fn.changePassword = function (options) {
        var base = this;

        var settings = $.extend({
            oldPasswordInput: ".j-old-password",
            newPasswordInput: ".j-new-password",
            confirmPasswordInput: ".j-confirm-password",

            oldPasswordEmptyErrorLabel: ".j-old-password-empty-error-label",
            oldPasswordWhitespaceErrorLabel: ".j-old-password-whitespace-error-label",
            newPasswordEmptyErrorLabel: ".j-new-password-empty-error-label",
            newPasswordWhitespaceErrorLabel: ".j-new-password-whitespace-error-label",
            confirmPasswordEmptyErrorLabel: ".j-confirm-password-empty-error-label",
            confirmPasswordWhitespaceErrorLabel: ".j-confirm-password-whitespace-error-label",
            passwordsDontMatchErrorLabel: ".j-passwords-dont-match-error-label",
            generalErrorLabel: ".j-general-error-label",

            submitButton: ".j-submit-button"
        }, options);

        base.find(settings.oldPasswordInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.newPasswordInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.confirmPasswordInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.submitButton).click(function (e) {
            e.preventDefault();

            base.find(settings.oldPasswordEmptyErrorLabel).hide();
            base.find(settings.oldPasswordWhitespaceErrorLabel).hide();
            base.find(settings.oldPasswordInput).removeClass("input--error");
            base.find(settings.newPasswordEmptyErrorLabel).hide();
            base.find(settings.newPasswordWhitespaceErrorLabel).hide();
            base.find(settings.newPasswordInput).removeClass("input--error");
            base.find(settings.confirmPasswordEmptyErrorLabel).hide();
            base.find(settings.confirmPasswordWhitespaceErrorLabel).hide();
            base.find(settings.confirmPasswordInput).removeClass("input--error");
            base.find(settings.passwordsDontMatchErrorLabel).hide();
            base.find(settings.generalErrorLabel).hide();

            if (base.find(settings.oldPasswordInput).val().length == 0) {
                base.find(settings.oldPasswordInput).addClass("input--error");
                base.find(settings.oldPasswordEmptyErrorLabel).show();
                return;
            }
            if (base.find(settings.oldPasswordInput).val().indexOf(' ') != -1) {
                base.find(settings.oldPasswordInput).addClass("input--error");
                base.find(settings.oldPasswordWhitespaceErrorLabel).show();
                return;
            }
            if (base.find(settings.newPasswordInput).val().length == 0) {
                base.find(settings.newPasswordInput).addClass("input--error");
                base.find(settings.newPasswordEmptyErrorLabel).show();
                return;
            }
            if (base.find(settings.newPasswordInput).val().indexOf(' ') != -1) {
                base.find(settings.newPasswordInput).addClass("input--error");
                base.find(settings.newPasswordWhitespaceErrorLabel).show();
                return;
            }
            if (base.find(settings.confirmPasswordInput).val().length == 0) {
                base.find(settings.confirmPasswordInput).addClass("input--error");
                base.find(settings.confirmPasswordEmptyErrorLabel).show();
                return;
            }
            if (base.find(settings.confirmPasswordInput).val().indexOf(' ') != -1) {
                base.find(settings.confirmPasswordInput).addClass("input--error");
                base.find(settings.confirmPasswordWhitespaceErrorLabel).show();
                return;
            }
            if (base.find(settings.newPasswordInput).val() !== base.find(settings.confirmPasswordInput).val()) {
                base.find(settings.confirmPasswordInput).addClass("input--error");
                base.find(settings.passwordsDontMatchErrorLabel).show();
                return;
            }
            var userGUID = base.attr("data-id");

            var passedData = {
                userGUID: userGUID,
                oldPassword: base.find(settings.oldPasswordInput).val(),
                newPassword: base.find(settings.newPasswordInput).val(),
                confirmPassword: base.find(settings.confirmPasswordInput).val()
            };

            base.find(settings.submitButton).attr("disabled", "disabled");

            $.ajax({
                type: "POST",
                url: '/KadenaWebService.asmx/' + base.attr("data-function"),
                data: JSON.stringify(passedData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d.success) {
                        toastr.success(config.localization.PasswordChange.SuccessTitle, config.localization.PasswordChange.Success);
                    } else {
                        base.find(settings.generalErrorLabel).html(data.d.errorMessage);
                        base.find(settings.generalErrorLabel).show();
                    }
                    base.find(settings.submitButton).removeAttr("disabled");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    base.find(settings.generalErrorLabel).html(config.localization.PasswordChange.Error);
                    base.find(settings.generalErrorLabel).show();

                    base.find(settings.submitButton).removeAttr("disabled");
                }
            });
        });
    }
}(jQuery));

// CONTACT FORM

(function ($) {
    $.fn.contactUsForm = function (options) {
        var base = this;

        var settings = $.extend({
            fullNameInput: ".j-full-name",
            fullNameErrorLabel: ".j-full-name-error-message",
            companyNameInput: ".j-company-name",
            emailInput: ".j-email",
            emailErrorLabel: ".j-email-error-message",
            phoneInput: ".j-phone",
            messageInput: ".j-message",
            messageErrorLabel: ".j-message-error-message",
            submitButton: ".j-submit-button",
            generalErrorLabel: ".j-general-error-message",
            attachments: ".js-drop-zone",
            attachmentsNumberErrorMessage: ".j-number-of-attachments-error-message",
            attachmentsSizeErrorMessage: ".j-total-attachments-size-error-message",
            generalErrorTitle: ".j-general-error-title"
        }, options);

        base.find(settings.fullNameInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.companyNameInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.emailInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.phoneInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.submitButton).click(function (e) {
            e.preventDefault();

            base.find(settings.fullNameErrorLabel).hide();
            base.find(settings.fullNameInput).removeClass("input--error");
            base.find(settings.emailErrorLabel).hide();
            base.find(settings.emailInput).removeClass("input--error");
            base.find(settings.messageErrorLabel).hide();
            base.find(settings.messageInput).removeClass("input--error");
            base.find(settings.generalErrorLabel).hide();
            base.find(settings.attachmentsNumberErrorMessage).hide();
            base.find(settings.attachmentsSizeErrorMessage).hide();

            if (base.find(settings.fullNameInput).val() == '') {
                base.find(settings.fullNameInput).addClass("input--error");
                base.find(settings.fullNameErrorLabel).show();
                return;
            }
            if (base.find(settings.emailInput).val() != '' && !IsEmailValid(base.find(settings.emailInput).val())) {
                base.find(settings.emailInput).addClass("input--error");
                base.find(settings.emailErrorLabel).show();
            }

            if (base.find(settings.messageInput).val() == '') {
                base.find(settings.messageInput).addClass("input--error");
                base.find(settings.messageErrorLabel).show();
                return;
            }
            // files count check - there is extra empty file input!
            if (base.find(settings.attachments).find(".js-drop-zone-file").length > 5) {
                base.find(settings.attachmentsNumberErrorMessage).show();
                return;
            }
            // file sizes check - there is extra empty file input!
            if (base.find(settings.attachments).find(".js-drop-zone-file").length > 1) {
                var totalSize = 0;

                base.find(settings.attachments).find(".js-drop-zone-file").each(function (index) {
                    if (index != 0) {
                        totalSize = totalSize + $(this)[0].files[0].size;
                    }
                });
                if (totalSize > 10000000) {
                    base.find(settings.attachmentsSizeErrorMessage).show();
                    return;
                }
            }

            var formData = new FormData()
            formData.append('fullName', base.find(settings.fullNameInput).val());
            formData.append('companyName', base.find(settings.companyNameInput).val());
            formData.append('email', base.find(settings.emailInput).val());
            formData.append('phone', base.find(settings.phoneInput).val());
            formData.append('message', base.find(settings.messageInput).val());

            if (base.find(settings.attachments).find(".js-drop-zone-file").length > 1) {
                base.find(settings.attachments).find(".js-drop-zone-file").each(function (index) {
                    if (index != 0) {
                        formData.append('file' + index, $(this)[0].files[0]);
                    }
                });
            }

            base.find(settings.submitButton).attr("disabled", "disabled");

            var xhr = new XMLHttpRequest();
            xhr.open("POST", base.attr("data-handler"), true);

            xhr.addEventListener("load", function (evt) {
                var target = evt.target || evt.srcElement;
                var data = JSON.parse(target.response);
                if (data.success) {
                    window.location.href = base.attr("data-thank-you-page");
                } else {
                    toastr.error(base.find(settings.generalErrorTitle).html(), data.errorMessage);
                }
                base.find(settings.submitButton).removeAttr("disabled");
            }, false);
            xhr.addEventListener("error", function (evt) {
                toastr.error(base.find(settings.generalErrorTitle).html(), '');

                base.find(settings.submitButton).removeAttr("disabled");
            }, false);

            xhr.send(formData);
        });
    }
}(jQuery));

// SUBMIT BID

(function ($) {
    $.fn.submitBid = function (options) {
        var base = this;

        var settings = $.extend({
            nameInput: ".j-name-input",
            nameErrorMessage: ".j-name-error-message",
            descriptionInput: ".j-description-input",
            descriptionErrorMessage: ".j-description-error-message",
            requestTypeGroup: ".j-request-type-checkbox-group",
            attachments: ".js-drop-zone",
            attachmentsFileExtensionErrorMessage: ".j-invalid-file-extension-error-message",
            attachmentsNumberErrorMessage: ".j-number-of-attachments-error-message",
            attachmentsSizeErrorMessage: ".j-total-attachments-size-error-message",
            biddingWayGroup: ".j-bidding-way-checkbox-group",
            productionDateInput: ".j-production-date",
            productionDateEmptyErrorMessage: ".j-production-date-empty-error-message",
            productionDateInvalidMessage: ".j-production-date-invalid-error-message",
            selectionDateInput: ".j-selection-date",
            selectionDateInvalidMessage: ".j-selection-date-invalid-error-message",
            generalErrorMessage: ".j-general-error-message",
            generalErrorTitle: ".j-general-error-message-title",
            submitButton: ".j-submit-button"
        }, options);

        base.find(settings.nameInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.submitButton).click(function (e) {
            e.preventDefault();

            base.find(settings.nameErrorMessage).hide();
            base.find(settings.nameInput).removeClass("input--error");
            base.find(settings.descriptionErrorMessage).hide();
            base.find(settings.descriptionInput).removeClass("input--error");
            base.find(settings.productionDateEmptyErrorMessage).hide();
            base.find(settings.productionDateInvalidMessage).hide();
            base.find(settings.productionDateInput).removeClass("input--error");
            base.find(settings.selectionDateInvalidMessage).hide();
            base.find(settings.selectionDateInput).removeClass("input--error");

            base.find(settings.attachmentsFileExtensionErrorMessage).hide();
            base.find(settings.attachmentsNumberErrorMessage).hide();
            base.find(settings.attachmentsSizeErrorMessage).hide();

            base.find(settings.generalErrorMessage).hide();

            if (base.find(settings.nameInput).val() == '') {
                base.find(settings.nameInput).addClass("input--error");
                base.find(settings.nameErrorMessage).show();
                return;
            }
            if (base.find(settings.descriptionInput).val() == '') {
                base.find(settings.descriptionInput).addClass("input--error");
                base.find(settings.descriptionErrorMessage).show();
                return;
            }
            // files extensions check - there is extra empty file input!
            if (base.find(settings.attachments).find(".js-drop-zone-file").length > 1) {
                var isOk = true;

                base.find(settings.attachments).find(".js-drop-zone-file").each(function (index) {
                    if (index != 0) {
                        if ($(this)[0].files[0].type != "image/png" && $(this)[0].files[0].type != "image/jpeg" && $(this)[0].files[0].type != "application/pdf") {
                            isOk = false;
                        }
                    }
                });
                if (!isOk) {
                    base.find(settings.attachmentsFileExtensionErrorMessage).show();
                    return;
                }
            }
            // files count check - there is extra empty file input!
            if (base.find(settings.attachments).find(".js-drop-zone-file").length > 5) {
                base.find(settings.attachmentsNumberErrorMessage).show();
                return;
            }
            // file sizes check - there is extra empty file input!
            if (base.find(settings.attachments).find(".js-drop-zone-file").length > 1) {
                var totalSize = 0;

                base.find(settings.attachments).find(".js-drop-zone-file").each(function (index) {
                    if (index != 0) {
                        totalSize = totalSize + $(this)[0].files[0].size;
                    }
                });
                if (totalSize > 10000000) {
                    base.find(settings.attachmentsSizeErrorMessage).show();
                    return;
                }
            }
            if (base.find(settings.productionDateInput).val() == '') {
                base.find(settings.productionDateInput).addClass("input--error");
                base.find(settings.productionDateEmptyErrorMessage).show();
                return;
            }
            // production date format validation

            // skipped - datepicker("getDate") is not valid method to get the date

            //if (base.find(settings.productionDateInput).datepicker("getDate") == null) {
            //    base.find(settings.productionDateInput).addClass("input--error");
            //    base.find(settings.productionDateInvalidMessage).show();
            //    return;
            //}
            // selection date format validation

            // skipped - datepicker("getDate") is not valid method to get the date

            //if (base.find(settings.selectionDateInput).val() != "" && base.find(settings.selectionDateInput).datepicker("getDate") == null) {
            //    base.find(settings.selectionDateInput).addClass("input--error");
            //    base.find(settings.selectionDateInvalidMessage).show();
            //    return;
            //}

            base.find(settings.submitButton).attr("disabled", "disabled");

            var formData = new FormData()
            formData.append('name', base.find(settings.nameInput).val());
            formData.append('description', base.find(settings.descriptionInput).val());
            formData.append('requestType', base.find(settings.requestTypeGroup).find("input:checked").attr("data-value"));
            formData.append('biddingWay', base.find(settings.biddingWayGroup).find("input:checked").attr("data-value"));
            formData.append('biddingWayNumber', base.find(settings.biddingWayGroup).find("input:checked").attr("data-number"));
            //formData.append('productionDate', base.find(settings.productionDateInput).datepicker("getDate").toISOString());
            formData.append('productionDateText', base.find(settings.productionDateInput).val());
            if (base.find(settings.selectionDateInput).val() != '') {
                //formData.append('selectionDate', base.find(settings.selectionDateInput).datepicker("getDate").toISOString());
                formData.append('selectionDateText', base.find(settings.selectionDateInput).val());
            }

            if (base.find(settings.attachments).find(".js-drop-zone-file").length > 1) {
                base.find(settings.attachments).find(".js-drop-zone-file").each(function (index) {
                    if (index != 0) {
                        formData.append('file' + index, $(this)[0].files[0]);
                    }
                });
            }
            var xhr = new XMLHttpRequest();
            xhr.open("POST", base.attr("data-handler"), true);

            xhr.addEventListener("load", function (evt) {
                var target = evt.target || evt.srcElement;
                var data = JSON.parse(target.response);
                if (data.success) {
                    window.location.href = base.attr("data-thank-you-page");
                } else {
                    toastr.error(base.find(settings.generalErrorTitle).html(), data.errorMessage);
                }
                base.find(settings.submitButton).removeAttr("disabled");
            }, false);
            xhr.addEventListener("error", function (evt) {
                base.find(settings.submitButton).removeAttr("disabled");
                toastr.error(base.find(settings.generalErrorTitle).html(), '');
            }, false);

            xhr.send(formData);
        });
    }
}(jQuery));

// REQUEST NEW PRODUCT

(function ($) {
    $.fn.requestNewProduct = function (options) {
        var base = this;

        var settings = $.extend({
            descriptionInput: ".j-description-input",
            descriptionErrorMessage: ".j-description-error-message",
            attachments: ".js-drop-zone",
            attachmentsFileExtensionErrorMessage: ".j-invalid-file-extension-error-message",
            attachmentsNumberErrorMessage: ".j-number-of-attachments-error-message",
            attachmentsSizeErrorMessage: ".j-total-attachments-size-error-message",
            generalErrorMessage: ".j-general-error-message",
            generalErrorTitle: ".j-general-error-title",
            submitButton: ".j-submit-button"
        }, options);

        base.find(settings.submitButton).click(function (e) {
            e.preventDefault();

            base.find(settings.descriptionErrorMessage).hide();
            base.find(settings.descriptionInput).removeClass("input--error");

            base.find(settings.attachmentsFileExtensionErrorMessage).hide();
            base.find(settings.attachmentsNumberErrorMessage).hide();
            base.find(settings.attachmentsSizeErrorMessage).hide();

            base.find(settings.generalErrorMessage).hide();

            if (base.find(settings.descriptionInput).val() == '') {
                base.find(settings.descriptionInput).addClass("input--error");
                base.find(settings.descriptionErrorMessage).show();
                return;
            }
            // files extensions check - there is extra empty file input!
            if (base.find(settings.attachments).find(".js-drop-zone-file").length > 1) {
                var isOk = true;

                base.find(settings.attachments).find(".js-drop-zone-file").each(function (index) {
                    if (index != 0) {
                        if ($(this)[0].files[0].type != "image/png" && $(this)[0].files[0].type != "image/jpeg" && $(this)[0].files[0].type != "application/pdf") {
                            isOk = false;
                        }
                    }
                });
                if (!isOk) {
                    base.find(settings.attachmentsFileExtensionErrorMessage).show();
                    return;
                }
            }
            // files count check - there is extra empty file input!
            if (base.find(settings.attachments).find(".js-drop-zone-file").length > 5) {
                base.find(settings.attachmentsNumberErrorMessage).show();
                return;
            }
            // file sizes check - there is extra empty file input!
            if (base.find(settings.attachments).find(".js-drop-zone-file").length > 1) {
                var totalSize = 0;

                base.find(settings.attachments).find(".js-drop-zone-file").each(function (index) {
                    if (index != 0) {
                        totalSize = totalSize + $(this)[0].files[0].size;
                    }
                });
                if (totalSize > 10000000) {
                    base.find(settings.attachmentsSizeErrorMessage).show();
                    return;
                }
            }

            base.find(settings.submitButton).attr("disabled", "disabled");

            var formData = new FormData()
            formData.append('description', base.find(settings.descriptionInput).val());
            if (base.find(settings.attachments).find(".js-drop-zone-file").length > 1) {
                base.find(settings.attachments).find(".js-drop-zone-file").each(function (index) {
                    if (index != 0) {
                        formData.append('file' + index, $(this)[0].files[0]);
                    }
                });
            }
            var xhr = new XMLHttpRequest();
            xhr.open("POST", base.attr("data-handler"), true);

            xhr.addEventListener("load", function (evt) {
                var target = evt.target || evt.srcElement;
                var data = JSON.parse(target.response);
                if (data.success) {
                    window.location.href = base.attr("data-thank-you-page");
                } else {
                    toastr.error(base.find(settings.generalErrorTitle).html(), data.errorMessage);
                }
                base.find(settings.submitButton).removeAttr("disabled");
            }, false);
            xhr.addEventListener("error", function (evt) {
                base.find(settings.submitButton).removeAttr("disabled");
                toastr.error(base.find(settings.generalErrorTitle).html(), '');
            }, false);

            xhr.send(formData);
        });
    }
}(jQuery));

// REQUEST NEW KIT

(function ($) {
    $.fn.requestNewKitForm = function (options) {
        var base = this;

        var settings = $.extend({
            nameInput: ".j-name-input",
            nameErrorMessage: ".j-name-error-message",
            descriptionInput: ".j-description-input",
            descriptionErrorMessage: ".j-description-error-message",
            productsCheckboxList: ".j-products",
            noProductsSelectedErrorMessage: ".j-no-product-selected-error-message",
            tooManyProductsSelectedErrorMessage: ".j-too-many-products-selected-error-message",
            generalErrorMessage: ".j-general-error-message",
            submitButton: ".j-submit-button"
        }, options);

        base.find(settings.nameInput).keyup(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                base.find(settings.submitButton).trigger("click");
            }
        });

        base.find(settings.submitButton).click(function (e) {
            e.preventDefault();

            base.find(settings.nameErrorMessage).hide();
            base.find(settings.nameInput).removeClass("input--error");
            base.find(settings.descriptionErrorMessage).hide();
            base.find(settings.descriptionInput).removeClass("input--error");
            base.find(settings.noProductsSelectedErrorMessage).hide();
            base.find(settings.tooManyProductsSelectedErrorMessage).hide();
            base.find(settings.generalErrorMessage).hide();

            if (base.find(settings.nameInput).val() == '') {
                base.find(settings.nameInput).addClass("input--error");
                base.find(settings.nameErrorMessage).show();
                return;
            }
            if (base.find(settings.descriptionInput).val() == '') {
                base.find(settings.descriptionInput).addClass("input--error");
                base.find(settings.descriptionErrorMessage).show();
                return;
            }
            if (base.find(settings.productsCheckboxList).find("input:checked").length < 2) {
                base.find(settings.noProductsSelectedErrorMessage).show();
                return;
            }
            if (base.find(settings.productsCheckboxList).find("input:checked").length > 6) {
                base.find(settings.tooManyProductsSelectedErrorMessage).show();
                return;
            }

            var productIDs = [];
            var productNames = [];

            base.find(settings.productsCheckboxList).find("input:checked").each(function () {
                productIDs.push($(this).attr("data-id"));
                productNames.push($(this).next().html());
            });

            var passedData = {
                name: base.find(settings.nameInput).val(),
                description: base.find(settings.descriptionInput).val(),
                productIDs: productIDs,
                productNames: productNames
            };

            base.find(settings.submitButton).attr("disabled", "disabled");

            $.ajax({
                type: "POST",
                url: '/KadenaWebService.asmx/' + base.attr("data-function"),
                data: JSON.stringify(passedData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d.success) {
                        window.location.href = base.attr("data-thank-you-page");
                    } else {
                        base.find(settings.generalErrorLabel).html(data.d.errorMessage);
                        base.find(settings.generalErrorLabel).show();
                    }
                    base.find(settings.submitButton).removeAttr("disabled");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    base.find(settings.generalErrorLabel).html(config.localization.ContactForm.Error);
                    base.find(settings.generalErrorLabel).show();

                    base.find(settings.submitButton).removeAttr("disabled");
                }
            });
        });
    }
}(jQuery));

// K-List init

(function ($) {
    $.fn.setNewKListPage = function (options) {
        var base = this;

        if ($('.j-mailing-list-uploader-error').val() != '') {
            setTimeout(function () { toastr.error(config.localization.newKList.generalErrorTitle, $('.j-mailing-list-uploader-error').val()); }, 0);
        }

        base.click(function (e) {
            if (!$('.js-drop-zone').hasClass('isDropped')) {
                toastr.error(config.localization.newKList.fileNotUploadedTitle, config.localization.newKList.fileNotUploadedText);
                e.preventDefault();
                return;
            };
            if (!$.trim($('input.js-drop-zone-name-input').val()).length) {
                toastr.error(config.localization.newKList.enterValidValueTitle, config.localization.newKList.enterValidValue);
                e.preventDefault();
                return;
            };
        });
    }
}(jQuery));

(function ($) {
    $.fn.KListPage = function (options) {
        var base = this;

        var settings = $.extend({
            errorMessage: ".j-error-message"
        }, options);

        var isSaved = getParameterByName("saved");

        if (isSaved != null && isSaved == '1') {
            setTimeout(function () { toastr.success(config.localization.newKList.listSavedTitle, config.localization.newKList.listSavedText); }, 0);
        }
        if (base.find(settings.errorMessage).val() != '') {
            setTimeout(function () { toastr.error(config.localization.newKList.generalErrorTitle, base.find(settings.errorMessage).val()); }, 0);
        }
    }
}(jQuery));

(function ($) {
    $.fn.KListColumnMappingPage = function (options) {
        var base = this;

        var settings = $.extend({
            errorTitle: ".j-error-title",
            errorMessage: ".j-error-text"
        }, options);

        if (base.find(settings.errorTitle).val() != '') {
            setTimeout(function () { toastr.error(base.find(settings.errorTitle).val(), base.find(settings.errorMessage).val()); }, 0);
        }
    }
}(jQuery));

// BID LIST

(function ($) {
    $.fn.BidList = function (options) {
        var base = this;

        var settings = $.extend({
            errorMessage: ".j-error-message"
        }, options);

        if (base.find(settings.errorMessage).val() != '') {
            setTimeout(function () { toastr.error(config.localization.BidList.generalErrorTitle, base.find(settings.errorMessage).val()); }, 500);
        }
    }
}(jQuery));

// custom script initialization

var customScripts = {
    logoutInit: function () {
        $("#js-logout").logout();
    },
    createPasswordInit: function () {
        if ($(".j-initial-password-setting-form").length > 0) {
            $(".j-initial-password-setting-form").createPassword();
        }
    },
    setPersonContactInformationInit: function () {
        if ($(".j-contant-person-form").length > 0) {
            $(".j-contant-person-form").setPersonContactInformation();
        }
    },
    changePasswordInit: function () {
        if ($(".j-password-change-form").length > 0) {
            $(".j-password-change-form").changePassword();
        }
    },
    contactUsFormInit: function () {
        if ($(".j-contact-us-form").length > 0) {
            $(".j-contact-us-form").contactUsForm();
        }
    },
    submitBidInit: function () {
        if ($(".j-bid-form").length > 0) {
            $(".j-bid-form").submitBid();
        }
    },
    requestNewProductInit: function () {
        if ($(".j-new-product-form").length > 0) {
            $(".j-new-product-form").requestNewProduct();
        }
    },
    requestNewKitInit: function () {
        if ($(".j-new-kit-request-form").length > 0) {
            $(".j-new-kit-request-form").requestNewKitForm();
        }
    },
    newKListInit: function () {
        if ($(".j-submit-mailing-list-button").length > 0) {
            $(".j-submit-mailing-list-button").setNewKListPage();
        }
    },
    kListInit: function () {
        if ($(".j-klist").length > 0) {
            $(".j-klist").KListPage();
        }
    },
    bidListInit: function () {
        if ($(".j-bid-list").length > 0) {
            $(".j-bid-list").BidList();
        }
    },
    kListColumnMappingInit: function () {
        if ($(".j-klist-column-mapper").length > 0) {
            $(".j-klist-column-mapper").KListColumnMappingPage();
        }
    },
    init: function () {
        var base = this;
        base.logoutInit();
        base.createPasswordInit();
        base.setPersonContactInformationInit();
        base.changePasswordInit();
        base.submitBidInit();
        base.contactUsFormInit();
        base.requestNewProductInit();
        base.requestNewKitInit();
        base.newKListInit();
        base.kListInit();
        base.bidListInit();
        base.kListColumnMappingInit();
    }
}
var customHelpers = {
    getQueryStringByName: function (name) {
        url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return "";
        if (!results[2]) return "";
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    },
    getCookie: function (cookieName) {
        var pattern = RegExp(cookieName + "=.[^;]*")
        matched = document.cookie.match(pattern);
        if (matched) {
            var cookie = matched[0].split('=');
            return cookie[1];
        }
        return false
    },
    deleteCookie: function (name) {
        document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;path=/';
    }
}
$(document).ready(function () {
    customScripts.init();
    var status = customHelpers.getQueryStringByName("status");
    var oldLoc = document.referrer;
    var newLoc = window.location.href;
    var oldPageURL = oldLoc != "" ? oldLoc.split('?')[0] : "";
    var newPageURL = newLoc != "" ? newLoc.split('?')[0] : "";
    var isSame = oldPageURL == newPageURL ? true : false;
    var page = customHelpers.getQueryStringByName("page");
    var cookieValue = customHelpers.getCookie("status");
    switch (cookieValue) {
        case 'added': toastr.success(config.localization.globalSuccess.addSuccessMessage);
            break;
        case 'updated': toastr.success(config.localization.globalSuccess.updateSuccessMessage);
            break;
        case 'deleted': toastr.success(config.localization.globalSuccess.deleteSuccessMessage);
            break;
        case 'ordertask': toastr.success(config.localization.orders.orderScheduleTaskStartMessage);
            break;
        case 'mappederror': toastr.error(config.localization.globalSuccess.pOSMappedWithProductError);
            break;
        case 'error': toastr.error(config.localization.globalSuccess.errorMessage);
            break;
        case 'ordersuccess': toastr.success(config.localization.orders.orderSuccessMessage);
            break;
        case 'invalidcartitems': toastr.error(config.localization.orders.cartContainsInvalidProducts);
            break;
        case 'invalidproduct': toastr.error(config.localization.orders.invalidProduct);
            break;
    }
    var errorCookie = customHelpers.getCookie("error");
    if (errorCookie == "orderfail") {
        toastr.error(config.localization.orders.orderErrorMessage);
    }
    if (status == 'error') {
        toastr.error(config.localization.globalSuccess.errorMessage);
    }
    customHelpers.deleteCookie("status");
    customHelpers.deleteCookie("error");


    if (window.location.search != undefined) {

        var searchText = customHelpers.getQueryStringByName("searchtext");
        if (searchText != undefined) {
            $('.js-SearchText').val(searchText);
        }
    }
});
