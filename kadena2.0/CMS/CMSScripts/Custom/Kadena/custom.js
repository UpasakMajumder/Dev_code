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
            generalErrorLabel: ".j-general-error-message"
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

            var passedData = {
                fullName: base.find(settings.fullNameInput).val(),
                companyName: base.find(settings.companyNameInput).val(),
                email: base.find(settings.emailInput).val(),
                phone: base.find(settings.phoneInput).val(),
                message: base.find(settings.messageInput).val()
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
                        // redirect to message sent page
                    } else {
                        //base.find(settings.generalErrorLabel).html(data.d.errorMessage);
                        ///base.find(settings.generalErrorLabel).show();
                    }
                    base.find(settings.submitButton).removeAttr("disabled");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //base.find(settings.generalErrorLabel).html(config.localization.PasswordChange.Error);
                    //base.find(settings.generalErrorLabel).show();

                    //base.find(settings.submitButton).removeAttr("disabled");
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
    init: function () {
        var base = this;

        base.logoutInit();
        base.createPasswordInit();
        base.setPersonContactInformationInit();
        base.changePasswordInit();
        base.contactUsFormInit();
    }
}

$(document).ready(function () {
    customScripts.init();
});