// Globala konstanter och variabler
let detailsDescDiv;
let detailsShowDescBtn
// --------------------------------------------------
// Initiera globala variabler och händelsehanterare
function init() {
    detailsShowDescBtn = document.getElementById('details-show-desc-btn');
    detailsDescDiv = document.getElementById('details-desc-toggle-div');
    console.log(detailsShowDescBtn);
    detailsShowDescBtn.addEventListener("click", toggleDetailsDesc)
} // Slut init
window.addEventListener('load', init);
// --------------------------------------------------

function toggleDetailsDesc() {
    console.log("funkar");
    if (detailsDescDiv.classList.contains("hidden")) {
        detailsShowDescBtn.classList.remove("details-desc-div-btn-dormant");
        detailsDescDiv.classList.remove("hidden");
    } else {
        detailsShowDescBtn.classList.add("details-desc-div-btn-dormant");
        detailsDescDiv.classList.add("hidden");
    }
}