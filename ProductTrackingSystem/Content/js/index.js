$(document).ready(function () {
    $("#selectbox2").hide();
});

function changed() {
    var index = document.getElementById("selectbox").selectedIndex;

    if (index == 2)
        $("#selectbox2").show();
    else
        $("#selectbox2").hide();
}



