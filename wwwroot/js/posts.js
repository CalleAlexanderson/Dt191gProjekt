// Globala konstanter och variabler
let detailsDescDiv;
let detailsShowDescBtn
let detailsCollectionShowBtn
let detailsCollectionDiv
// --------------------------------------------------
// Initiera globala variabler och händelsehanterare
function postsInit() {
    detailsShowDescBtn = document.getElementById('details-show-desc-btn');
    detailsDescDiv = document.getElementById('details-desc-toggle-div');
    detailsCollectionShowBtn = document.getElementById('post-details-collection-button');
    detailsCollectionDiv = document.getElementById('post-details-collection-div');

    if (detailsShowDescBtn != null) {
        detailsShowDescBtn.addEventListener("click", toggleDetailsDesc)
    }

    if (detailsCollectionShowBtn != null) {
        detailsCollectionShowBtn.addEventListener("click", toggleDetailsCollection)
    }

} // Slut init
window.addEventListener('load', postsInit);
// --------------------------------------------------

function toggleDetailsCollection() {
    console.log("klockar på colleciton");
    if (detailsCollectionDiv.classList.contains("hidden")) {
        detailsCollectionShowBtn.classList.remove("post-details-collection-button-dormant");
        detailsCollectionDiv.classList.remove("hidden");
    } else {
        detailsCollectionShowBtn.classList.add("post-details-collection-button-dormant");
        detailsCollectionDiv.classList.add("hidden");
    }
}

function toggleDetailsDesc() {
    if (detailsDescDiv.classList.contains("hidden")) {
        detailsShowDescBtn.classList.remove("details-desc-div-btn-dormant");
        detailsDescDiv.classList.remove("hidden");
    } else {
        detailsShowDescBtn.classList.add("details-desc-div-btn-dormant");
        detailsDescDiv.classList.add("hidden");
    }
}