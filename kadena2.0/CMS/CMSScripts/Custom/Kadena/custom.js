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
                    window.location.reload(false);
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
            if (base.find(settings.confirmPasswordInput).val() == '') {
                base.find(settings.confirmPasswordInput).addClass("input--error");
                base.find(settings.confirmPasswordErrorLabel).html(config.localization.initialPasswordSetting.passwordEmptyValidationMessage);
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
                        window.location.href = window.location.href;
                    } else {
                        base.find(settings.errorMassage).html(data.d.errorMessage);
                        base.find(settings.errorMassage).show();
                    }
                    base.find(settings.submitButton).removeAttr("disabled");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    base.find(settings.errorMassage).html(config.localization.ContactPersonDetailsChange.Error);
                    base.find(settings.errorMassage).show();

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

            oldPasswordErrorLabel: ".j-old-password-error-label",
            newPasswordErrorLabel: ".j-new-password-error-label",
            confirmPasswordErrorLabel: ".j-confirm-password-error-label",
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

            base.find(settings.oldPasswordErrorLabel).hide();
            base.find(settings.oldPasswordInput).removeClass("input--error");
            base.find(settings.newPasswordErrorLabel).hide();
            base.find(settings.newPasswordInput).removeClass("input--error");
            base.find(settings.confirmPasswordErrorLabel).hide();
            base.find(settings.confirmPasswordInput).removeClass("input--error");
            base.find(settings.passwordsDontMatchErrorLabel).hide();
            base.find(settings.generalErrorLabel).hide();

            if (base.find(settings.oldPasswordInput).val() == '') {
                base.find(settings.oldPasswordInput).addClass("input--error");
                base.find(settings.oldPasswordErrorLabel).show();
                return;
            }
            if (base.find(settings.newPasswordInput).val() == '') {
                base.find(settings.newPasswordInput).addClass("input--error");
                base.find(settings.newPasswordErrorLabel).show();
                return;
            }
            if (base.find(settings.confirmPasswordInput).val() == '') {
                base.find(settings.confirmPasswordInput).addClass("input--error");
                base.find(settings.confirmPasswordErrorLabel).show();
                return;
            }
            if (base.find(settings.newPasswordInput).val() != base.find(settings.confirmPasswordInput).val()) {
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
                        alert(config.localization.PasswordChange.Success);
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
                    base.find(settings.generalErrorLabel).html(data.errorMessage);
                    base.find(settings.generalErrorLabel).show();
                }
                base.find(settings.submitButton).removeAttr("disabled");
            }, false);
            xhr.addEventListener("error", function (evt) {
                base.find(settings.generalErrorLabel).html(config.localization.ContactForm.Error);
                base.find(settings.generalErrorLabel).show();

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
            if (base.find(settings.productionDateInput).datepicker("getDate") == null) {
                base.find(settings.productionDateInput).addClass("input--error");
                base.find(settings.productionDateInvalidMessage).show();
                return;
            }
            // selection date format validation
            if (base.find(settings.selectionDateInput).val() != "" && base.find(settings.selectionDateInput).datepicker("getDate") == null) {
                base.find(settings.selectionDateInput).addClass("input--error");
                base.find(settings.selectionDateInvalidMessage).show();
                return;
            }

            base.find(settings.submitButton).attr("disabled", "disabled");

            var formData = new FormData()
            formData.append('name', base.find(settings.nameInput).val());
            formData.append('description', base.find(settings.descriptionInput).val());
            formData.append('requestType', base.find(settings.requestTypeGroup).find("input:checked").attr("data-value"));
            formData.append('biddingWay', base.find(settings.biddingWayGroup).find("input:checked").attr("data-value"));
            formData.append('biddingWayNumber', base.find(settings.biddingWayGroup).find("input:checked").attr("data-number"));
            formData.append('productionDate', base.find(settings.productionDateInput).datepicker("getDate").toISOString());
            if (base.find(settings.selectionDateInput).val() != '') {
                formData.append('selectionDate', base.find(settings.selectionDateInput).datepicker("getDate").toISOString());
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
                    base.find(settings.generalErrorMessage).html(data.errorMessage);
                    base.find(settings.generalErrorMessage).show();
                }
                base.find(settings.submitButton).removeAttr("disabled");
            }, false);
            xhr.addEventListener("error", function (evt) {
                base.find(settings.submitButton).removeAttr("disabled");
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
                    base.find(settings.generalErrorMessage).html(data.errorMessage);
                    base.find(settings.generalErrorMessage).show();
                }
                base.find(settings.submitButton).removeAttr("disabled");
            }, false);
            xhr.addEventListener("error", function (evt) {
                base.find(settings.submitButton).removeAttr("disabled");
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

// custom script initialization

var customScripts = {
    logoutInit: function () {
        $("#js-logout").logout();
    },
    createPasswordInit: function () {
        $(".j-initial-password-setting-form").createPassword();
    },
    setPersonContactInformationInit: function () {
        $(".j-contant-person-form").setPersonContactInformation();
    },
    changePasswordInit: function () {
        $(".j-password-change-form").changePassword();
    },
    contactUsFormInit: function () {
        $(".j-contact-us-form").contactUsForm();
    },
    submitBidInit: function () {
        $(".j-bid-form").submitBid();
    },
    requestNewProductInit: function () {
        $(".j-new-product-form").requestNewProduct();
    },
    requestNewKitInit: function () {
        $(".j-new-kit-request-form").requestNewKitForm();
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
    }
}

$(document).ready(function () {
    customScripts.init();
});