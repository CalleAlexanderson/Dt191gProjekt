// Globala konstanter och variabler
let detailsDescDiv;
let detailsShowDescBtn
// --------------------------------------------------
// Initiera globala variabler och händelsehanterare
function postsInit() {
    detailsShowDescBtn = document.getElementById('details-show-desc-btn');
    detailsDescDiv = document.getElementById('details-desc-toggle-div');

    if (detailsShowDescBtn != null) {
        detailsShowDescBtn.addEventListener("click", toggleDetailsDesc)
    }

} // Slut init
window.addEventListener('load', postsInit);
// --------------------------------------------------

function toggleDetailsDesc() {
    if (detailsDescDiv.classList.contains("hidden")) {
        detailsShowDescBtn.classList.remove("details-desc-div-btn-dormant");
        detailsDescDiv.classList.remove("hidden");
    } else {
        detailsShowDescBtn.classList.add("details-desc-div-btn-dormant");
        detailsDescDiv.classList.add("hidden");
    }
}