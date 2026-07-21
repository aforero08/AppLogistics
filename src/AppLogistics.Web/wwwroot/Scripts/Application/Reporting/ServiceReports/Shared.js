$(function () {
    $("#StartDate").datepicker({
        changeMonth: true,
        changeYear: true,
        defaultDate: "-7d",
        firstDay: 0,
        showButtonPanel: true,
        showOtherMonths: true,
        selectOtherMonths: true,
        yearRange: "c-4:c+2"
    });
        //.datepicker("setDate", "-7d");

    $("#EndDate").datepicker({
        changeMonth: true,
        changeYear: true,
        firstDay: 0,
        showButtonPanel: true,
        showOtherMonths: true,
        selectOtherMonths: true,
        yearRange: "c-4:c+2"
    });
        //.datepicker("setDate", "+0d");

    document.getElementById("ServiceId")?.focus();

});
