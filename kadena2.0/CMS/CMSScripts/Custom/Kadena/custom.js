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

// custom script initialization

var customScripts = {
    logoutInit: function () {
        $("#js-logout").logout();
    },
    createPasswordInit: function () {
        $(".j-initial-password-setting-form").createPassword();
    },
    init: function () {
        var base = this;

        base.logoutInit();
        base.createPasswordInit();
    }
}

$(document).ready(function () {
    customScripts.init();
});