// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Globala konstanter och variabler

// --------------------------------------------------
// Initiera globala variabler och händelsehanterare
function init() {
    let count = 0;
    let navbtn = document.getElementById('nav-toggle-btn');
    let navDiv = document.getElementById('nav-div');
    let dropDownBtn = document.getElementById('profile-btn');
    let dropDown = document.getElementById('profile-dropdown');
    console.log(dropDown);
    console.log(navDiv);

    dropDownBtn.addEventListener("mouseenter", () => {
        setTimeout(() => {
            dropDown.classList.remove("hidden");
        }, 250);
    })
    dropDownBtn.addEventListener("mouseleave", () => {
        dropDown.classList.add("hidden");
    })

    dropDown.addEventListener("mouseenter", () => {
        dropDown.classList.remove("hidden");
    })
    dropDown.addEventListener("mouseleave", () => {
        dropDown.classList.add("hidden");
    })

    navbtn.addEventListener("click", () => {
        count++;
        console.log("funkar");
        console.log(count);
        if (navDiv.classList.contains("hidden")) {
            navDiv.classList.remove("hidden")
        } else {
            navDiv.classList.add("hidden")
        }
    })

} // Slut init
window.addEventListener('load', init);
// --------------------------------------------------

async function name(params) {
    sleep
}
