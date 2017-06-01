//Function to close alert box
$('.close').click(function () {
    var Parent = $(this).parent('div')
    if ($(Parent).hasClass('display')) {
        $(Parent).removeClass('display')
        $(Parent).addClass('display-hide')
    }

});

//Function to clear all the fields inside div
function ClearAllFields(Container) {
    var TextInputs = $(Container).find('input:text')
    var PasswordInputs = $(Container).find('input:password')
    var DropDowns = $(Container).find('select')
    var Textarea = $(Container).find('textarea')
    var RadioButton = $(Container).find('input:radio').parent('span.checked')
    var CheckBox = $(Container).find('input:checkbox').parent('span.checked')
    $(TextInputs).each(function () {
        $(this).val('')
    });
    $(PasswordInputs).each(function () {
        $(this).val('')
    });
    $(DropDowns).each(function () {
        $(this).val(0)
    });
    $(Textarea).each(function () {
        $(this).val('');
    });

    $(RadioButton).each(function () {
        $(this).removeClass('checked');
    });

    $(CheckBox).each(function () {
        $(this).removeClass('checked');
    });
}

//Function to populate dropdown
function GetDDData(URL, Data, targetDDlId) {
    $.post(URL, Data, function (Result) {
        if (PopulateDropdown(Result.ListObject, targetDDlId)) {
            return true;
        } else {
            return false;
        }
        
    })
}
function PopulateDropdown(ListObject, ddlId) {
    var FirstItem = $(ddlId).find('option[value="0"]').text();
    $(ddlId).empty();
    $(ddlId).append('<option selected="selected" value="0">' + FirstItem + '</option>')
    $.each(ListObject, function (index, value) {
        $(ddlId).append('<option value="' + value.DDValue + '">' + value.DDText + '</option>')
    });

    return true;
}

function ValidateEmail(emailid) {
    var pattern = new RegExp(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/);
    return pattern.test(emailid);
};
function ValidatePhone(phone) {
    var expr = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    return expr.test(phone);
};
function ValidateUserName(username) {
    var expr = /^\w+$/;
    return expr.test(username);
};
function ValidatePassword(password) {
    var expr = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}/;
    return expr.test(password);
};
function ValidateOnlyDigits(item) {
    var pattern = new RegExp(/^\d+$/);
    return pattern.test(item);
}

//Function to Validate all fields base on theit ids
// Validate("comma seperated control ids", "comma seperated error message in same order as ids, target element in which error message has to be shown")
function Validate(inputControls, ErrorMessages, targetElement) {
    var splittedElements = inputControls.split(",");
    var SplittedMessage = ErrorMessages.split(",");
    var TargetElement = $(targetElement);
    var haserror = false;
    $.each(splittedElements, function (i) {
        var Element = $('#' + splittedElements[i])
        var controltype = $(Element).attr('type')
        if (typeof controltype === "undefined") {
            controltype = $(Element).prop("tagName")
        }
        switch (controltype) {
            case "text":
                if (!ValidateTextControl(Element)) {
                    $(TargetElement).html(SplittedMessage[i])
                    haserror = true;
                };
                break;
            case "password":
                if (!ValidateTextControl(Element)) {
                    $(TargetElement).html(SplittedMessage[i])
                    haserror = true;
                };
                break;
            case "TEXTAREA":
                if (!ValidateTextControl(Element)) {
                    $(TargetElement).html(SplittedMessage[i])
                    haserror = true;
                };
                break;
            case "SELECT":
                if (!ValidatedropdownControl(Element)) {
                    $(TargetElement).html(SplittedMessage[i])
                    haserror = true;
                };
                break;
            default:
                return false;
        }

        if (haserror) {
            return false;
        }
    });
    if (haserror)
        return false;
    else
        return true;
}
function ValidateTextControl(textbox) {
    var value = $(textbox).val();
    
    if (value == "") {
        return false;
    } else {
        return true;
    }
}
function ValidatedropdownControl(dropdown) {
    var value = $(dropdown).val();
    if (value == "0") {
        return false;
    } else {
        return true;
    }
}
//Function Validate ends here