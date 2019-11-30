$(document).ready(function () {
    if (document.getElementById("selectListType").value != 3)
        $("#selectListMonth").hide();
});

function changed() {
    var index = document.getElementById("selectListType").value;

    if (index == 3)
        $("#selectListMonth").show();
    else
        $("#selectListMonth").hide();
}