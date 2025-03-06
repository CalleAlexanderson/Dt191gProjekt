// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Globala konstanter och variabler

// --------------------------------------------------
// Initiera globala variabler och händelsehanterare
function init() {
    let count = 0;
    let navbtn = document.getElementById('nav-toggle-btn');
    let navDiv = document.getElementById('nav-div');
    let dropDownBtn = document.getElementById('profile-button');
    let dropDown = document.getElementById('profile-dropdown');
    console.log(dropDown);
    console.log(navDiv);

    dropDownBtn.addEventListener("mouseenter", () => {
        setTimeout(() => {
            dropDown.classList.remove("nav-hidden");
        }, 250);
    })
    dropDownBtn.addEventListener("mouseleave", () => {
        dropDown.classList.add("nav-hidden");
    })

    dropDown.addEventListener("mouseenter", () => {
        dropDown.classList.remove("nav-hidden");
    })
    dropDown.addEventListener("mouseleave", () => {
        dropDown.classList.add("nav-hidden");
    })

    navbtn.addEventListener("click", () => {
        count++;
        console.log("funkar");
        console.log(count);
        if (navDiv.classList.contains("nav-hidden")) {
            navDiv.classList.remove("nav-hidden")
        } else {
            navDiv.classList.add("nav-hidden")
        }
    })

} // Slut init
window.addEventListener('load', init);
// --------------------------------------------------

async function name(params) {
    sleep
}
